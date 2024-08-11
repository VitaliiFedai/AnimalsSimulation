using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalSimulation.Core
{
    public class Spawner<T> : ISpawner<T>, IDespawner<T> where T : MonoBehaviour, IRespawnable
    {
        private int nextID = 0;

        public event Action<T> OnSpawn;
        public event Action<T> OnDespawn;
        public event Action<T> OnInstantiate;

        private readonly LinkedList<T> _entities = new LinkedList<T>();

        private readonly Dictionary<T, Queue<T>> _despawnedEntities = new Dictionary<T, Queue<T>>();

        private readonly Dictionary<T, T> _spawnedEntities = new Dictionary<T, T>();

        public T Spawn(T prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            T entity = GetOrCreate(prefab, position, rotation, parent);
            _entities.AddLast(entity);
            OnSpawn?.Invoke(entity);
            return entity;
        }

        public void MoveToDespawned(T entity)
        {
            _entities.Remove(entity);
            T prefab = _spawnedEntities[entity];

            if (!_despawnedEntities.ContainsKey(prefab))
            {
                _despawnedEntities[prefab] = new Queue<T>();
            }

            _despawnedEntities[prefab].Enqueue(entity);

            OnDespawn?.Invoke(entity);
            entity.gameObject.SetActive(false);
        }

        private T GetOrCreate(T prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            T entity = null;

            if (_despawnedEntities.ContainsKey(prefab))
            {
                Queue<T> despawnedEntities = _despawnedEntities[prefab];

                if (despawnedEntities.Count > 0)
                {
                    entity = despawnedEntities.Dequeue();
                    entity.Respawn(position, rotation, parent);
                    entity.gameObject.SetActive(true);
                }
            }

            if (entity is null)
            {
                entity = UnityEngine.Object.Instantiate(prefab, position, rotation, parent);
                entity.gameObject.name = entity.gameObject.name + nextID++.ToString();
                _spawnedEntities[entity] = prefab;
                OnInstantiate?.Invoke(entity);
            }

            return entity;
        }

        public void DespawnAll()
        {
            while (_entities.Count > 0)
            {
                _entities.Last.Value.Despawn();
            }
        }
    }
}