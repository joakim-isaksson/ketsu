using EZCameraShake;
using UnityEngine;

namespace Game
{
    public class Shaker : MonoBehaviour
    {
        [Header("Light Shake")]
        public float LMagnitude = 1f;
        public float LRoughness = 3f;
        public float LFadeOutTime = 0.5f;

        [Header("Medium Shake")]
        public float MMagnitude = 2;
        public float MRoughness = 5f;
        public float MFadeOutTime = 0.75f;

        [Header("Heavy Shake")]
        public float HMagnitude = 5;
        public float HRoughness = 10f;
        public float HFadeOutTime = 3.0f;

        public void LightShake()
        {
            CameraShaker.Instance.ShakeOnce(LMagnitude, LRoughness, 0, LFadeOutTime);
        }

        public void MediumShake()
        {
            CameraShaker.Instance.ShakeOnce(MMagnitude, MRoughness, 0, MFadeOutTime);
        }

        public void HeavyShake()
        {
            CameraShaker.Instance.ShakeOnce(HMagnitude, HRoughness, 0, HFadeOutTime);
        }
    }
}