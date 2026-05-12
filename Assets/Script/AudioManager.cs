using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music")]
    public AudioClip mainMenuMusic;
    public AudioClip level1Music;
    public AudioClip level2Music;
    public AudioClip level3Music;

    [Header("SFX")]
    public AudioClip buttonClick;
    public AudioClip shootSFX;
    public AudioClip hitSFX;

    void Awake()
    {
        // 🔥 Singleton
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

        // 🔊 Load volume
        float music = PlayerPrefs.GetFloat("Music", 1f);
        float sfx = PlayerPrefs.GetFloat("SFX", 1f);

        musicSource.volume = music;
        sfxSource.volume = sfx;

        // 🔥 Ganti lagu saat scene berubah
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeMusic(scene.name);
    }

    void ChangeMusic(string sceneName)
    {
        AudioClip newClip = null;

        switch (sceneName)
        {
            case "MainMenu":
                newClip = mainMenuMusic;
                break;

            case "Level1":
                newClip = level1Music;
                break;

            case "Level2":
                newClip = level2Music;
                break;

            case "Level3":
                newClip = level3Music;
                break;
        }

        if (musicSource.clip == newClip) return;

        musicSource.clip = newClip;
        musicSource.Play();
    }

    // 🔊 PLAY SFX
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // 🔊 MUSIC VOLUME
    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat("Music", value);
    }

    // 🔊 SFX VOLUME
    public void SetSFXVolume(float value)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = value;
        }

        PlayerPrefs.SetFloat("SFX", value);
    }
}