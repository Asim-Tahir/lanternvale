using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer _mainMixer;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    private const string SAVED_MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SAVED_SFX_VOLUME_KEY = "SFXVolume";
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
    }

    private void Start()
    {
        float musicVolume = PlayerPrefs.GetFloat(SAVED_MUSIC_VOLUME_KEY, 0.75f);
        float sfxVolume = PlayerPrefs.GetFloat(SAVED_SFX_VOLUME_KEY, 0.75f);

        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

    public void SetMusicVolume(float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;

        _mainMixer.SetFloat(MIXER_MUSIC_PARAM_KEY, dB);
        PlayerPrefs.SetFloat(SAVED_MUSIC_VOLUME_KEY, sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;

        _mainMixer.SetFloat(MIXER_SFX_PARAM_KEY, dB);
        PlayerPrefs.SetFloat(SAVED_SFX_VOLUME_KEY, sliderValue);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (!clip) return;

        _sfxSource.PlayOneShot(clip);
    }
}
