using UnityEngine;
using Zenject;
using AnimalSimulation.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalSimulation
{
    public class Gameplay : MonoBehaviour
    {
        private const float DefaultEffectSimulationSpeed = 1.0f;
        private const float MaxTimeToFoodSec = 5.0f;
        private const int FoodLayerMask = 7;

        [SerializeField] private Field _field;
        [SerializeField] private Transform _animalsParent;
        [SerializeField] private Transform _foodParent;
        [SerializeField] private Transform _effectsParent;
        [SerializeField] private UIHandler _uiHandler;

        public float SimulationSpeed 
        {
            get => _simulationSpeed;
            set
            {
                _simulationSpeed = value;
                _effectsSimulationSpeedController.SetSimulationSpeed(value);
            } 
        }
        
        private AnimalsUpdater _animalUpdater;
        private EffectSimulationSpeedController _effectsSimulationSpeedController;
        private FoodSpawnPointsByCeils _foodSpawnPointProvider;
        private IWorldSettings _worldSettings;
        private EntitySpawner _spawner;
        private EntityPrefabs _prefabs;
        private float _simulationSpeed;
        private float _savedSimulationSpeed;
        private bool _isPaused;

        [Inject]
        public void Construct(IWorldSettings worldSettings, EntitySpawner entitySpawner, EntityPrefabs gameplayPrefabs)
        {
            _worldSettings = worldSettings;
            _spawner = entitySpawner;
            _prefabs = gameplayPrefabs;
            _animalUpdater = new AnimalsUpdater();

            _foodSpawnPointProvider = new FoodSpawnPointsByCeils(_spawner, _worldSettings.AnimalSpeed * MaxTimeToFoodSec);

            _effectsSimulationSpeedController = new EffectSimulationSpeedController(DefaultEffectSimulationSpeed);
            SimulationSpeed = DefaultEffectSimulationSpeed;
            _uiHandler.SetSimulationSpeed(SimulationSpeed);
            _uiHandler.OnSliderValueChanged += (value) =>
            {
                SimulationSpeed = value;
            };
        }

        public void Pause()
        {
            if (!_isPaused)
            { 
                _savedSimulationSpeed = SimulationSpeed;
                SimulationSpeed = 0;
                _isPaused = true;
            }
        }

        public void Resume()
        {
            if (_isPaused)
            {
                SimulationSpeed = _savedSimulationSpeed;
                _isPaused = false;
            }
        }

        private void OnEnable()
        {
            _field?.Init(new Vector2Int(_worldSettings.FieldSize, _worldSettings.FieldSize));
            _foodSpawnPointProvider.Bounds = _field.Bounds;
            _foodSpawnPointProvider.Start();
            _animalUpdater.Add(SpawnAll());
        }

        private void OnDisable()
        {
            _spawner.DespawnAll();
            _foodSpawnPointProvider.Stop();
            _animalUpdater.Clear();
            _effectsSimulationSpeedController.Clear();
        }

        private void Update()
        {
            _animalUpdater.Update(SimulationSpeed);
        }

        private IEnumerable<Animal> SpawnAll()
        {
            for (var i = 0; i < _worldSettings.AnimalsCount; i++)
            {
                Vector3 position = GetRandomPosition();
                Animal animal = _spawner.Spawn(_prefabs.Animal, position, Quaternion.identity, _animalsParent) as Animal;
                animal.TargetFood = CreateFood(_foodSpawnPointProvider.GetNewPointNear(position));
                animal.Speed = _worldSettings.AnimalSpeed;
                yield return animal;
            }
        }

        private Vector3 GetRandomPosition()
        {
            return new Vector3(
                Random.Range(_field.Bounds.min.x, _field.Bounds.max.x),
                0,
                Random.Range(_field.Bounds.min.z, _field.Bounds.max.z));
        }

        private Food CreateFood(Vector3 position)
        {
            Food food = _spawner.Spawn(_prefabs.Food, position, Quaternion.identity, _foodParent) as Food;
            food.OnPick += OnPick;
            return food;
        }

        private void OnPick(Food food, Animal picker)
        {
            food.OnPick -= OnPick;
            picker.TargetFood = CreateFood(_foodSpawnPointProvider.GetNewPointNear(picker.transform.position));

            Effect effect = _spawner.Spawn(_prefabs.GatherEffect, food.Position, Quaternion.identity, _foodParent) as Effect;
            _effectsSimulationSpeedController.Add(effect);
        }
    }
}