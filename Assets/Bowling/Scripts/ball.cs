using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ball : MonoBehaviour
{
    public float forceMultiplier = 50f; // Adjust to control swipe strength
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
        if (isBot)
            return; // Bot ball won't listen to user input

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

        // Difficulty arrays
        int[] easyArr = { 0, 1, 1, 0, 1, 0, 1, 0, 1, 0 };
        int[] mediumArr = { 1, 1, 1, 0, 1, 0, 0, 1, 0, 1 };
        int[] hardArr = { 1, 1, 1, 0, 1, 1, 1, 0, 1, 1 };
        int[] difficultyArr = new int[10];
        int winOrLose = 1;

        // Pick difficulty pattern
        if (SceneLoader.instance.difficulty == "easy")
        {
            System.Array.Copy(easyArr, difficultyArr, easyArr.Length);
        }
        else if (SceneLoader.instance.difficulty == "medium")
        {
            System.Array.Copy(mediumArr, difficultyArr, mediumArr.Length);
        }
        else
        {
            System.Array.Copy(hardArr, difficultyArr, hardArr.Length);
        }

        // Pick random outcome
        winOrLose = difficultyArr[Random.Range(0, difficultyArr.Length)];

        float[] easyLoseAngles = { -15f, 15f, -12f, 12f };
        float[] easyWinAngles = { -10f, 10f, -11f, 11f };

        float[] mediumLoseAngles = { -9f, 9f, -10f, 10f };
        float[] mediumWinAngles = { -8f, 8f, -8.5f, 8.5f };

        float[] hardLoseAngles = { -6f, 6f, -5f, 5f };
        float[] hardWinAngles = { 0f };

        float angleY = 0f;

        // 🎯 Select angle sets based on mode & win/lose
        if (SceneLoader.instance.difficulty == "easy")
        {
            angleY =
                (winOrLose == 0)
                    ? mediumLoseAngles[Random.Range(0, easyLoseAngles.Length)]
                    : mediumWinAngles[Random.Range(0, easyWinAngles.Length)];
        }
        else if (SceneLoader.instance.difficulty == "medium")
        {
            angleY =
                (winOrLose == 0)
                    ? mediumLoseAngles[Random.Range(0, mediumLoseAngles.Length)]
                    : mediumWinAngles[Random.Range(0, mediumWinAngles.Length)];
        }
        else if (SceneLoader.instance.difficulty == "hard")
        {
            angleY =
                (winOrLose == 0)
                    ? hardLoseAngles[Random.Range(0, hardLoseAngles.Length)]
                    : hardWinAngles[Random.Range(0, hardWinAngles.Length)];
        }

        // Rotate forward vector by chosen angle
        Vector3 forward = Quaternion.Euler(0, angleY, 0) * Vector3.forward;


        if (ballSound != null && audioSource != null)
            audioSource.PlayOneShot(ballSound);

        rb.AddForce(forward.normalized * forceMultiplier, ForceMode.Impulse);
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
