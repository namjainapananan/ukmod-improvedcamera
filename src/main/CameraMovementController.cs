
using System.Collections.Generic;
using UnityEngine;

namespace ukmod_improvedcamera.src.main
{
    public class CameraMovementController : MonoSingleton<CameraMovementController>
    {
        public Vector3 FinalResult;
        public List<Vector3> currentFrameVectorList = new List<Vector3>();
        public CameraShakeEffect camShakeEffect;
        public ViewRollEffect viewRollEffect;

        void Awake()
        {
            camShakeEffect = gameObject.AddComponent<CameraShakeEffect>();
            viewRollEffect = gameObject.AddComponent<ViewRollEffect>();
        }

        void Update()
        {
            Vector3 sumVector = Vector3.zero;
            foreach (var effect in currentFrameVectorList)
            {
                sumVector += effect;
            }
            currentFrameVectorList.Clear();

            FinalResult = sumVector;
        }
    }
}
