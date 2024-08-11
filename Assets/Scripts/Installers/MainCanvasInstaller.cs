using UnityEngine;
using Zenject;

namespace AnimalSimulation.Installers
{
    public class MainCanvasInstaller : MonoInstaller
    {
        [SerializeField] private Canvas _mainCanvas;

        public override void InstallBindings()
        {
            Container.BindInstance(_mainCanvas).AsSingle();
        }
    }
}