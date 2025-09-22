using System.Collections;
using TMPro;
using UnityEngine;

public class Bluetruck : MonoBehaviour
{
    public float laneDistance = 1f;
    public float laneChangeSpeed = 10f;
    public float jumpForce = 25f;
    private bool isGrounded = true;

    private int currentLane = 0;
    private Rigidbody rb;

    public AudioClip obstacleHitSound;
    public AudioClip LaneChangeSound;
    private AudioSource audioSource;

    public GameObject hitEffectPrefab;
    public Transform effectPoint;

    [Header("Pause time on hit")]
    public float HoldTime = 1f;

    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private bool swipeDetected = false;

    // Score
    public int Score = 0;
    public TextMeshProUGUI BlueScore;

    public PlayerManager owner;

    public float detectDistance = 15f; // how far ahead to check (along Z)
    public string obstacleTag = "Obstacle";
    public bool botJumping = false;

    public TrailRenderer frontRightTrail;
    public TrailRenderer backRightTrail;
    public TrailRenderer frontLeftTrail;
    public TrailRenderer backLeftTrail;

    // Trail control
    private Coroutine tireCoroutine = null;
    public float tireDuration = 0.5f;          // how long the trail should remain (subject to grounded check)
    public float landingCooldown = 0.1f;       // short buffer after landing before trails allowed
    private float lastLandedTime = -10f;

    // 🔹 Bot-related
    int[] easyArr = { 0, 1, 1, 0, 1, 0, 1, 0, 1, 0 };
    int[] mediumArr = { 1, 1, 1, 0, 1, 0, 0, 1, 0, 1 };
    int[] hardArr = { 1, 1, 1, 0, 1, 1, 1, 0, 1, 1 };
    int[] difficultyArr;
    int WinOrLose;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        // If solo mode → setup bot difficulty
        if (SceneLoader.instance.numOfPlayers == 1)
        {
            if (SceneLoader.instance.difficulty == "easy")
                difficultyArr = easyArr;
            else if (SceneLoader.instance.difficulty == "medium")
                difficultyArr = mediumArr;
            else
                difficultyArr = hardArr;
        }

        // ensure trails start off
        DisableTrails();
    }

    void Update()
    {
        if (SceneLoader.instance.numOfPlayers == 1)
        {
            HandleBot(); // BOT control
        }
        else
        {
            DetectSwipe(); // PLAYER control
        }

        if (isGrounded)
        {
            Vector3 targetPos = new Vector3(
                currentLane * laneDistance,
                rb.position.y,
                rb.position.z
            );
            Vector3 newPos = Vector3.Lerp(rb.position, targetPos, Time.deltaTime * laneChangeSpeed);
            rb.MovePosition(newPos);
        }
    }

    // ---------------- MANUAL CONTROL ----------------
    void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.y > Screen.height * 0.5f)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    startTouchPos = touch.position;
                    swipeDetected = true;
                }
                else if (touch.phase == TouchPhase.Ended && swipeDetected)
                {
                    endTouchPos = touch.position;
                    HandleSwipe(endTouchPos - startTouchPos);
                    swipeDetected = false;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > Screen.height * 0.5f)
        {
            startTouchPos = Input.mousePosition;
            swipeDetected = true;
        }
        else if (Input.GetMouseButtonUp(0) && swipeDetected)
        {
            endTouchPos = (Vector2)Input.mousePosition;
            HandleSwipe(endTouchPos - startTouchPos);
            swipeDetected = false;
        }
    }

    void SpawnTireMark()
    {
        // don't spawn if mid-air or bot is in a jump state, or if just landed (cooldown)
        if (!isGrounded || botJumping) return;
        if (Time.time - lastLandedTime < landingCooldown) return;

        // restart coroutine if already running
        if (tireCoroutine != null) StopCoroutine(tireCoroutine);
        tireCoroutine = StartCoroutine(EnableTireTrail());
    }

    IEnumerator EnableTireTrail()
    {
        // final pre-check: if we are not grounded then bail out
        if (!isGrounded || botJumping)
        {
            tireCoroutine = null;
            yield break;
        }

        // enable
        frontRightTrail.emitting = true;
        backRightTrail.emitting = true;
        frontLeftTrail.emitting = true;
        backLeftTrail.emitting = true;

        float elapsed = 0f;
        while (elapsed < tireDuration)
        {
            // if truck leaves ground or bot starts jumping, stop immediately
            if (!isGrounded || botJumping) break;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // disable
        DisableTrails();
        tireCoroutine = null;
    }

    void DisableTrails()
    {
        frontRightTrail.emitting = false;
        backRightTrail.emitting = false;
        frontLeftTrail.emitting = false;
        backLeftTrail.emitting = false;
    }

    void HandleSwipe(Vector2 swipeDelta)
    {
        if (swipeDelta.magnitude < 50f)
            return;

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x < 0 && currentLane < 1)
            {
                currentLane++;
                if (isGrounded && !botJumping) SpawnTireMark();
                audioSource.PlayOneShot(LaneChangeSound);
            }
            else if (swipeDelta.x > 0 && currentLane > -1)
            {
                currentLane--;
                if (isGrounded && !botJumping) SpawnTireMark();
                audioSource.PlayOneShot(LaneChangeSound);
            }
        }
        else
        {
            if (swipeDelta.y < 0 && isGrounded)
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void HandleBot()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag(obstacleTag);

        foreach (GameObject obs in obstacles)
        {
            // Only check obstacles in the same lane
            int obsLane = Mathf.RoundToInt(obs.transform.position.x / laneDistance);

            if (obsLane == currentLane)
            {
                float zDiff = obs.transform.position.z - transform.position.z;

                // If obstacle is ahead and close
                if (zDiff > 0 && zDiff <= detectDistance)
                {
                    DecideAction();
                    break; // act only once per obstacle
                }
            }
        }
    }

    void DecideAction()
    {

        if (difficultyArr != null && difficultyArr.Length > 0)
        {
            WinOrLose = difficultyArr[Random.Range(0, difficultyArr.Length)];
            Debug.Log("Bot decision: " + WinOrLose);

            if (WinOrLose == 1)
            {
                // Bot avoids obstacle
                int action = Random.Range(0, 3);

                if (action == 0) // Move left
                {
                    if (currentLane > -1)
                    {
                        currentLane--;
                        if (isGrounded && !botJumping) SpawnTireMark();
                        audioSource.PlayOneShot(LaneChangeSound);
                    }
                }
                else if (action == 1) // Move right
                {
                    if (currentLane < 1)
                    {
                        currentLane++;
                        if (isGrounded && !botJumping) SpawnTireMark();
                        audioSource.PlayOneShot(LaneChangeSound);
                    }
                }
                else if (action == 2) // Jump
                {
                    if ( isGrounded && !botJumping)
                    {
                        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                        botJumping = true;
                        isGrounded = false;
                    }
                }
            }
            else
            {
                // Bot fails → do nothing
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            botJumping = false;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            audioSource.PlayOneShot(obstacleHitSound);
            owner.StopForSeconds(HoldTime);

            if (hitEffectPrefab != null)
            {
                // Spawn effect at effectPoint (child of truck) or fallback to truck position
                Transform parent = effectPoint != null ? effectPoint : transform;
                GameObject effect = Instantiate(
                    hitEffectPrefab,
                    parent.position,
                    Quaternion.identity,
                    parent
                );

                // Auto-destroy after 1 second
                Destroy(effect, 2f);
            }
        }

        if (collision.gameObject.CompareTag("CheckPoint"))
        {
            Score++;
            BlueScore.text = Score.ToString();
            Debug.Log("BlueScore = " + Score);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
