using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver { get; private set; }
    public static bool isLevelComplete { get; private set; }
    public static bool isPaused { get; private set; }

    [Header("UI")]
    public GameObject pausePanel;
    public GameObject pauseButton;

    [Header("Finish")]
    public Transform finishPoint;
    public float finishDistance = 1.5f;

    private void Awake()
    {
        isGameOver = false;
        isLevelComplete = false;
        isPaused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isGameOver || isLevelComplete) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public static void GameOver()
    {
        isGameOver = true;
    }

    public static void CompleteLevel()
    {
        isLevelComplete = true;
        Time.timeScale = 0f; // 🔥 freeze game saat selesai
    }

    // =====================
    // 🎮 BUTTON FUNCTIONS
    // =====================

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // =====================
    // ⏸️ PAUSE SYSTEM
    // =====================

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (pauseButton != null)
            pauseButton.SetActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (pauseButton != null)
            pauseButton.SetActive(true);
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    // =====================
    // 🚪 EXIT GAME
    // =====================

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}