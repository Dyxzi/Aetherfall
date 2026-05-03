using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    void Awake()
    {
        // Singleton
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

        // Load setting
        float music = PlayerPrefs.GetFloat("Music", 1f);
        float sfx = PlayerPrefs.GetFloat("SFX", 1f);

        if (musicSource != null)
            musicSource.volume = music;

        if (sfxSource != null)
            sfxSource.volume = sfx;
    }

    public void SetMusicVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;

        PlayerPrefs.SetFloat("Music", value);
    }

    public void SetSFXVolume(float value)
    {
        if (sfxSource != null)
            sfxSource.volume = value;

        PlayerPrefs.SetFloat("SFX", value);
    }
}