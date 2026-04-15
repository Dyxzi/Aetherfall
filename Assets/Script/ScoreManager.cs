using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public static int currentEnemyProgress { get; private set; }
    public static int targetEnemyProgress { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EnemyControler[] enemyControllers =
            FindObjectsOfType<EnemyControler>();

        targetEnemyProgress = enemyControllers.Length;
        currentEnemyProgress = 0;
    }

    public static void DefeatEnemy()
    {
        currentEnemyProgress += 1;
    }

    // 🔥 cek apakah semua enemy sudah mati
    public static bool IsAllEnemyDefeated()
    {
        return currentEnemyProgress >= targetEnemyProgress;
    }
}