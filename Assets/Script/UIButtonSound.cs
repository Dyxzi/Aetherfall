using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public AudioClip sound;

    public void PlaySound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(sound);
        }
    }
}