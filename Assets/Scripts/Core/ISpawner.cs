using UnityEngine;

namespace AnimalSimulation.Core
{
    public interface ISpawner<T> where T : Object
    {
        public T Spawn(T prefab, Vector3 position, Quaternion rotation, Transform parent);
    }
}