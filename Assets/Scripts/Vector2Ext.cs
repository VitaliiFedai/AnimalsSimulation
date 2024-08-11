using UnityEngine;

namespace AnimalSimulation.Tools
{
    public static class Vector2Ext
    {
        public static Vector2 PerpendicularClockwise(this Vector2 vector)
        {
            return new Vector2(vector.y, -vector.x);
        }

        public static Vector2 PerpendicularCounterClockwise(this Vector2 vector)
        {
            return new Vector2(-vector.y, vector.x);
        }
    }
}
