using System;
using UnityEngine;

namespace AnimalSimulation.Core
{
    public class Entity : MonoBehaviour, IRespawnable
    {
        public event Action<Entity> OnDespawn;
        private EntitySpawner _spawner;

        public void Respawn(Vector3 position, Quaternion rotation, Transform parent)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.parent = parent;

            OnRespawn();
        }

        public void SetSpawner(EntitySpawner spawner)
        {
            _spawner = spawner;
        }

        public void Despawn()
        {
            OnDespawnInner();
            OnDespawn = null;
            _spawner?.MoveToDespawned(this);
        }

        protected virtual void OnRespawn()
        { 
        }
        protected virtual void OnDespawnInner()
        {
            OnDespawn?.Invoke(this);
        }
    }
}