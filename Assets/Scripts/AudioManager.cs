using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer _mainMixer;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";

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
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 0.75f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 0.75f);

        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

    public void SetMusicVolume(float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;

        _mainMixer.SetFloat(MUSIC_KEY, dB);
        PlayerPrefs.SetFloat(MUSIC_KEY, sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;

        _mainMixer.SetFloat(SFX_KEY, dB);
        PlayerPrefs.SetFloat(SFX_KEY, sliderValue);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (!clip) return;

        _sfxSource.PlayOneShot(clip);
    }
}
