//using System.Collections;
//using UnityEngine;

//public class dartScript : MonoBehaviour
//{
//    public float throwForce = 100f;
//    public Rigidbody2D rb;
//    public bool isThrown = false;
//    private bool hasScored = false;
//    private bool isStuck = false;
//    public int playerNumber = 1;
//    public GameObject hitEffect;

//    private bool inputLocked = false;
//    private bool turnProcessed = false;

//    public AudioClip stickSound;
//    private boardRotator rotator;

//    private void Start()
//    {
//        rotator = FindObjectOfType<boardRotator>();
//    }

//    private void Update()
//    {
//        if (isThrown || inputLocked) return;
//        if (scoreManager.instance == null) return;
//        if (scoreManager.instance.currentPlayer != playerNumber) return;

//        bool isSoloMode = scoreManager.instance.IsSoloMode;
//        Vector3 mousePos = Input.mousePosition;

//        if (playerNumber == 1)
//        {
//            if (Input.GetMouseButtonDown(0) && mousePos.y <= Screen.height * 0.5f)
//            {
//                inputLocked = true;
//                ThrowDart();
//            }
//        }
//        else
//        {
//            if (isSoloMode)
//            {
//                inputLocked = true;
//                StartCoroutine(BotThrowRoutine());
//            }
//            else
//            {
//                if (Input.GetMouseButtonDown(0) && mousePos.y > Screen.height * 0.5f)
//                {
//                    inputLocked = true;
//                    ThrowDart();
//                }
//            }
//        }
//    }

//    private IEnumerator BotThrowRoutine()
//    {
//        string difficulty = scoreManager.instance.Difficulty;

//        int[] targetScores;
//        switch (difficulty.ToLower())
//        {
//            case "easy":
//                targetScores = new int[] { 2 };
//                break;
//            case "medium":
//                targetScores = new int[] { 2, 3 };
//                break;
//            default:
//                targetScores = new int[] { 1, 4, 5 };
//                break;
//        }

//        // Adjust this value to fit your game's dart physics
//        float dartTravelTime = 0.25f;

//        // Degrees of tolerance around 'top' to consider a valid shot
//        float aimTolerance = 10f;

//        while (true)
//        {
//            var (predictedScore, angleToTop) = rotator.ClosestZoneToTopAfter(dartTravelTime);

//            if (System.Array.Exists(targetScores, score => score == predictedScore) && angleToTop <= aimTolerance)
//            {
//                ThrowDart(false); // Bot throws
//                yield break;
//            }
//            yield return null;
//        }
//    }

//    private void ThrowDart(bool isBot = false)
//    {
//        if (isThrown) return;

//        isThrown = true;

//        Vector2 throwDirection = (playerNumber == 1) ? Vector2.up : Vector2.down;
//        rb.velocity = Vector2.zero;
//        rb.angularVelocity = 0;
//        rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);

//        scoreManager.instance.RegisterThrow(playerNumber);
//        scoreManager.instance.spawner.UpdateQueue(playerNumber);
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("dart"))
//        {
//            dartScript otherDart = collision.GetComponent<dartScript>();
//            if (otherDart != null && otherDart.isStuck)
//            {
//                if (hitEffect != null)
//                    Instantiate(hitEffect, transform.position, Quaternion.identity);

//                hasScored = true;
//                ProcessNextTurn();
//                Destroy(gameObject, 0.1f);
//                return;
//            }
//        }

//        if (!hasScored)
//        {
//            ScoreValueScript scorePart = collision.GetComponent<ScoreValueScript>();
//            if (scorePart != null)
//            {
//                hasScored = true;
//                scoreManager.instance.AddScore(playerNumber, scorePart.value);
//            }
//        }

//        if (collision.CompareTag("dartBoard"))
//        {
//            StickDart(collision.transform);
//        }
//    }

//    private void StickDart(Transform parent)
//    {
//        isThrown = false;
//        isStuck = true;

//        rb.velocity = Vector2.zero;
//        rb.angularVelocity = 0f;
//        rb.isKinematic = true;

//        transform.SetParent(parent);

//        AudioSource.PlayClipAtPoint(stickSound, transform.position);

//        // Resume board rotation and process next turn after delay
//        Invoke(nameof(ResumeBoard), 0.5f);
//        Invoke(nameof(ProcessNextTurn), 0.5f);
//    }

//    private void ResumeBoard()
//    {
//        if (rotator != null)
//            rotator.enabled = true;
//    }

//    private void ProcessNextTurn()
//    {
//        if (!turnProcessed)
//        {
//            turnProcessed = true;
//            scoreManager.instance.NextTurn();
//        }
//    }
//}

using System.Collections;
using UnityEngine;

public class dartScript : MonoBehaviour
{
    public float throwForce = 50f;
    public Rigidbody2D rb;
    public bool isThrown = false;
    private bool hasScored = false;
    private bool isStuck = false;
    public int playerNumber = 1;
    public GameObject hitEffect;

