using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuController : MonoBehaviour
{
  private const string MUSIC_VOLUME_KEY = "MusicVolume";
  private const string SFX_VOLUME_KEY = "SFXVolume";

  #region Panels
  [SerializeField] private GameObject _pausePanel;
  [SerializeField] private GameObject _settingsPanel;
  #endregion

  #region Buttons

  #region Top-level Buttons
  [SerializeField] private Button _pauseButton;
  #endregion

  #region Pause Panel Buttons
  [SerializeField] private Button _continueButton;
  [SerializeField] private Button _settingsButton;
  [SerializeField] private Button _exitButton;
  #endregion

  #region Settings Panel Buttons
  [SerializeField] private Slider _musicSlider;
  [SerializeField] private Slider _sfxSlider;
  [SerializeField] private Button _backButton;
  #endregion
  #endregion

  private InputAction _cancelAction;

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
    _pausePanel.SetActive(false);
    _settingsPanel.SetActive(false);

    _pauseButton.onClick.AddListener(OnPauseClicked);

    _continueButton.onClick.AddListener(OnContinueClicked);
    _settingsButton.onClick.AddListener(OnSettingsClicked);
    _exitButton.onClick.AddListener(OnExitClicked);

    _musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.75f);
    _sfxSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 0.75f);
    _backButton.onClick.AddListener(OnBackClicked);

    _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    _sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

    SelectUIElement(_pauseButton.gameObject);
  }

  private void OnContinueClicked()
  {
    _pausePanel.SetActive(false);
  }

  private void OnSettingsClicked()
  {
    _pausePanel.SetActive(false);
    _settingsPanel.SetActive(true);

    SelectUIElement(_backButton.gameObject);
  }

  private void OnPauseClicked()
  {
    TogglePausePanel();

    SelectUIElement(_continueButton.gameObject);
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
    _pausePanel.SetActive(true);

    SelectUIElement(_continueButton.gameObject);
  }

  private void TogglePausePanel()
  {
    _pausePanel.SetActive(!_pausePanel.activeSelf);

    if (_pausePanel.activeSelf)
    {
      SelectUIElement(_continueButton.gameObject);
    }
  }

  private void OnCancelPerformed(InputAction.CallbackContext context)
  {
    if (_settingsPanel.activeSelf)
    {
      _settingsPanel.SetActive(false);
      _pausePanel.SetActive(true);

      SelectUIElement(_continueButton.gameObject);
    }
    else
    {
      TogglePausePanel();
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
