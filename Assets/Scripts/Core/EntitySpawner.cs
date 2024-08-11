using UnityEngine;

namespace AnimalSimulation.Core
{
    public class EntitySpawner : Spawner<Entity>
    {
        private readonly Transform _despawnedEntitiesParent;

        public EntitySpawner(Transform despawnedEntitiesParent)
        {
            _despawnedEntitiesParent = despawnedEntitiesParent;

            OnSpawn += entity => 
            { 
                entity.SetSpawner(this); 
            };       

            OnDespawn += entity => 
            { 
                entity.transform.parent = _despawnedEntitiesParent; 
            };       
        }
    }
}