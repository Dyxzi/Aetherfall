using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    [Header("Daftar Tombol Level")]
    [Tooltip("Masukkan tombol Level 1 di index 0, Level 2 di index 1, dst.")]
    public Button[] levelButtons;

    void Start()
    {
        // Ambil data level yang sudah terbuka (default 1)
        int reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);

        for (int i = 1; i < levelButtons.Length; i++)
        {
            // Level 1 (index 0) selalu terbuka
            // Level 2 (index 1) hanya terbuka jika reachedLevel >= 2
            if (i + 1 > reachedLevel)
            {
                levelButtons[i].interactable = false; // Tombol jadi tidak bisa diklik
                
                // Opsional: bikin warnanya jadi agak gelap/transparan
                var colors = levelButtons[i].colors;
                colors.disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                levelButtons[i].colors = colors;
            }
        }
    }

    // Fungsi untuk dipasang di Button OnClick
    public void OpenLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Gunakan ini untuk ngetes/reset dari awal lagi
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("ReachedLevel");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}