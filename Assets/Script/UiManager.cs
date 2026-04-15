using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI enemyProgressText;
    public Image playerHealthBar;
    public float playerHealthBarFullX = 78;

    public GameObject gameOverUI;
    public GameObject levelCompleteUI;
    public GameObject pauseUI;

    public PlayerMovment player;

    // 🔥 TAMBAHAN (FINISH SYSTEM)
    [Header("Finish")]
    public Transform finishPoint;
    public float finishDistance = 1.5f;

    void Start()
    {
        if (playerHealthBar)
        {
            playerHealthBar.rectTransform.pivot = new Vector2(0, 0.5f);
        }
    }
    void Update()
    {
        // Enemy progress
        if (enemyProgressText)
        {
            enemyProgressText.text = ScoreManager.currentEnemyProgress + " of "
            + ScoreManager.targetEnemyProgress;
        }

        // Health bar
        if (playerHealthBar)
        {
            Vector2 size = playerHealthBar.rectTransform.sizeDelta;
            float targetX = player.health / player.healthMax * playerHealthBarFullX;
            size.x = Mathf.Lerp(size.x, targetX, Time.deltaTime * 10f);
            playerHealthBar.rectTransform.sizeDelta = size;
        }

        // Finish check
        CheckFinish();

        // UI STATE
        if (levelCompleteUI)
            levelCompleteUI.SetActive(GameManager.isLevelComplete);

        if (gameOverUI)
            gameOverUI.SetActive(GameManager.isGameOver);

        if (pauseUI)
            pauseUI.SetActive(GameManager.isPaused);

        // 🔥 AUTO PAUSE SYSTEM
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

    // 🔥 FUNCTION BARU (FINISH SYSTEM)
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
}