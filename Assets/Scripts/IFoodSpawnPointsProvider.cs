using UnityEngine;

namespace AnimalSimulation
{
    public interface IFoodSpawnPointsProvider 
    {
        public Bounds Bounds { get; set; }
        public Vector3 GetNewPointNear(Vector3 position);
    }
}