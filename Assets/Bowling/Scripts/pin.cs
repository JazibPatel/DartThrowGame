using UnityEngine;
using UnityEngine.SceneManagement;

public class pin : MonoBehaviour
{
    private bool scored = false;
    private AudioSource audioSource;
    public AudioClip pinSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        
        if (!scored)
        {
            float tilt = Vector3.Angle(Vector3.up, transform.up);

            if (tilt > 45f) // Pin fell
            {
                scored = true;
                if (audioSource != null && pinSound != null)
                {
                    audioSource.PlayOneShot(pinSound);
                }
                int currentPlayer = gameManager.Instance.GetCurrentPlayer();
                bowlingScoreManager.Instance.AddScore(currentPlayer, 1);
            }
        }
    }
}
