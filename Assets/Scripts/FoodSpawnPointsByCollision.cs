using UnityEngine;

namespace AnimalSimulation
{
    public class FoodSpawnPointsByCollision : IFoodSpawnPointsProvider
    {
        private const float GapMultiplier = 1f;

        private float _maxRange;
        private int _layerMask;
        private float _objectRadius;
        private float _objectDiameter;

        public Bounds Bounds {get; set;}

        public FoodSpawnPointsByCollision(float maxRange, Bounds bounds, int layerMask, float objectRadius)
        {
            _maxRange = maxRange;
            Bounds = bounds;
            _layerMask = layerMask;
            _objectRadius = objectRadius;
            _objectDiameter = _objectRadius * 2f;
        }

        public Vector3 GetNewPointNear(Vector3 position)
        {
            Vector3 basePoint = GetInFieldPointNear(position);
            Vector3 offset = basePoint - position;
            Vector3 direction = offset.normalized;
            float distance = offset.magnitude;

            RaycastHit[] hits = Physics.SphereCastAll(position + Vector3.up * 0.5f, _objectRadius, direction, distance, _layerMask);

            float excludeDistance = float.MaxValue;

            for (int i = 0; i < hits.Length - 1; i++)
            {
                float maxDistance = GetMaxDistance(in hits, excludeDistance);
                if (maxDistance > 0)
                {
                    float testOffset = maxDistance + _objectRadius;
                    Vector3 testPoint = position + direction * testOffset;
                    float hitDistance = Physics.SphereCast(testPoint, _objectRadius, -direction, out RaycastHit hit, testOffset, _layerMask) ? hit.distance : testOffset;

                    if (hitDistance > _objectDiameter + _objectRadius)
                    {
                        return testPoint - direction * _objectDiameter;
                    }
                    excludeDistance = maxDistance;
                }
            }
            if (hits.Length > 0)
            {
                Debug.Log($"<color=red>FORCED RESULT</color>{nameof(GetNewPointNear)} = {position + offset}");
            }
            return position + offset;
        }

        private Vector3 GetInFieldPointNear(Vector3 position)
        {
            float angle = Random.Range(0.0f, 360.0f);
            float distance = Random.Range(0, _maxRange);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 offset = direction * distance;
            return new Vector3(
                Mathf.Clamp(position.x + offset.x, Bounds.min.x, Bounds.max.x),
                0,
                Mathf.Clamp(position.z + offset.z, Bounds.min.z, Bounds.max.z)
            );
        }

        private float GetMaxDistance(in RaycastHit[] hits, float lessThan)
        {
            float maxDistance = 0;
            foreach (RaycastHit hit in hits)
            {
                if (hit.distance < lessThan)
                {
                    maxDistance = Mathf.Max(maxDistance, hit.distance);
                }
            }
            return maxDistance;
        }
    }
}