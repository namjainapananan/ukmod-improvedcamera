
using System.Collections.Generic;
using UnityEngine;

namespace ukmod_improvedcamera.src.main
{
    public class CameraShakeInstance
    {
        public float Amplitude;
        public float Frequency;
        public float Duration;
        public float FadeInDuration;
        public float FadeOutDuration;

        public float Age;
        public bool IsDestroyed()
        {
            return Age > Duration;
        }

        public CameraShakeInstance(float amplitude, float frequency, float duration, float fadeIn, float fadeOut)
        {
            Amplitude = amplitude;
            Frequency = frequency;
            Duration = duration;
            FadeInDuration = fadeIn;
            FadeOutDuration = fadeOut;
            Age = 0f;
        }

        public void Update(float deltaTime)
        {
            Age += deltaTime;
        }

        public Vector3 GetCurrentOffset()
        {
            if (IsDestroyed()) return Vector3.zero;

            float timeValue = Age * Frequency;
            float x = Mathf.Sin(timeValue) * MainPluginModule.pluginConfigVar.screenshakePitch.value;
            float y = Mathf.Cos(timeValue / 2) * MainPluginModule.pluginConfigVar.screenshakeYaw.value;
            float z = Mathf.Sin(timeValue / 2) * MainPluginModule.pluginConfigVar.screenshakeRoll.value;

            float envelope = 1f;

            if (Age < FadeInDuration)
            {
                envelope = Age / FadeInDuration;
            }
            else if (Age > (Duration - FadeOutDuration))
            {
                float timeRemaining = Duration - Age;
                envelope = timeRemaining / FadeOutDuration;
            }

            // Combine everything
            return new Vector3(x, y, z) * Amplitude * envelope;
        }
    }

    public class CameraShakeEffect : MonoBehaviour
    {
        private List<CameraShakeInstance> activeShakes = new List<CameraShakeInstance>();
        public void StartBasicShake( float shakeAmount )
        {
            CameraShakeInstance newShake = new CameraShakeInstance(shakeAmount / 2, MainPluginModule.pluginConfigVar.screenshakeFrequency.value, Mathf.Max(shakeAmount / 3, 0.5f), 0.05f, 0.275f);
            activeShakes.Add(newShake);
        }
        public void StartShake(float amplitude, float frequency, float duration, float fadein, float fadeout)
        {
            CameraShakeInstance newShake;
            if (frequency == -1)
            {
                newShake = new CameraShakeInstance(amplitude, MainPluginModule.pluginConfigVar.screenshakeFrequency.value, duration, fadein, fadeout);
            }
            else
            {
                newShake = new CameraShakeInstance(amplitude, frequency, duration, fadein, fadeout);
            }

            activeShakes.Add(newShake);
        }
        void Update()
        {
            Vector3 totalOffset = Vector3.zero;
            for (int i = activeShakes.Count - 1; i >= 0; i--)
            {
                CameraShakeInstance shake = activeShakes[i];
                shake.Update(Time.unscaledDeltaTime);

                if (shake.IsDestroyed())
                {
                    activeShakes.RemoveAt(i);
                }
                else
                {
                    // This is where the blending happens! Just sum the vectors.
                    totalOffset += shake.GetCurrentOffset();
                }
            }
            totalOffset *= MainPluginModule.pluginConfigVar.screenshakeMaster.value;
            if (MainPluginModule.pluginConfigVar.enabledScreenshake.value)
            {
                MonoSingleton<CameraMovementController>.Instance.currentFrameVectorList.Add(totalOffset);
            }
        }
    }
}
