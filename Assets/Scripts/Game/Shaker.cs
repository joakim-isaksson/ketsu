using EZCameraShake;
using UnityEngine;

namespace Game
{
    public class Shaker : MonoBehaviour
    {
        public float Magnitude = 2f;
        public float Roughness = 10f;
        public float FadeOutTime = 5f;

        public void Shake()
        {
            CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, 0, FadeOutTime);
        }
    }
}