//using System.Collections;
//using System.Linq;
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
//        switch (difficulty)
//        {
//            case "Easy": targetScores = new int[] { 1, 2 }; break;
//            case "Medium": targetScores = new int[] { 3 }; break;
//            default: targetScores = new int[] { 5 }; break;
//        }

//        while (true)
//        {
//            int currentTopScore = rotator.GetCurrentTopScore();
//            Debug.Log("Top Score Zone: " + currentTopScore);

//            if (System.Array.Exists(targetScores, score => score == currentTopScore))
//            {
//                ThrowDart(false); // false = bot
//                yield break;
//            }

//            yield return null; // wait one frame and check again
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

//        // Resume rotation after 0.5 sec
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
    public float throwForce = 100f;
    public Rigidbody2D rb;
    public bool isThrown = false;
    private bool hasScored = false;
    private bool isStuck = false;
    public int playerNumber = 1;
    public GameObject hitEffect;

    private bool inputLocked = false;
    private bool turnProcessed = false;

    public AudioClip stickSound;
    private boardRotator rotator;

    private void Start()
    {
        rotator = FindObjectOfType<boardRotator>();
    }

    private void Update()
    {
        if (isThrown || inputLocked) return;
        if (scoreManager.instance == null) return;
        if (scoreManager.instance.currentPlayer != playerNumber) return;

        bool isSoloMode = scoreManager.instance.IsSoloMode;
        Vector3 mousePos = Input.mousePosition;

        if (playerNumber == 1)
        {
            if (Input.GetMouseButtonDown(0) && mousePos.y <= Screen.height * 0.5f)
            {
                inputLocked = true;
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
                    ThrowDart();
                }
            }
        }
    }

    private IEnumerator BotThrowRoutine()
    {
        string difficulty = scoreManager.instance.Difficulty;

        int[] targetScores;
        switch (difficulty.ToLower())
        {
            case "easy":
                targetScores = new int[] { 2 };
                break;
            case "medium":
                targetScores = new int[] { 2, 3 };
                break;
            default:
                targetScores = new int[] { 1, 4, 5 };
                break;
        }

        // Adjust this value to fit your game's dart physics
        float dartTravelTime = 0.25f;

        // Degrees of tolerance around 'top' to consider a valid shot
        float aimTolerance = 10f;

        while (true)
        {
            var (predictedScore, angleToTop) = rotator.ClosestZoneToTopAfter(dartTravelTime);

            if (System.Array.Exists(targetScores, score => score == predictedScore) && angleToTop <= aimTolerance)
            {
                ThrowDart(false); // Bot throws
                yield break;
            }
            yield return null;
        }
    }

    private void ThrowDart(bool isBot = false)
    {
        if (isThrown) return;

        isThrown = true;

        Vector2 throwDirection = (playerNumber == 1) ? Vector2.up : Vector2.down;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);

        scoreManager.instance.RegisterThrow(playerNumber);
        scoreManager.instance.spawner.UpdateQueue(playerNumber);
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

        // Resume board rotation and process next turn after delay
        Invoke(nameof(ResumeBoard), 0.5f);
        Invoke(nameof(ProcessNextTurn), 0.5f);
    }

    private void ResumeBoard()
    {
        if (rotator != null)
            rotator.enabled = true;
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
