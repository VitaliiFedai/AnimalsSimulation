using System.Collections.Generic;

namespace AnimalSimulation
{
    public class EffectSimulationSpeedController
    { 
        private readonly LinkedList<Effect> _effects = new();
        private float _simulationSpeed;

        public EffectSimulationSpeedController(float simulationSpeed)
        {
            _simulationSpeed = simulationSpeed;
        }

        public void SetSimulationSpeed(float simulationSpeed)
        {
            _simulationSpeed = simulationSpeed;

            foreach (Effect effect in _effects)
            { 
                effect.SimulationSpeed = _simulationSpeed;
            }
        }

        public void Add(Effect effect)
        { 
            _effects.AddLast(effect);
            effect.SimulationSpeed = _simulationSpeed;
            effect.OnDespawn += (entity) => { Remove(entity as Effect); };
        }

        private void Remove(Effect effect) 
        { 
            _effects.Remove(effect);
        }

        public void Clear()
        {
            _effects.Clear();
        }
    }
}