using UnityEngine;
using Zenject;

namespace AnimalSimulation.Installers
{
    public class EntityPrefabsInstaller : MonoInstaller
    {
        [SerializeField] private Animal _animalPrefab;
        [SerializeField] private Food _foodPrefab;
        [SerializeField] private Effect _gatherEffectPrefab;

        public override void InstallBindings()
        {
            Container.Bind<EntityPrefabs>().AsSingle().WithArguments(_animalPrefab, _foodPrefab, _gatherEffectPrefab).NonLazy();
        }
    }
}