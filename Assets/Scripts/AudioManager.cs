using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer _mainMixer;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    private readonly SettingsPrefManager _settingsPrefManager = SettingsPrefManager.GetInstance();

    private const string MIXER_MUSIC_PARAM_KEY = "MusicVolume";
    private const string MIXER_SFX_PARAM_KEY = "SFXVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _settingsPrefManager.Load();
    }

    private void Start()
    {
        SetMusicVolume(_settingsPrefManager.MusicVolume);
        SetSFXVolume(_settingsPrefManager.SFXVolume);
    }

    public void SetMusicVolume(float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;

        _mainMixer.SetFloat(MIXER_MUSIC_PARAM_KEY, dB);
        _settingsPrefManager.SetMusicVolume(sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;

        _mainMixer.SetFloat(MIXER_SFX_PARAM_KEY, dB);
        _settingsPrefManager.SetSFXVolume(sliderValue);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (!clip) return;

        _sfxSource.PlayOneShot(clip);
    }
}
