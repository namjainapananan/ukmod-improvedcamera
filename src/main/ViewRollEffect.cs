

using System.Runtime.ConstrainedExecution;
using ukmod_improvedcamera.src.util;
using ULTRAKILL.Portal;
using UnityEngine;

namespace ukmod_improvedcamera.src.main
{
    public class ViewRollEffect : MonoBehaviour
    {
        Rigidbody rb;
        Vector3 previousRBPosition;
        Vector3 currentRBPosition;
        SpringVector3 trackingSpring = new SpringVector3(Vector3.zero, Vector3.zero);
        Vector3 SpringOffsetBeforePortal;

        void Awake()
        {
            rb = transform.parent.GetComponent<Rigidbody>();
            previousRBPosition = rb.position;
            currentRBPosition = rb.position;
        }
        void Start()
        {
            trackingSpring.Position = rb.position;
        }
        void FixedUpdate()
        {
            previousRBPosition = currentRBPosition;
            currentRBPosition = rb.position;
        }
        public Vector3 GetInterpolatedPosition(Vector3 prev, Vector3 cur)
        {
            // 1. Calculate the interpolation alpha (0.0 to 1.0)
            float alpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
            alpha = Mathf.Clamp01(alpha); // Safety clamp

            // 2. Linearly interpolate between the two physics frames
            return Vector3.Lerp(prev, cur, alpha);
        }
        void Update()
        {
            trackingSpring.frequency = MainPluginModule.pluginConfigVar.viewtiltRecoveryFrequency.value;
            trackingSpring.dampingRatio = MainPluginModule.pluginConfigVar.viewtiltRecoveryDamping.value;
            trackingSpring.Target = GetInterpolatedPosition(previousRBPosition, currentRBPosition);
            trackingSpring.Update(Time.deltaTime);

            float pitch = Vector3.Dot(trackingSpring.Position - trackingSpring.Target, transform.up) * MainPluginModule.pluginConfigVar.viewtiltPitch.value;
            float yaw = Vector3.Dot(trackingSpring.Position - trackingSpring.Target, transform.right) * -0.5f * MainPluginModule.pluginConfigVar.viewtiltYaw.value;
            float roll = Vector3.Dot(trackingSpring.Position - trackingSpring.Target, transform.right) * MainPluginModule.pluginConfigVar.viewtiltRoll.value;
            Vector3 totalOffset = new Vector3(pitch, yaw, roll);
            totalOffset *= MainPluginModule.pluginConfigVar.viewtiltMaster.value;
            if (MainPluginModule.pluginConfigVar.enabledViewtilt.value)
            {
                MonoSingleton<CameraMovementController>.Instance.currentFrameVectorList.Add(totalOffset);
            }
            SpringOffsetBeforePortal = trackingSpring.Position - trackingSpring.Target;
        }
        public void OnPortalTravesal(PortalTravelDetails details)
        {
            trackingSpring.Position = rb.position + SpringOffsetBeforePortal;
            trackingSpring.Target = rb.position;
            previousRBPosition = rb.position;
            currentRBPosition = rb.position;
        }
    }
}
