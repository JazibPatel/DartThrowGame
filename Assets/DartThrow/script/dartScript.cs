using UnityEngine;
using System.Collections;

public class dartScript : MonoBehaviour
{
    public float throwForce = 100f;
    public Rigidbody2D rb;
    public bool isThrown = false;
    private bool hasScored = false;
    private bool isStuck = false;
    public int playerNumber = 1; // 1=Red, 2=Blue
    public GameObject hitEffect;

    private bool inputLocked = false;    // prevents double throw
    private bool turnProcessed = false;  // prevents multiple NextTurn calls

    public AudioClip stickSound;

    private void Update()
    {
        if (isThrown || inputLocked) return;
        if (scoreManager.instance == null) return;
        if (scoreManager.instance.currentPlayer != playerNumber) return;

        // Mouse input (Single Click)
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            if ((playerNumber == 1 && mousePos.y <= Screen.height * 0.2f) ||
                (playerNumber == 2 && mousePos.y >= Screen.height * 0.8f))
            {
                inputLocked = true;  // 🔹 Lock immediately
                ThrowDart();
            }
        }
    }

    private void ThrowDart()
    {
        if (isThrown) return;

        isThrown = true;

        // Reset velocity and throw
        Vector2 throwDirection = (playerNumber == 1) ? Vector2.up : Vector2.down;
        rb.velocity = Vector2.zero;
        rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);

        // Register throw & update queue
        scoreManager.instance.RegisterThrow(playerNumber);
        scoreManager.instance.spawner.UpdateQueue(playerNumber);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Hit another dart
        if (collision.CompareTag("dart"))
        {
            dartScript otherDart = collision.GetComponent<dartScript>();
            if (otherDart != null && otherDart.isStuck)
            {
                Debug.Log("Hit a stuck dart!");
                if (hitEffect != null)
                    Instantiate(hitEffect, transform.position, Quaternion.identity);

                hasScored = true;

                ProcessNextTurn();  // 🔹 Only one call
                Destroy(gameObject, 0.1f);
                return;
            }
        }

        // Score detection
        if (!hasScored)
        {
            ScoreValueScript scorePart = collision.GetComponent<ScoreValueScript>();
            if (scorePart != null)
            {
                hasScored = true;
                scoreManager.instance.AddScore(playerNumber, scorePart.value);
            }
        }

        // Stick to board
        if (collision.CompareTag("dartBoard"))
        {
            StickDart(collision.transform);
        }
    }

    private void StickDart(Transform parent)
    {
        isThrown = false;
        isStuck = true;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;

        transform.SetParent(parent);

        AudioSource.PlayClipAtPoint(stickSound, transform.position);

        Invoke(nameof(ProcessNextTurn), 0.5f);
    }

    private void ProcessNextTurn()
    {
        if (!turnProcessed)
        {
            turnProcessed = true;
            scoreManager.instance.NextTurn();
        }
    }
}
