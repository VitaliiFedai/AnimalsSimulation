using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace AnimalSimulation.Installers
{
    public class ButtonInstaller : MonoInstaller
    {
        [Inject] private DiContainer _container;
        [SerializeField] private GameObject _gameplayPrefab;
        [SerializeField] private Button _button;

        public override void InstallBindings()
        {
            _button.onClick.AddListener(() => { _container.InstantiatePrefab(_gameplayPrefab); });
        }
    }
}