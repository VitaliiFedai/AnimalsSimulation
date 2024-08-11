using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace AnimalSimulation.UI
{
    public class SliderWithText : MonoBehaviour
    {
        public event Action<float> OnValueChanged;

        [SerializeField] protected Slider _slider;
        [SerializeField] protected TMP_Text _sliderValue;

        public float Value
        {
            get => _slider.value;
            set
            {
                _slider.value = value;
                _sliderValue.text = value.ToString("F1");
            }
        }

        public float MaxValue
        {
            get => _slider.maxValue;
            set => _slider.maxValue = value;
        }

        private void Start()
        {
            _slider.onValueChanged.AddListener((value) =>
            {
                _sliderValue.text = value.ToString("F1");
                OnValueChanged?.Invoke(value);
            });
        }
    }
}