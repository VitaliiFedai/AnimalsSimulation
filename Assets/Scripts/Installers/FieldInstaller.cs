using UnityEngine;
using Zenject;

namespace AnimalSimulation.Installers
{
    public class FieldInstaller : MonoInstaller
    {
        [SerializeField] private Field _field;

        public override void InstallBindings()
        {
            Container.BindInstance(_field).AsSingle();
        }
    }
}