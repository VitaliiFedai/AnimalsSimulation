using System.Collections.Generic;

namespace AnimalSimulation
{
    public class AnimalsUpdater
    { 
        private readonly List<Animal> _animals = new();

        public void Update(float timeScale)
        {
            foreach (Animal animal in _animals)
            { 
                animal.Refresh(timeScale);
            }
        }

        public void Add(IEnumerable<Animal> animals)
        { 
            _animals.AddRange(animals);
        }

        public void Clear()
        {
            _animals.Clear();
        }
    }
}