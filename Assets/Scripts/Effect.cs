using UnityEngine;
using AnimalSimulation.Core;
namespace AnimalSimulation
{
    [RequireComponent(typeof(ParticleSystem))]
    public class Effect : Entity
    {
        public float SimulationSpeed
        {
            get => _particleSystem.main.simulationSpeed;
            set 
            {
                ParticleSystem.MainModule main = _particleSystem.main;
                main.simulationSpeed = value;

                foreach (ParticleSystem child in GetComponentsInChildren<ParticleSystem>())
                {
                    main = child.main;
                    main.simulationSpeed = value;
                }
            }
        }

        private ParticleSystem _particleSystem;

        protected override void OnRespawn()
        {
            _particleSystem.Play(true);
        }

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void OnParticleSystemStopped()
        {
            Despawn();
        }
    }
}