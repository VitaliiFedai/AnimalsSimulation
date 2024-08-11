using Zenject;

namespace AnimalSimulation.Installers
{
    public class FoodSpawnPointProviderInstaller : MonoInstaller 
    {
        private const float MaxTimeToFoodSec = 5.0f;
        [Inject] private IWorldSettings _worldSettings;

        public override void InstallBindings()
        {
            Container.Bind<IFoodSpawnPointsProvider>().To<FoodSpawnPointsByCeils>().WithArguments(_worldSettings.AnimalSpeed * MaxTimeToFoodSec).NonLazy();
        }
    }
}