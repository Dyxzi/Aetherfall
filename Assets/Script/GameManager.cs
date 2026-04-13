using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver { get; private set; }
    public static bool isLevelComplete { get; private set; }
    public static bool isPaused { get; private set; }

    private void Awake()
    {
        isGameOver = false;
        isLevelComplete = false;
        isPaused = false;
        Time.timeScale = 1f; // pastikan normal saat start
    }

    public static void GameOver()
    {
        isGameOver = true;
    }

    public static void CompleteLevel()
    {
        isLevelComplete = true;
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
        Time.timeScale = 0f; // stop game
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // lanjutkan game
    }

    // =====================
    // 🚪 EXIT GAME
    // =====================

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // biasanya Main Menu
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1); // scene game
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}