using System.Collections;
using UnityEngine;

public class ball : MonoBehaviour
{
    public float forceMultiplier = 10f; // Adjust to control swipe strength
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Rigidbody rb;
    private bool hasHit = false;
    public AudioClip ballSound;
    public AudioClip pinSound;
    private AudioSource audioSource;

    private bool isBot = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // Check if this is the bot's ball
        if (SceneLoader.instance.numOfPlayers == 1 && gameManager.Instance.GetCurrentPlayer() == 2)
        {
            isBot = true;
            StartCoroutine(BotThrow());
        }
    }

    void Update()
    {
        if (isBot) return; // Bot ball won't listen to user input

        // Mouse swipe (PC)
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            Swipe();
        }

        // Touch swipe (Mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                Swipe();
            }
        }
    }

    void Swipe()
    {
        Vector2 swipeDirection = endTouchPosition - startTouchPosition;

        if (swipeDirection.magnitude > 50f) // Minimum swipe distance
        {
            ThrowBall(swipeDirection.x / 200f);
        }
    }

    private void ThrowBall(float curve)
    {
        // Base direction from camera
        Vector3 forward = Vector3.forward;

        // Add some left/right curve
        Vector3 forceDirection = forward + (Vector3.right * curve);

        if (ballSound != null && audioSource != null)
            audioSource.PlayOneShot(ballSound);

        rb.AddForce(forceDirection.normalized * forceMultiplier, ForceMode.Impulse);
    }

    private IEnumerator BotThrow()
    {
        yield return new WaitForSeconds(1.5f); // small delay to simulate thinking

        float curve = 0f;

        // TODO: later we’ll set different angle/curve values based on difficulty
        if (SceneLoader.instance.difficulty == "Easy")
            curve = Random.Range(-1f, 1f);
        else if (SceneLoader.instance.difficulty == "Medium")
            curve = Random.Range(-0.5f, 0.5f);
        else if (SceneLoader.instance.difficulty == "Hard")
            curve = 0f; // straight shot for now

        ThrowBall(curve);
    }

    // Detect collision with pin
    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHit)
        {
            if (collision.collider.CompareTag("pin"))
            {
                hasHit = true;
                if (pinSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(pinSound);
                }
                Debug.Log("Hit to pins");
                StartCoroutine(WaitAndEndTurn());
            }
            else if (collision.collider.CompareTag("channel"))
            {
                hasHit = true;
                Debug.Log("Hit to channel");
                StartCoroutine(WaitAndEndTurn());
            }
        }
    }

    private IEnumerator WaitAndEndTurn()
    {
        yield return new WaitForSeconds(5f); // wait for pins to fall
        gameManager.Instance.EndTurn(); // call manager to flip camera + switch player
    }
}