    private bool inputLocked = false;
    private bool turnProcessed = false;
    private float throwCooldown = 0.3f; // Cooldown to prevent rapid throws
    private float lastThrowTime = 0f;

    public AudioClip stickSound;
    private boardRotator rotator;

    private void Start()
    {
        rotator = FindObjectOfType<boardRotator>();
    }

    private void Update()
    {
        if (isThrown || inputLocked || Time.time < lastThrowTime + throwCooldown) return;
        if (scoreManager.instance == null) return;

        bool isSoloMode = scoreManager.instance.IsSoloMode;
        Vector3 mousePos = Input.mousePosition;

        if (playerNumber == 1)
        {
            if (Input.GetMouseButtonDown(0) && mousePos.y <= Screen.height * 0.5f)
            {
                inputLocked = true;
                lastThrowTime = Time.time;
                ThrowDart();
            }
        }
        else
        {
            if (isSoloMode)
            {
                inputLocked = true;
                StartCoroutine(BotThrowRoutine());
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && mousePos.y > Screen.height * 0.5f)
                {
                    inputLocked = true;
                    lastThrowTime = Time.time;
                    ThrowDart();
                }
            }
        }
    }

    private IEnumerator BotThrowRoutine()
    {
        string difficulty = scoreManager.instance.Difficulty;
        int winOrLose = scoreManager.instance.WinOrLose; // Access the WinOrLose value

        int[] targetScores;

        // Adjust targets based on win/lose and difficulty
        if (winOrLose == 1) // Bot aims to win: target higher scores
        {
            switch (difficulty.ToLower())
            {
                case "easy":
                    targetScores = new int[] { 2 , 3 }; // Moderate-high for easy win
                    break;
                case "medium":
                    targetScores = new int[] { 3 , 4 }; // High for medium win
                    break;
                default: // hard
                    targetScores = new int[] { 2 ,3, 4, 5 }; // Only best for hard win
                    break;
            }
        }
        else // winOrLose == 0: Bot aims to lose: target lower scores
        {
            switch (difficulty.ToLower())
            {
                case "easy":
                    targetScores = new int[] { 1 }; // Very low for easy lose
                    break;
                case "medium":
                    targetScores = new int[] { 1, 2 }; // Low for medium lose
                    break;
                default: // hard
                    targetScores = new int[] { 1, 2}; // Mid-low for hard lose (still challenging)
                    break;
            }
        }

        float dartTravelTime = 2f;
        float aimTolerance = 10f;
        float maxWaitTime = 5f; // Prevent bot from getting stuck
        float startTime = Time.time;

        while (Time.time < startTime + maxWaitTime)
        {
            var (predictedScore, angleToTop) = rotator.ClosestZoneToTopAfter(dartTravelTime);

            if (System.Array.Exists(targetScores, score => score == predictedScore) && angleToTop <= aimTolerance)
            {
                ThrowDart(true); // Bot throws
                yield break;
            }
            yield return null;
        }

        // Fallback: Throw if no ideal condition is met
        ThrowDart(true);
        yield break;
    }

    private void ThrowDart(bool isBot = false)
    {
        if (isThrown) return;

        isThrown = true;

        Vector2 throwDirection = (playerNumber == 1) ? Vector2.up : Vector2.down;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);

        // NEW: Fallback for miss (if no collision after 2s, complete turn with 0 score)
        Invoke(nameof(ForceProcessNextTurn), 2f);
    }

    // NEW: Fallback method for misses
    private void ForceProcessNextTurn()
    {
        if (!hasScored && !isStuck)
        {
            hasScored = true; // Mark as scored (0 points)
            ProcessNextTurn();
        }
    }

    private void ProcessNextTurn()
    {
        if (!turnProcessed)
        {
            turnProcessed = true;
            inputLocked = false;
            scoreManager.instance.RegisterCompletedThrow(playerNumber); // NEW: Call completion here
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("dart"))
        {
            dartScript otherDart = collision.GetComponent<dartScript>();
            if (otherDart != null && otherDart.isStuck)
            {
                if (hitEffect != null)
                    Instantiate(hitEffect, transform.position, Quaternion.identity);

                hasScored = true;
                ProcessNextTurn();
                Destroy(gameObject, 0.1f);
                return;
            }
        }

        if (!hasScored)
        {
            ScoreValueScript scorePart = collision.GetComponent<ScoreValueScript>();
            if (scorePart != null)
            {
                hasScored = true;
                scoreManager.instance.AddScore(playerNumber, scorePart.value);
            }
        }

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

        Invoke(nameof(ResumeBoard), 0.5f);
        Invoke(nameof(ProcessNextTurn), 0.5f);
    }

    private void ResumeBoard()
    {
        if (rotator != null)
            rotator.enabled = true;
    }
}
