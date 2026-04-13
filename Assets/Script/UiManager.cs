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

    void Start()
    {
        if (playerHealthBar)
        {
            playerHealthBar.rectTransform.pivot = new Vector2(0, 0.5f);
        }
    }

    void Update()
    {
        if (enemyProgressText)
        {
            enemyProgressText.text = ScoreManager.currentEnemyProgress + " of "
            + ScoreManager.targetEnemyProgress;
        }
        if (playerHealthBar)
        {
            Vector2 size = playerHealthBar.rectTransform.sizeDelta;
            float targetX = player.health / player.healthMax * playerHealthBarFullX;
            size.x = Mathf.Lerp(size.x, targetX, Time.deltaTime * 10f);
            playerHealthBar.rectTransform.sizeDelta = size;
        }
        if (levelCompleteUI && GameManager.isLevelComplete)
        {
            levelCompleteUI.SetActive(GameManager.isLevelComplete);
        }
        else if (gameOverUI && GameManager.isGameOver)
        {
            gameOverUI.SetActive(GameManager.isGameOver);
        }
        if (pauseUI)
        {
            pauseUI.SetActive(GameManager.isPaused);
        }
    }



}
