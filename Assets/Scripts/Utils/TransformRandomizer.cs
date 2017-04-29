using UnityEngine;

namespace Utils
{
    public class TransformRandomizer : MonoBehaviour
    {
        [Header("Position")]
        public Vector3 PositionOffsetMin;
        public Vector3 PositionOffsetMax;

        [Header("Rotation")]
        public Vector3 RotationOffsetMin;
        public Vector3 RotationOffsetMax;

        [Header("Scale")]
        public bool KeepScaleRatio;
        public Vector3 ScaleMultiplierMin;
        public Vector3 ScaleMultiplierMax;

        void Awake()
        {
            transform.position = new Vector3(
                transform.position.x + Random.Range(PositionOffsetMin.x, PositionOffsetMax.x),
                transform.position.y + Random.Range(PositionOffsetMin.y, PositionOffsetMax.y),
                transform.position.z + Random.Range(PositionOffsetMin.z, PositionOffsetMax.z)
            );

            transform.Rotate(Vector3.right, Random.Range(RotationOffsetMin.x, RotationOffsetMax.x));
            transform.Rotate(Vector3.up, Random.Range(RotationOffsetMin.y, RotationOffsetMax.y));
            transform.Rotate(Vector3.forward, Random.Range(RotationOffsetMin.z, RotationOffsetMax.z));

            if (KeepScaleRatio)
            {
                var multiplier = Random.Range(ScaleMultiplierMin.x, ScaleMultiplierMax.x);
                transform.localScale = new Vector3(
                    transform.localScale.x * multiplier,
                    transform.localScale.y * multiplier,
                    transform.localScale.z * multiplier
                );
            }
            else
            {
                transform.localScale = new Vector3(
                    transform.localScale.x * Random.Range(ScaleMultiplierMin.x, ScaleMultiplierMax.x),
                    transform.localScale.y * Random.Range(ScaleMultiplierMin.y, ScaleMultiplierMax.y),
                    transform.localScale.z * Random.Range(ScaleMultiplierMin.z, ScaleMultiplierMax.z)
                );
            }
            
        }
    }
}