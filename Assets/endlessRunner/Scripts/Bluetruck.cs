using System.Collections;
using UnityEngine;
using TMPro;
public class Bluetruck : MonoBehaviour
{
    public float laneDistance = 3f; // distance between lanes
    public float laneChangeSpeed = 5f; // smooth movement
    public float jumpForce = 7f;
    private bool isGrounded = true;

    private int currentLane = 0; // -1 left, 0 center, 1 right
    private Rigidbody rb;

    public AudioClip obstacleHitSound;
    private AudioSource audioSource;

    [Header("Pause time on hit")]
    public float HoldTime;

    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private bool swipeDetected = false;

    // Score
    public int Score = 0;
    public TextMeshProUGUI BlueScore;

    public PlayerManager owner;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        DetectSwipe();

        // Smooth lane movement
        Vector3 targetPos = new Vector3(currentLane * laneDistance, rb.position.y, rb.position.z);
        Vector3 newPos = Vector3.Lerp(rb.position, targetPos, Time.deltaTime * laneChangeSpeed);
        rb.MovePosition(newPos);
    }

    void DetectSwipe()
    {
        // Touch input (for mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Only react to top half of screen
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

        // Mouse input (for editor testing)
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

    void HandleSwipe(Vector2 swipeDelta)
    {
        if (swipeDelta.magnitude < 50f) return; // ignore small swipes

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            // Horizontal swipe (flipped)
            if (swipeDelta.x < 0 && currentLane < 1)   // LEFT swipe → go RIGHT
            {
                currentLane++;
            }
            else if (swipeDelta.x > 0 && currentLane > -1) // RIGHT swipe → go LEFT
            {
                currentLane--;
            }
        }
        else
        {
            // Vertical swipe (flipped)
            if (swipeDelta.y < 0 && isGrounded) // Down swipe → JUMP
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    //IEnumerator KnockbackWorld()
    //{
    //    float originalRoad = gameManager.Instance.roadSpeed;
    //    float originalObs = gameManager.Instance.barrierSpeed;
    //    float originalTree = gameManager.Instance.treespeed;
    //    float originalCheck = gameManager.Instance.checkPointSpeed;

    //    gameManager.Instance.roadSpeed = 0f;
    //    gameManager.Instance.barrierSpeed = -originalObs * 0.01f;
    //    gameManager.Instance.treespeed = 0f;
    //    gameManager.Instance.checkPointSpeed = 0f;

    //    yield return new WaitForSeconds(HoldTime);

    //    gameManager.Instance.roadSpeed = originalRoad;
    //    gameManager.Instance.barrierSpeed = originalObs;
    //    gameManager.Instance.treespeed = originalTree;
    //    gameManager.Instance.checkPointSpeed = originalCheck;
    //}
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground2"))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle2"))
        {
            audioSource.PlayOneShot(obstacleHitSound, 0.3f);
            //StartCoroutine(KnockbackWorld());
            owner.StopForSeconds(HoldTime);
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
        if (collision.gameObject.CompareTag("Ground2"))
        {
            isGrounded = false;
        }
    }
}


