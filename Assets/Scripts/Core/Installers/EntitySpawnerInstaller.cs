using UnityEngine;
using Zenject;
using AnimalSimulation.Core;

namespace AnimalSimulation.Installers
{
    public class EntitySpawnerInstaller : MonoInstaller
    {
        [SerializeField] private Transform _despawnedEntitiesParent;

        public override void InstallBindings()
        {
            Container.Bind<EntitySpawner>().AsSingle().WithArguments(_despawnedEntitiesParent).NonLazy();
        }
    }
}