using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
  private GameManager _gameManager => GameManager.Instance;

  #region Panels
  [SerializeField] private GameObject _mainMenuPanel;
  [SerializeField] private GameObject _settingsPanel;
  #endregion

  #region Buttons
  #region Main Menu Panel Buttons
  [SerializeField] private Button _continueButton;
  [SerializeField] private Button _newGameButton;
  [SerializeField] private Button _settingsButton;
  #endregion

  #region Settings Panel Buttons
  [SerializeField] private Button _settingsBackButton;
  #endregion
  #endregion

  private void Start()
  {
    OpenMainMenuPanel();

    if (_continueButton != null)
    {
      _continueButton.interactable = _gameManager.CanContinueGame;

      _continueButton.onClick.AddListener(OnContinueClicked);
    }

    if (_newGameButton != null)
    {
      _newGameButton.onClick.AddListener(OnNewGameClicked);
    }

    if (_settingsButton != null)
    {
      _settingsButton.onClick.AddListener(OnSettingsClicked);
    }

    if (_settingsBackButton != null)
    {
      _settingsBackButton.onClick.AddListener(OnSettingsBackClicked);
    }
  }

  private void OnDestroy()
  {
    if (_continueButton != null)
    {
      _continueButton.onClick.RemoveListener(OnContinueClicked);
    }

    if (_newGameButton != null)
    {
      _newGameButton.onClick.RemoveListener(OnNewGameClicked);
    }

    if (_settingsButton != null)
    {
      _settingsButton.onClick.RemoveListener(OnSettingsClicked);
    }

    if (_settingsBackButton != null)
    {
      _settingsBackButton.onClick.RemoveListener(OnSettingsBackClicked);
    }
  }

  private void OpenSettingsPanel()
  {
    if (_settingsPanel != null)
    {
      _settingsPanel.SetActive(true);
    }

    if (_mainMenuPanel != null)
    {
      _mainMenuPanel.SetActive(false);
    }
  }

  private void OpenMainMenuPanel()
  {
    if (_mainMenuPanel != null)
    {
      _mainMenuPanel.SetActive(true);
    }

    if (_settingsPanel != null)
    {
      _settingsPanel.SetActive(false);
    }
  }

  private void OnContinueClicked()
  {
    _gameManager.ContinueGame();
  }

  private void OnNewGameClicked()
  {
    _gameManager.NewGame();
  }

  private void OnSettingsClicked()
  {
    OpenSettingsPanel();
  }

  private void OnSettingsBackClicked()
  {
    OpenMainMenuPanel();
  }
}
