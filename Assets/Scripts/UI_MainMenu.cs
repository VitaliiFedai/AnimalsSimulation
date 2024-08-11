using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace AnimalSimulation.UI
{
    public class UI_MainMenu : GameWindow
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _exitButton;

        [Header("Prefabs")]
        [SerializeField] private UI_Settings _settingsPrefab;
        [Inject (Id = "Prefab")] private Gameplay _gameplayPrefab;

        private void Start()
        {
            _continueButton.onClick.AddListener(OnContinueButtonClick);
            _continueButton.interactable = false;
            _newGameButton.onClick.AddListener(OnNewGameButtonClick);
            _exitButton.onClick.AddListener(OnExitButtonClick);
        }

        private void OnContinueButtonClick()
        {
            _ = HideAndOpen(_settingsPrefab.gameObject);
        }

        private void OnNewGameButtonClick()
        {
            _ = HideAndOpen(_settingsPrefab.gameObject);
        }

        private void OnExitButtonClick()
        {
            Application.Quit();
        }
    }
}