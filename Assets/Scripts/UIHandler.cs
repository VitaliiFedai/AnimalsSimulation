using UnityEngine;
using System;
using AnimalSimulation.UI;
using UnityEngine.UI;
using Zenject;
using TMPro.EditorUtilities;

namespace AnimalSimulation
{
    public class UIHandler : MonoBehaviour
    {
        public event Action<float> OnSliderValueChanged;
        
        [SerializeField] private SliderWithBase _slider;
        [SerializeField] private Button _menuButton;

        [SerializeField] private UI_Menu _menuPrefab;
        [Inject] private DiContainer _container;
        [Inject] private Canvas _mainCanvas;

        public void SetSimulationSpeed(float value)
        {
            _slider.Value = value;
        }

        private void Start()
        {
            SetSimulationSpeed(1.0f);
            _slider.OnValueChanged += (value) =>
            {
                OnSliderValueChanged?.Invoke(value);
            };

            _menuButton.onClick.AddListener(() => 
            {
                _container.InstantiatePrefab(_menuPrefab, _mainCanvas.transform);
            });
        }
    }
}