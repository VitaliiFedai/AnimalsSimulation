using UnityEngine;

namespace AnimalSimulation.Core
{
    public interface IDespawner<T> where T : Object
    {
        public void MoveToDespawned(T entity);
    }
}