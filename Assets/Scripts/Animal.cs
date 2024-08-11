using AnimalSimulation.Core;
using System.Collections.Generic;
using UnityEngine;
using AnimalSimulation.Tools;
using UnityEngine.UIElements;


namespace AnimalSimulation
{

    [RequireComponent(typeof(Rigidbody))]
    public class Animal : Entity, IRefreshable
    {
        private const float StopDistance = 0.1f;
        private const float StraightForwardWeight = 2f;
        private const float ObstacleClampValue = 0.65f;
        private const float BaseWeightDistance = 2f;
        private readonly Vector3 UpOffset = Vector3.up * 0.5f;

        [SerializeField] private Collider _collider;
        public Vector3 TargetPosition { get; private set; }

        public Food TargetFood {
            get => _targetFood;
            set
            {
                _targetFood = value;
                TargetPosition = _targetFood != null ? _targetFood.Position : transform.position;
            }
        } 

        public float Speed { get; set; }
        public Collider Collider => _collider;
        
        private Food _targetFood;
        private Rigidbody _rigidBody;
        private readonly LinkedList<Animal> _closeAnimals = new LinkedList<Animal>();

        public void Refresh(float timeScale)
        {
            Vector3 velocity = GetVelocity(timeScale);
            _rigidBody.velocity = velocity;

            if (TargetFood != null && TargetFood.CanBePickedBy(this))
            {
                TargetFood.Pick(this);
            }
        }

        protected override void OnRespawn()
        {
            TargetFood = null;
        }

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private Vector3 GetVelocity(float timeScale)
        {
            float step = Speed * timeScale * Time.deltaTime;
            float stoppingDistance = 2 * step;

            float distance = Vector3.Distance(TargetPosition, transform.position);
            Vector3 velocity = distance > StopDistance ? GetMoveDirection() * Speed * timeScale : Vector3.zero;

            if (distance < stoppingDistance)
            {
                float distanceWeight = distance / stoppingDistance;
                velocity *= distanceWeight;
            }
            return velocity;
        }

        private Vector3 GetMoveDirection()
        {
            Vector2 position = GetVector2FromVector3(transform.position);
            Vector2 targetPosition = GetVector2FromVector3(TargetPosition);

            Vector2 vectorToTarget = targetPosition - position;

            Vector2 direction = vectorToTarget.normalized;
            Vector2 normal = direction.PerpendicularClockwise();
            Vector2 weightVector = direction * StraightForwardWeight;
            foreach (Animal other in _closeAnimals)
            {
                Vector2 otherPosition = GetVector2FromVector3(other.transform.position);
                Vector2 vectorToOther = otherPosition - position;
                Vector2 directionToOther = vectorToOther.normalized;

                float angle = Vector2.SignedAngle(direction, vectorToOther);
                float dot = Vector2.Dot(directionToOther, direction);
                float distanceWeight = BaseWeightDistance / Mathf.Max(1f, vectorToOther.magnitude);

                if (dot * distanceWeight > ObstacleClampValue)
                {
                    Vector2 sideVector = angle < 0 ? directionToOther.PerpendicularCounterClockwise() : directionToOther.PerpendicularClockwise();
                    Vector2 backVector = -directionToOther;
                    float sideWeight = dot > 0.01f ? 2f * Mathf.Clamp01(0.9f / dot) : 0;
                    weightVector += Vector2.Reflect(vectorToOther, normal) + (sideVector - directionToOther) * sideWeight;
                }
            }
            return GetVector3FromVector2(weightVector.normalized);
        }

        private void OnTriggerEnter(Collider other)
        {
            Animal animal = other.GetComponent<Animal>();
            _closeAnimals.AddLast(animal);
        }

        private void OnTriggerExit(Collider other)
        {
            Animal animal = other.GetComponent<Animal>();
            _closeAnimals.Remove(animal);
        }

        private void OnDrawGizmosSelected()
        {
            Vector2 position = GetVector2FromVector3(transform.position);
            Vector2 targetPosition = GetVector2FromVector3(TargetPosition);

            Gizmos.color = Color.yellow;
            DrawGizmosLine(position, targetPosition);

            Vector2 vectorToTarget = targetPosition - position;

            Vector2 direction = vectorToTarget.normalized;
            Vector2 normal = direction.PerpendicularClockwise();
            Vector2 weightVector = direction * StraightForwardWeight;

            Gizmos.color = Color.blue;
            DrawGizmosLine(position, position + weightVector);

            foreach (Animal other in _closeAnimals)
            {
                Vector2 otherPosition = GetVector2FromVector3(other.transform.position);
                Vector2 vectorToOther = otherPosition - position;
                Vector2 directionToOther = vectorToOther.normalized;

                float angle = Vector2.SignedAngle(direction, vectorToOther);
                float dot = Vector2.Dot(directionToOther, direction);
                float distanceWeight = BaseWeightDistance / Mathf.Max(1f, vectorToOther.magnitude);

                //float distanceWeight = Mathf.Clamp01(BaseWeightDistance / Mathf.Max(1f, vectorToOther.magnitude));

                if (dot * distanceWeight > ObstacleClampValue)
                {
                    Vector2 sideVector = angle < 0 ? directionToOther.PerpendicularCounterClockwise() : directionToOther.PerpendicularClockwise();
                    float sideWeight = dot > 0.01f ? 2f * Mathf.Clamp01(0.9f / dot) : 0;
                    weightVector += Vector2.Reflect(vectorToOther, normal) + sideVector * sideWeight;

                    Gizmos.color = Color.blue;
                    DrawGizmosLine(position, position + sideVector * sideWeight);
                    DrawGizmosLine(position, position + Vector2.Reflect(vectorToOther, normal));

                    Gizmos.color = Color.red;
                    DrawGizmosLine(position, otherPosition);
                }
            }
            Gizmos.color = Color.green;
            DrawGizmosLine(position, position + weightVector);
        }

        private void DrawGizmosLine(Vector2 pointA, Vector2 pointB)
        {
            Gizmos.DrawLine(GetVector3FromVector2(pointA) + UpOffset, GetVector3FromVector2(pointB) + UpOffset);
        }

        private Vector3 GetVector3FromVector2(Vector2 vector) 
        {
            return new Vector3(vector.x, 0, vector.y);
        }

        private Vector3 GetVector2FromVector3(Vector3 vector) 
        {
            return new Vector2(vector.x, vector.z);
        }
    }
}