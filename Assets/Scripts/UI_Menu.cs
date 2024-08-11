using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace AnimalSimulation.UI
{
    public class UI_Menu : GameWindow
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _saveAndQuitButton;

        [Header("Prefabs")]
        [SerializeField] private UI_MainMenu _mainMenuPrefab;

        [Inject (Id = Names.Instance)] private Gameplay _gameplay;

        private void Start()
        {
            _gameplay.Pause();
            _continueButton.onClick.AddListener(OnContinueButtonClick);
            _saveAndQuitButton.onClick.AddListener(OnSaveAndQuitButtonClick);
        }

        private void OnContinueButtonClick()
        {
            _ = HideAndCall(_gameplay.Resume);
        }

        private async void OnSaveAndQuitButtonClick()
        {
            await HideAndOpen(_mainMenuPrefab.gameObject);
            Destroy(_gameplay.gameObject);
        }
    }
}