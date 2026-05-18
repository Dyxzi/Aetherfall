using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Enemy Progress")]
    public TextMeshProUGUI enemyProgressText;
    public Image enemyProgressBar;
    public GameObject allEnemyDefeatedText;

    [Header("Player Health")]
    public Image playerHealthBar;
    public float playerHealthBarFullX = 78;

    [Header("UI Panels")]
    public GameObject gameOverUI;
    public GameObject levelCompleteUI;
    public GameObject pauseUI;

    [Header("Main Menu")]
    public TextMeshProUGUI totalCrystal;

    [Header("Player")]
    public PlayerMovment player;

    [Header("Finish")]
    public Transform finishPoint;
    public float finishDistance = 1.5f;

    [Header("Settings")]
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Load total kristal di Main Menu
        if (totalCrystal != null)
        {
            int total = PlayerPrefs.GetInt("TotalCrystal", 0);
            totalCrystal.text = ": " + total;
        }

        Time.timeScale = 1f;

        // Set sumbu bar darah di sebelah kiri
        if (playerHealthBar)
        {
            playerHealthBar.rectTransform.pivot = new Vector2(0, 0.5f);
        }

        // Load volume musik
        if (musicSlider != null)
        {
            float music = PlayerPrefs.GetFloat("Music", 1f);
            musicSlider.value = music;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(music);
            }
        }

        // Load volume SFX
        if (sfxSlider != null)
        {
            float sfx = PlayerPrefs.GetFloat("SFX", 1f);
            sfxSlider.value = sfx;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetSFXVolume(sfx);
            }
        }

        if (allEnemyDefeatedText != null)
        {
            allEnemyDefeatedText.SetActive(false);
        }
    }

    void Update()
    {
        // Update teks jumlah musuh (contoh: 1/5)
        if (enemyProgressText)
        {
            enemyProgressText.text = " " + ScoreManager.currentEnemyProgress + " / " + ScoreManager.targetEnemyProgress;
        }

        // Animasi bar progres musuh
        if (enemyProgressBar)
        {
            float target = (float)ScoreManager.currentEnemyProgress / ScoreManager.targetEnemyProgress;
            enemyProgressBar.fillAmount = Mathf.Lerp(enemyProgressBar.fillAmount, target, Time.deltaTime * 5f);
        }

        // Notifikasi semua musuh kalah
        if (allEnemyDefeatedText != null)
        {
            allEnemyDefeatedText.SetActive(ScoreManager.IsAllEnemyDefeated());
        }

        // Animasi pengurangan bar darah player
        if (playerHealthBar)
        {
            Vector2 size = playerHealthBar.rectTransform.sizeDelta;
            float targetX = player.health / player.healthMax * playerHealthBarFullX;
            size.x = Mathf.Lerp(size.x, targetX, Time.deltaTime * 10f);
            playerHealthBar.rectTransform.sizeDelta = size;
        }

        CheckFinish();

        // Sinkronisasi status panel UI
        if (levelCompleteUI) levelCompleteUI.SetActive(GameManager.isLevelComplete);
        if (gameOverUI) gameOverUI.SetActive(GameManager.isGameOver);
        if (pauseUI) pauseUI.SetActive(GameManager.isPaused);

        CheckGameState();
    }

    // Menghentikan waktu game otomatis jika ada panel UI yang terbuka
    void CheckGameState()
    {
        bool anyUIOpen = false;

        if (pauseUI != null && pauseUI.activeSelf) anyUIOpen = true;
        if (gameOverUI != null && gameOverUI.activeSelf) anyUIOpen = true;
        if (levelCompleteUI != null && levelCompleteUI.activeSelf) anyUIOpen = true;

        Time.timeScale = anyUIOpen ? 0f : 1f;
    }

    // Cek apakah player sudah sampai di titik finish dan semua musuh kalah
    void CheckFinish()
    {
        if (player == null || finishPoint == null) return;

        float distance = Vector2.Distance(player.transform.position, finishPoint.position);

        if (distance <= finishDistance)
        {
            if (ScoreManager.IsAllEnemyDefeated())
            {
                GameManager.CompleteLevel();
            }
            else
            {
                Debug.Log("Kalahkan semua musuh dulu!");
            }
        }
    }

    // Pengaturan volume musik via Slider
    public void SetMusic(float value)
    {
        PlayerPrefs.SetFloat("Music", value);
        if (AudioManager.Instance != null) AudioManager.Instance.SetMusicVolume(value);
    }

    // Pengaturan volume SFX via Slider
    public void SetSFX(float value)
    {
        PlayerPrefs.SetFloat("SFX", value);
        if (AudioManager.Instance != null) AudioManager.Instance.SetSFXVolume(value);
    }

    // Reset total tabungan kristal
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("TotalCrystal");
        ScoreManager.crystal = 0;
        PlayerPrefs.Save();

        if (totalCrystal != null) totalCrystal.text = ": ";
        Debug.Log("Progress Reset");
    }
}