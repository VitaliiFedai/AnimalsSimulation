using UnityEngine;

namespace AnimalSimulation.Tools
{
    public static class Vector3Ext
    {
        public static Vector3 MirrorBy(this Vector3 vector, Vector3 normal)
        {
            return MirrorByNormalized(vector, normal.normalized);
        }

        public static Vector3 MirrorByNormalized(this Vector3 vector, Vector3 normal)
        {
            return vector - 2 * Vector3.Dot(vector, normal) * normal;
        }
    }
}
