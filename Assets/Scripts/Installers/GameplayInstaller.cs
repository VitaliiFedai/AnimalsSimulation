using UnityEngine;
using Zenject;

namespace AnimalSimulation.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private Gameplay _gameplay;

        public override void InstallBindings()
        {
            Container.BindInstance(_gameplay).WithId(Names.Instance);
        }
    }
}