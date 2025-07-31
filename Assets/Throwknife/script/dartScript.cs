//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class dartScript : MonoBehaviour
//{
//    public Rigidbody2D rb;
//    public float throwSpeed = 10f;
//    public bool isThrown = false;
//    private bool hasScored = false;

//    void Start() { }

//    void Update()
//    {
//        if (!isThrown && Input.GetMouseButtonDown(0) && IsTouchInAllowedArea(Input.mousePosition))
//        {
//            isThrown = true;
//        }

//        if (!isThrown && Input.touchCount > 0)
//        {
//            Touch touch = Input.GetTouch(0);
//            if (touch.phase == TouchPhase.Began && IsTouchInAllowedArea(touch.position))
//            {
//                isThrown = true;
//            }
//        }

//        if (isThrown)
//        {
//            transform.Translate(Vector2.up * throwSpeed * Time.deltaTime, Space.World);
//        }
//    }

//    private bool IsTouchInAllowedArea(Vector2 screenPosition)
//    {
//        float screenWidth = Screen.width;
//        float screenHeight = Screen.height;

//        Rect allowedArea = new Rect(
//            screenWidth * 0.25f,
//            0,
//            screenWidth * 0.5f,
//            screenHeight * 0.25f
//        ); // middle bottom 25%
//        return allowedArea.Contains(screenPosition);
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        // Score first — but only once
//        if (!hasScored)
//        {
//            ScoreValueScript scorePart = collision.GetComponent<ScoreValueScript>();
//            if (scorePart != null)
//            {
//                hasScored = true;
//                Debug.Log("Score: " + scorePart.value);
//                // GameManager.instance.AddScore(scorePart.value);
//            }
//        }

//        // Stick to dartboard
//        if (collision.CompareTag("dartBoard"))
//        {
//            isThrown = false;

//            transform.SetParent(collision.transform);

//            rb = GetComponent<Rigidbody2D>();
//            if (rb != null)
//            {
//                rb.velocity = Vector2.zero;
//                rb.isKinematic = true;


//            }
//        }
//    }
//}




// this one is different 

//using System.Collections;
//using UnityEngine;

//public class dartScript : MonoBehaviour
//{
//    public Rigidbody2D rb;
//    public float throwSpeed = 10f;
//    public bool isThrown = false;
//    private bool hasScored = false;
//    private bool stuck = false;

//    void OnEnable()
//    {
//        // Reset dart state when spawned
//        isThrown = false;
//        hasScored = false;
//        stuck = false;

//        if (rb == null)
//            rb = GetComponent<Rigidbody2D>();

//        if (rb != null)
//        {
//            rb.velocity = Vector2.zero;
//            rb.isKinematic = false;
//        }

//        transform.SetParent(null); // detach from previous parent if any
//        transform.localScale = Vector3.one; // reset scale
//    }

//    void Update()
//    {
//        if (stuck) return;

//#if UNITY_EDITOR
//        if (!isThrown && Input.GetMouseButtonDown(0) && IsTouchInAllowedArea(Input.mousePosition))
//        {
//            isThrown = true;
//        }
//#endif

//        if (!isThrown && Input.touchCount > 0)
//        {
//            Touch touch = Input.GetTouch(0);
//            if (touch.phase == TouchPhase.Began && IsTouchInAllowedArea(touch.position))
//            {
//                isThrown = true;
//            }
//        }

//        if (isThrown)
//        {
//            transform.Translate(Vector2.up * throwSpeed * Time.deltaTime, Space.World);
//        }
//    }

//    private bool IsTouchInAllowedArea(Vector2 screenPosition)
//    {
//        float screenWidth = Screen.width;
//        float screenHeight = Screen.height;

//        Rect allowedArea = new Rect(
//            screenWidth * 0.25f,
//            0,
//            screenWidth * 0.5f,
//            screenHeight * 0.25f
//        );

//        return allowedArea.Contains(screenPosition);
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (stuck) return;

//        // Score once
//        if (!hasScored)
//        {
//            ScoreValueScript scorePart = collision.GetComponent<ScoreValueScript>();
//            if (scorePart != null)
//            {
//                hasScored = true;
//                Debug.Log("Score: " + scorePart.value);
//                // GameManager.instance.AddScore(scorePart.value);
//            }
//        }

//        if (collision.CompareTag("dartBoard"))
//        {
//            isThrown = false;
//            stuck = true;

//            transform.SetParent(collision.transform, worldPositionStays: true); // Keep original scale/position

//            if (rb != null)
//            {
//                rb.velocity = Vector2.zero;
//                rb.isKinematic = true;
//            }

//            StartCoroutine(SpawnNextDartWithDelay());
//        }
//    }

//    IEnumerator SpawnNextDartWithDelay()
//    {
//        yield return new WaitForSeconds(0.5f);
//        GameManager.instance.SpawnNextDart();
//    }
//}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dartScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float throwSpeed = 10f;
    public bool isThrown = false;

    private bool hasScored = false;

    void Update()
    {
        // Mouse click
        if (!isThrown && Input.GetMouseButtonDown(0) && IsTouchInAllowedArea(Input.mousePosition))
        {
            isThrown = true;
        }

        // Mobile touch
        if (!isThrown && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && IsTouchInAllowedArea(touch.position))
            {
                isThrown = true;
            }
        }

        // Move dart upward
        if (isThrown)
        {
            transform.Translate(Vector2.up * throwSpeed * Time.deltaTime, Space.World);
        }
    }

    private bool IsTouchInAllowedArea(Vector2 screenPosition)
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        Rect allowedArea = new Rect(
            screenWidth * 0.25f,
            0,
            screenWidth * 0.5f,
            screenHeight * 0.25f
        );

        return allowedArea.Contains(screenPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Hit Object: " + collision.name + " | Tag: " + collision.tag);

        // Score first — but only once
        if (!hasScored)
        {
            ScoreValueScript scorePart = collision.GetComponent<ScoreValueScript>();
            if (scorePart != null)
            {
                hasScored = true;
                Debug.Log("Score: " + scorePart.value);
                // GameManager.instance.AddScore(scorePart.value);
            }
        }

        // Stick to dartboard
        if (collision.CompareTag("dartBoard"))
        {
            isThrown = false;

            transform.SetParent(collision.transform);

            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
            }
        }
    }
}


