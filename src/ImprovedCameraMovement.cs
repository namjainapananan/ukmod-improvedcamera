
using System.Collections.Generic;
using ukmod_improvedcamera.src.modules;
using ukmod_improvedcamera.src.util;
using UnityEngine;

namespace ukmod_improvedcamera.src
{
    public class ImprovedCameraMovement : MonoBehaviour
    {
        public static ImprovedCameraMovement Instance { get; private set; }
        public Vector3 FinalResult;
        public List<ICameraEffect> allEffects = new List<ICameraEffect>();

        public ViewRoll viewRollEffect = new ViewRoll();
        void Awake()
        {
            RegisterInstance();
            allEffects.Add(viewRollEffect);
        }
        void Start()
        {
            foreach (var effect in allEffects)
            {
                effect.Start();
            }
        }
        void Update()
        {
            Vector3 sumVector = Vector3.zero;
            foreach (var effect in allEffects)
            {
                effect.Update(Time.deltaTime);
                sumVector += effect.GetOffset();
            }

            FinalResult = sumVector;
        }

        void RegisterInstance()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}
