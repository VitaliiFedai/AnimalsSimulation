using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace AnimalSimulation.UI
{
    public class UI_Settings : GameWindow
    {
        [SerializeField] private SliderWithText _fieldSizeSlider;
        [SerializeField] private SliderWithText _animalsCountSlider;
        [SerializeField] private SliderWithText _animalsSpeedSlider;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _backButton;

        [Header("Links")]
        [SerializeField] private UI_MainMenu _mainMenuPrefab;

        [Inject (Id = Names.Prefab)] private Gameplay _gameplayPrefab;
        [Inject] private WorldSettings _worldSettings;

        private void Start()
        {
            _startButton.onClick.AddListener(OnStartButtonClick);
            _backButton.onClick.AddListener(OnBackButtonClick);

            _fieldSizeSlider.Value = _worldSettings.FieldSize;
            _animalsCountSlider.MaxValue = GetMaxAnimalsCount(_worldSettings.FieldSize);
            _animalsCountSlider.Value = _worldSettings.AnimalsCount;
            _animalsSpeedSlider.Value = _worldSettings.AnimalSpeed;

            _fieldSizeSlider.OnValueChanged += (value) => 
            { 
                _worldSettings.FieldSize = (int)value;
                _animalsCountSlider.MaxValue = GetMaxAnimalsCount(_worldSettings.FieldSize);
            };
            
            _animalsCountSlider.OnValueChanged += (value) => 
            { 
                _worldSettings.AnimalsCount = (int)value; 
            };
            
            _animalsSpeedSlider.OnValueChanged += (value) => 
            { 
                _worldSettings.AnimalSpeed = value; 
            };
        }

        private void OnStartButtonClick()
        {
            _ = HideAndOpen(_gameplayPrefab.gameObject, null);
        }

        private void OnBackButtonClick()
        {
            _ = HideAndOpen(_mainMenuPrefab.gameObject);
        }

        private int GetMaxAnimalsCount(int fieldSize)
        {
            return fieldSize * fieldSize / 2;
        }
    }
}