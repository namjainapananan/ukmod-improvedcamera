
using UnityEngine;

namespace ukmod_improvedcamera.src.util
{
    public class SpringFloat
    {
        public float frequency = 5f;
        public float dampingRatio = 0.5f;
        public float Target;
        public float Position;
        public float Velocity;

        public SpringFloat(float startingPosition, float target)
        {
            Position = startingPosition;
            Target = target;
            Velocity = 0;
        }
        public void Reset()
        {
            Position = Target;
            Velocity = 0;
        }
        public void Update(float deltaTime)
        {
            if (deltaTime <= 0f) return;

            float angularFrequency = frequency * 2f * Mathf.PI;

            float displacement = Position - Target;

            float acceleration = (-angularFrequency * angularFrequency * displacement)
                                 - (2f * dampingRatio * angularFrequency * Velocity);

            Velocity += acceleration * deltaTime;
            Position += Velocity * deltaTime;
        }
    }
}
