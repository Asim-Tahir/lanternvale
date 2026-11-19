using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
  private const string MUSIC_VOLUME_KEY = "MusicVolume";
  private const string SFX_VOLUME_KEY = "SFXVolume";

  #region Panels
  [SerializeField] private GameObject _mainMenuPanel;
  [SerializeField] private GameObject _settingsPanel;
  #endregion

  #region Buttons
  #region Main Menu Panel Buttons
  [SerializeField] private Button _continueButton;
  [SerializeField] private Button _newGameButton;
  [SerializeField] private Button _settingsButton;
  [SerializeField] private Button _exitButton;
  #endregion

  #region Settings Panel Buttons
  [SerializeField] private Slider _musicSlider;
  [SerializeField] private Slider _sfxSlider;
  [SerializeField] private Button _backButton;
  #endregion
  #endregion

  [SerializeField] private string _firstLevelSceneName = "GameScene";

  private InputAction _cancelAction;

  private const string SAVED_PLACEMENT_KEY = "SavedPlacementData";
  private const string SAVED_LEVEL_KEY = "SavedLevel";

  private void Awake()
  {
    _cancelAction = new InputAction("Cancel", binding: "<Keyboard>/escape");
    _cancelAction.AddBinding("<Gamepad>/buttonEast");
    _cancelAction.performed += OnCancelPerformed;
    _cancelAction.Enable();
  }

  private void OnDestroy()
  {
    _cancelAction.performed -= OnCancelPerformed;
    _cancelAction.Disable();
    _cancelAction.Dispose();
  }

  private void Start()
  {
    _mainMenuPanel.SetActive(true);
    _settingsPanel.SetActive(false);

    bool hasSavedLevel = PlayerPrefs.HasKey(SAVED_LEVEL_KEY);

    _continueButton.interactable = hasSavedLevel;

    _continueButton.onClick.AddListener(OnContinueClicked);
    _newGameButton.onClick.AddListener(OnNewGameClicked);
    _settingsButton.onClick.AddListener(OnSettingsClicked);
    _exitButton.onClick.AddListener(OnExitClicked);

    _musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.75f);
    _sfxSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.75f);
    _backButton.onClick.AddListener(OnBackClicked);

    _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    _sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

    SelectUIElement(_newGameButton.gameObject);
  }

  private void OnContinueClicked()
  {
    if (PlayerPrefs.HasKey(SAVED_LEVEL_KEY))
    {
      string savedLevel = PlayerPrefs.GetString(SAVED_LEVEL_KEY);
      SceneManager.LoadScene(savedLevel);
    }
  }

  private void OnNewGameClicked()
  {
    PlayerPrefs.DeleteKey(SAVED_PLACEMENT_KEY);
    PlayerPrefs.SetString(SAVED_LEVEL_KEY, _firstLevelSceneName);
    PlayerPrefs.Save();
    SceneManager.LoadScene(_firstLevelSceneName);
  }

  private void OnSettingsClicked()
  {
    _mainMenuPanel.SetActive(false);
    _settingsPanel.SetActive(true);

    SelectUIElement(_musicSlider.gameObject);
  }

  private void OnExitClicked()
  {
    Application.Quit();
  }

  private void OnMusicVolumeChanged(float value)
  {
    if (AudioManager.Instance != null)
    {
      AudioManager.Instance.SetMusicVolume(value);

      PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, value);
      PlayerPrefs.Save();
    }

  }

  private void OnSFXVolumeChanged(float value)
  {
    if (AudioManager.Instance != null)
    {
      AudioManager.Instance.SetSFXVolume(value);

      PlayerPrefs.SetFloat(SFX_VOLUME_KEY, value);
      PlayerPrefs.Save();
    }
  }

  private void OnBackClicked()
  {
    _settingsPanel.SetActive(false);
    _mainMenuPanel.SetActive(true);

    SelectUIElement(_newGameButton.gameObject);
  }

  private void OnCancelPerformed(InputAction.CallbackContext context)
  {
    if (_settingsPanel.activeSelf)
    {
      OnBackClicked();
    }
  }

  private void SelectUIElement(GameObject uiElement)
  {
    if (EventSystem.current != null && uiElement != null)
    {
      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(uiElement);
    }
  }
}
