using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public static int currentEnemyProgress { get; private set; }
    public static int targetEnemyProgress { get; private set; }

    [Header("Level Settings")]
    [SerializeField] private int levelToUnlock; // Isi di Inspector (Contoh: isi 2 jika ini Level 1)

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Mencari semua enemy di awal level
        EnemyControler[] enemyControllers = FindObjectsByType<EnemyControler>(FindObjectsSortMode.None);
        
        targetEnemyProgress = enemyControllers.Length;
        currentEnemyProgress = 0;
    }

    public static void DefeatEnemy()
    {
        currentEnemyProgress += 1;

        // Cek jika semua musuh sudah mati, langsung buka level selanjutnya
        if (IsAllEnemyDefeated())
        {
            UnlockLevel();
        }
    }

    public static bool IsAllEnemyDefeated()
    {
        // Menjaga agar tidak error jika target adalah 0
        if (targetEnemyProgress == 0) return false;
        return currentEnemyProgress >= targetEnemyProgress;
    }

    private static void UnlockLevel()
    {
        // Kita butuh referensi ke levelToUnlock dari Instance karena fungsinya static
        int nextLevel = Instance.levelToUnlock;

        int reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);

        if (nextLevel > reachedLevel)
        {
            PlayerPrefs.SetInt("ReachedLevel", nextLevel);
            PlayerPrefs.Save();
            Debug.Log("Selamat! Level " + nextLevel + " sekarang terbuka.");
        }
    }
}