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

    [Header("Player")]
    public PlayerMovment player;

    // 🔥 FINISH SYSTEM
    [Header("Finish")]
    public Transform finishPoint;
    public float finishDistance = 1.5f;

    // 🔊 SETTINGS AUDIO
    [Header("Settings")]
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        Time.timeScale = 1f;
        // 🔥 Health bar pivot
        if (playerHealthBar)
        {
            playerHealthBar.rectTransform.pivot =
            new Vector2(0, 0.5f);
        }

        // 🔊 LOAD MUSIC
        if (musicSlider != null)
        {
            float music =
            PlayerPrefs.GetFloat("Music", 1f);

            musicSlider.value = music;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(music);
            }
        }

        // 🔊 LOAD SFX
        if (sfxSlider != null)
        {
            float sfx =
            PlayerPrefs.GetFloat("SFX", 1f);

            sfxSlider.value = sfx;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetSFXVolume(sfx);
            }
        }

        // 🔥 Hide text awal
        if (allEnemyDefeatedText != null)
        {
            allEnemyDefeatedText.SetActive(false);
        }
    }

    void Update()
    {
        // 🔥 ENEMY TEXT
        if (enemyProgressText)
        {
            enemyProgressText.text =
            " " +
            ScoreManager.currentEnemyProgress +
            " / " +
            ScoreManager.targetEnemyProgress;
        }

        // 🔥 ENEMY PROGRESS BAR
        if (enemyProgressBar)
        {
            float target =
            (float)ScoreManager.currentEnemyProgress /
            ScoreManager.targetEnemyProgress;

            enemyProgressBar.fillAmount =
            Mathf.Lerp(
                enemyProgressBar.fillAmount,
                target,
                Time.deltaTime * 5f
            );
        }

        // 🔥 ALL ENEMY DEFEATED TEXT
        if (allEnemyDefeatedText != null)
        {
            if (ScoreManager.IsAllEnemyDefeated())
            {
                allEnemyDefeatedText.SetActive(true);
            }
            else
            {
                allEnemyDefeatedText.SetActive(false);
            }
        }

        // 🔥 HEALTH BAR
        if (playerHealthBar)
        {
            Vector2 size =
            playerHealthBar.rectTransform.sizeDelta;

            float targetX =
            player.health /
            player.healthMax *
            playerHealthBarFullX;

            size.x =
            Mathf.Lerp(
                size.x,
                targetX,
                Time.deltaTime * 10f
            );

            playerHealthBar.rectTransform.sizeDelta = size;
        }

        // 🔥 FINISH CHECK
        CheckFinish();

        // 🔥 UI STATE
        if (levelCompleteUI)
            levelCompleteUI.SetActive(GameManager.isLevelComplete);

        if (gameOverUI)
            gameOverUI.SetActive(GameManager.isGameOver);

        if (pauseUI)
            pauseUI.SetActive(GameManager.isPaused);

        // 🔥 AUTO PAUSE
        CheckGameState();
    }

    void CheckGameState()
    {
        bool anyUIOpen = false;

        if (pauseUI != null && pauseUI.activeSelf)
            anyUIOpen = true;

        if (gameOverUI != null && gameOverUI.activeSelf)
            anyUIOpen = true;

        if (levelCompleteUI != null && levelCompleteUI.activeSelf)
            anyUIOpen = true;

        if (anyUIOpen)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    // 🔥 FINISH SYSTEM
    void CheckFinish()
    {
        if (player == null || finishPoint == null)
            return;

        float distance =
        Vector2.Distance(
            player.transform.position,
            finishPoint.position
        );

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

    // 🔊 MUSIC SETTINGS
    public void SetMusic(float value)
    {
        PlayerPrefs.SetFloat("Music", value);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }
    }

    // 🔊 SFX SETTINGS
    public void SetSFX(float value)
    {
        PlayerPrefs.SetFloat("SFX", value);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
    }
}