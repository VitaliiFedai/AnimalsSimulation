using UnityEngine;
using Zenject;

namespace AnimalSimulation.Installers
{
    public class GameplayPrefabInstaller : MonoInstaller
    {
        [SerializeField] private Gameplay _gameplayPrefab;

        public override void InstallBindings()
        {
            Container.BindInstance(_gameplayPrefab).WithId(Names.Prefab);
        }
    }
}