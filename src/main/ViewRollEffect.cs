

using ukmod_improvedcamera.src.util;
using ULTRAKILL.Portal;
using UnityEngine;

namespace ukmod_improvedcamera.src.main
{
    public class ViewRollEffect : MonoBehaviour
    {
        Rigidbody rb;
        Vector3 rbPosTarget;
        SpringVector3 trackingSpring = new SpringVector3(Vector3.zero, Vector3.zero);
        Vector3 SpringOffsetBeforePortal;

        void Awake()
        {
            rb = transform.parent.GetComponent<Rigidbody>();
        }
        void Start()
        {
            trackingSpring.Position = rb.position;
        }
        void FixedUpdate()
        {
            rbPosTarget = rb.position;
        }
        void Update()
        {
            trackingSpring.frequency = 2.5f;
            trackingSpring.dampingRatio = 0.5f;
            trackingSpring.Target = rbPosTarget;
            trackingSpring.Update(Time.deltaTime);

            float pitch = Vector3.Dot(trackingSpring.Position - trackingSpring.Target, transform.up);
            float yaw = Vector3.Dot(trackingSpring.Position - trackingSpring.Target, transform.right) * -0.5f;
            float roll = Vector3.Dot(trackingSpring.Position - trackingSpring.Target, transform.right);
            Vector3 totalOffset = new Vector3(pitch, yaw, roll);
            if (gameObject.GetComponent<CameraController>().tilt)
            {
                MonoSingleton<CameraMovementController>.Instance.currentFrameVectorList.Add(totalOffset);
            }
            SpringOffsetBeforePortal = trackingSpring.Position - trackingSpring.Target;
        }
        public void OnPortalTravesal(PortalTravelDetails details)
        {
            trackingSpring.Position = rb.position + SpringOffsetBeforePortal;
            trackingSpring.Target = rb.position;
            rbPosTarget = rb.position;
        }
    }
}
