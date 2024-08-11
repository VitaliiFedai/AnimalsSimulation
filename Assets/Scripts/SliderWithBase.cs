using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace AnimalSimulation.UI
{
    public class SliderWithBase : MonoBehaviour
    {
        public event Action<float> OnValueChanged;

        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _sliderValue;
        [SerializeField, Range(0, 1.0f)] private float _relativeBaseValue = 0.1f;
        [SerializeField] private float _baseValue = 1.0f;

        public float Value
        {
            get => GetExteriorValue(_slider.value);
            set
            {
                _slider.value = GetInnerValue(value);
                _sliderValue.text = value.ToString("F1");
            }
        }

        public float MaxValue => _slider.maxValue;

        private void Start()
        {
            _slider.onValueChanged.AddListener((value) =>
            {
                float exteriorValue = GetExteriorValue(value);
                _sliderValue.text = exteriorValue.ToString("F1");
                OnValueChanged?.Invoke(exteriorValue);
            });
            Value = _baseValue;
        }

        private float GetExteriorValue(float innerValue)
        {
            float relativeValue = innerValue / _slider.maxValue;
            return relativeValue <= _relativeBaseValue ?
                Mathf.Lerp(0, _baseValue, Mathf.InverseLerp(0, _relativeBaseValue, relativeValue)) :
                Mathf.Lerp(_baseValue, _slider.maxValue, Mathf.InverseLerp(_relativeBaseValue, 1.0f, relativeValue));
        }

        private float GetInnerValue(float exteriorValue)
        {
            return exteriorValue <= _baseValue ?
                Mathf.Lerp(0, _relativeBaseValue * _slider.maxValue, Mathf.InverseLerp(0, _baseValue, exteriorValue)) :
                Mathf.Lerp(_relativeBaseValue * _slider.maxValue, _slider.maxValue, Mathf.InverseLerp(_baseValue, _slider.maxValue, exteriorValue));
        }
    }
}