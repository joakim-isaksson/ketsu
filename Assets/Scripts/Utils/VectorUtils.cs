using UnityEngine;

namespace Ketsu.Utils
{
    public class VectorUtils
    {
        /// <summary>
        /// Mirrors given vector
        /// </summary>
        /// <param name="target">Point to be mirrored</param>
        /// <param name="point">Mirroring point</param>
        /// <returns>Mirrored vector</returns>
        public static Vector3 Mirror(Vector3 target, Vector3 point)
        {
            return new Vector3(
                point.x + (point.x - target.x),
                point.y + (point.y - target.y),
                point.z + (point.z - target.z)
            );
        }
    }
}