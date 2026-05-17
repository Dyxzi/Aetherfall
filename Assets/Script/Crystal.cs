using UnityEngine;

public class Crystal : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Crystal saat ini
            ScoreManager.crystal += value;

            // 🔥 Total crystal permanen
            int total =
            PlayerPrefs.GetInt("TotalCrystal", 0);

            total += value;

            PlayerPrefs.SetInt(
                "TotalCrystal",
                total
            );

            Destroy(gameObject);
        }
    }
}