using UnityEngine;
using Zenject;

namespace AnimalSimulation.Installers
{
    public class WorldSettingsInstaller : MonoInstaller
    {
        [SerializeField] private int _fieldSize = 10;
        [SerializeField] private int _animalsCount = 10;
        [SerializeField] private float _animalSpeed = 1f;

        public override void InstallBindings()
        {
            WorldSettings worldSettings = new WorldSettings()
            {
                FieldSize = _fieldSize,
                AnimalsCount = _animalsCount,
                AnimalSpeed = _animalSpeed
            };
            Container.Bind<IWorldSettings>().To<WorldSettings>().FromInstance(worldSettings);
            Container.Bind<WorldSettings>().FromInstance(worldSettings);
        }
    }
}