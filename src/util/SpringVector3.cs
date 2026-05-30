

using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace ukmod_improvedcamera.src.util
{
    public class SpringVector3
    {
        public float frequency = 5f;
        public float dampingRatio = 0.5f;
        public Vector3 Target;
        public Vector3 Position;
        public Vector3 Velocity;

        public SpringVector3(Vector3 startingPosition, Vector3 target)
        {
            Position = startingPosition;
            Target = target;
            Velocity = Vector3.zero;
        }
        public void Reset()
        {
            Position = Target;
            Velocity = Vector3.zero;
        }
        public void Update(float deltaTime)
        {
            if (deltaTime <= 0f) return;

            float angularFrequency = frequency * 2f * Mathf.PI;

            Vector3 displacement = Position - Target;

            Vector3 acceleration = (-angularFrequency * angularFrequency * displacement)
                                 - (2f * dampingRatio * angularFrequency * Velocity);

            Velocity += acceleration * deltaTime;
            Position += Velocity * deltaTime;
        }
    }
}
