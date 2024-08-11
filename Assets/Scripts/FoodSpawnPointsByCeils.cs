using AnimalSimulation.Core;
using UnityEngine;

namespace AnimalSimulation
{
    public class FoodSpawnPointsByCeils : IFoodSpawnPointsProvider
    {
        private readonly Vector3 CeilCenterOffset = new(0.5f, 0, 0.5f);

        public Bounds Bounds
        {
            get => _bounds;
            set
            {
                _bounds = value;
                _fieldSize = new Vector2Int((int)_bounds.size.x, (int)_bounds.size.z);
                SetField(_fieldSize);
            }
        }

        private bool[] _field;
        private Bounds _bounds;
        private EntitySpawner _spawner;
        private Vector2Int _fieldSize;
        private float _maxRange;
        private float _maxRangeSqr;

        public FoodSpawnPointsByCeils(EntitySpawner spawner, float maxRange)
        {
            _spawner = spawner;
            _maxRange = maxRange;
            _maxRangeSqr = _maxRange * _maxRange;
        }

        ~FoodSpawnPointsByCeils()
        {
            _spawner.OnSpawn -= OnSpawn;
            _spawner.OnDespawn -= OnDespawn;
        }

        public void Start()
        {
            _spawner.OnSpawn += OnSpawn;
            _spawner.OnDespawn += OnDespawn;
        }

        public void Stop()
        {
            _spawner.OnSpawn -= OnSpawn;
            _spawner.OnDespawn -= OnDespawn;
        }

        public Vector3 GetNewPointNear(Vector3 position)
        {
            Vector2Int animalCeil = GetCeilPosition(position);

            Vector3 inFieldPoint = GetInFieldPointNear(position);

            Vector2Int ceilPosition = GetCeilPosition(inFieldPoint);

            int index = GetIndex(ceilPosition);

            if (!_field[index])
            {
                return GetPosition(ceilPosition);
            }

            int maxDistanceToBounds = 
                Mathf.Max(
                    Mathf.Max(ceilPosition.x, _fieldSize.x - ceilPosition.x),
                    Mathf.Max(ceilPosition.y, _fieldSize.y - ceilPosition.y)
                );

            for (int distance = 1; distance <= maxDistanceToBounds; distance++)
            {
                if (FindEmptyCeilNearCeil(ceilPosition, distance, animalCeil, out Vector2Int additionalCeilPosition))
                {
                    return GetPosition(additionalCeilPosition);
                }
            }

            throw new System.Exception($"Could not find empty ceil near {position}!");
        }

        private bool FindEmptyCeilNearCeil(Vector2Int centerCeil, int distance, Vector2Int animalCeil, out Vector2Int ceilPosition)
        {
            // Bottom Row
            if (FindEmptyCeilAtZone(centerCeil.x - distance, centerCeil.y - distance, centerCeil.x + distance, centerCeil.y - distance, animalCeil, out ceilPosition))
            {
                return true;
            }

            // Top Row
            if (FindEmptyCeilAtZone(centerCeil.x - distance, centerCeil.y + distance, centerCeil.x + distance, centerCeil.y + distance, animalCeil, out ceilPosition))
            {
                return true;
            }

            // Left Column
            if (FindEmptyCeilAtZone(centerCeil.x - distance, centerCeil.y - distance + 1, centerCeil.x - distance, centerCeil.y + distance - 1, animalCeil, out ceilPosition))
            {
                return true;
            }

            // Right Column
            if (FindEmptyCeilAtZone(centerCeil.x + distance, centerCeil.y - distance + 1, centerCeil.x + distance, centerCeil.y + distance - 1, animalCeil, out ceilPosition))
            {
                return true;
            }

            ceilPosition = new Vector2Int(-1, -1);
            return false;
        }

        private bool FindEmptyCeilAtZone(int minX, int minY, int maxX, int maxY, Vector2Int animalCeil, out Vector2Int ceilPosition)
        {
            for (int i = minY; i <= maxY; i++)
            {
                for (int j = minX; j <= maxX; j++)
                {
                    if (IsCeilInBounds(j, i) && !_field[GetIndex(j, i)] && ValidateCeilByDistance(j, i, animalCeil))
                    {
                        ceilPosition = new Vector2Int(j, i);
                        return true;
                    }
                }
            }
            ceilPosition = new Vector2Int(-1, -1);
            return false;
        }

        private bool ValidateCeilByDistance(int X, int Y, Vector2Int animalCeil)
        {
            float distanceSqr = (new Vector2Int(X, Y) - animalCeil).sqrMagnitude;
            return distanceSqr <= _maxRangeSqr;
        }

        private bool IsCeilInBounds(int X, int Y)
        {
            return (0 <= X) && (X < _fieldSize.x) && (0 <= Y) && (Y < _fieldSize.y);
        }

        private void SetField(Vector2Int size)
        {
            _field = new bool[size.x * size.y];
        }

        private Vector3 GetPosition(Vector2Int ceilPosition)
        {
            return new Vector3(ceilPosition.x + CeilCenterOffset.x, 0, ceilPosition.y + CeilCenterOffset.z) + _bounds.min;
        }

        private void OnSpawn(Entity entity)
        {
            if (entity is Food food)
            {
                Vector2Int ceilPosition = GetCeilPosition(food.Position);

                int index = GetIndex(ceilPosition);
                if (_field[index])
                {
                    Debug.Log($"<color=red>{nameof(OnSpawn)}: Ceil is already filled!</color> {ceilPosition} - {food}");
                }
                else
                {
                    _field[index] = true;
                }
            }
        }

        private void OnDespawn(Entity entity)
        {
            if (entity is Food food)
            {
                Vector2Int ceilPosition = GetCeilPosition(food.Position);
                int index = GetIndex(ceilPosition);
                if (!_field[index])
                {
                    Debug.Log($"<color=red>{nameof(OnDespawn)}: Ceil is not filled!</color> {ceilPosition} - {food}");
                }
                else
                {
                    _field[index] = false;
                }
            }
        }

        private Vector3 GetInFieldPointNear(Vector3 position)
        {
            //            return new Vector3(10.3f, 0, 0);

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

        private Vector2Int GetCeilPosition(Vector3 position)
        {
            Vector3 positionInField = position - _bounds.min;
            return new Vector2Int(
                Mathf.Clamp((int)positionInField.x, 0, _fieldSize.x - 1), 
                Mathf.Clamp((int)positionInField.z, 0, _fieldSize.y - 1)
            );
        }

        private int GetIndex(Vector3 position)
        {
            Vector2Int ceilPosition = GetCeilPosition(position);
            return GetIndex(ceilPosition);
        }

        private int GetIndex(Vector2Int ceilPosition)
        {
            return GetIndex(ceilPosition.x, ceilPosition.y);
        }

        private int GetIndex(int ceilX, int ceilY)
        {
            return ceilX + ceilY * _fieldSize.x;
        }
    }
}