using UnityEngine;

namespace AnimalSimulation.Core
{
    public interface IRespawnable
    {
        public void Respawn(Vector3 position, Quaternion rotation, Transform parent);
        public void Despawn();
    }
}