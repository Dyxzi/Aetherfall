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

    [Header("SFX Clips")]
    public AudioClip buttonClick;

    void Awake()
    {
        // Singleton - Menjaga agar AudioManager tidak hancur saat pindah scene
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

        // Otomatis mencari AudioSource di child object jika belum di-assign di Inspector
        if (musicSource == null && transform.childCount > 0)
        {
            musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        }

        if (sfxSource == null && transform.childCount > 1)
        {
            sfxSource = transform.GetChild(1).GetComponent<AudioSource>();
        }

        // Load volume yang tersimpan dari PlayerPrefs
        float music = PlayerPrefs.GetFloat("Music", 1f);
        float sfx = PlayerPrefs.GetFloat("SFX", 1f);

        if (musicSource != null) musicSource.volume = music;
        if (sfxSource != null) sfxSource.volume = sfx;

        // Mendaftarkan fungsi OnSceneLoaded ke sistem Unity
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unsubscribe dari event saat object dihancurkan agar tidak memory leak
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeMusic(scene.name);
    }

    void ChangeMusic(string sceneName)
    {
        AudioClip newClip = null;

        // Menentukan lagu berdasarkan nama Scene Unity kamu
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

        if (newClip == null || musicSource == null) return;
        if (musicSource.clip == newClip) return; // Jika lagunya sama, jangan diulang dari awal

        musicSource.clip = newClip;
        musicSource.Play();
    }

    // Memutar SFX apa saja secara dinamis dengan memasukkan Audio Clip-nya langsung.
   
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    //untuk memutar suara Klik Tombol UI.
    public void PlayClickSound()
    {
        PlaySFX(buttonClick);
    }

    //
    // Menghentikan background musik yang sedang berjalan.
    //
    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }


    public void SetMusicVolume(float value)
    {
        if (musicSource != null) musicSource.volume = value;
        PlayerPrefs.SetFloat("Music", value);
    }

    public void SetSFXVolume(float value)
    {
        if (sfxSource != null) sfxSource.volume = value;
        PlayerPrefs.SetFloat("SFX", value);
    }
}