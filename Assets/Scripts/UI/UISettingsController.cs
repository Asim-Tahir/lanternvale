using UnityEngine;
using UnityEngine.UI;

public class UISettingsController : MonoBehaviour
{
  [SerializeField] private Slider _musicSlider;
  [SerializeField] private Slider _sfxSlider;

  private readonly SettingsPrefManager _prefManager = SettingsPrefManager.GetInstance();

  private void Awake()
  {
    _prefManager.Load();

    _musicSlider.value = _prefManager.MusicVolume;
    _sfxSlider.value = _prefManager.SFXVolume;
  }

  private void Start()
  {
    _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    _sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
  }

  private void OnMusicVolumeChanged(float value)
  {
    if (AudioManager.Instance != null)
    {
      AudioManager.Instance.SetMusicVolume(value);
      _prefManager.SetMusicVolume(value);
    }
  }

  private void OnSFXVolumeChanged(float value)
  {
    if (AudioManager.Instance != null)
    {
      AudioManager.Instance.SetSFXVolume(value);
      _prefManager.SetSFXVolume(value);
    }
  }
}
