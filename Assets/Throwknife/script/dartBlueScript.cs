//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class dartBlueScript : MonoBehaviour
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
//            transform.Translate(Vector2.down * throwSpeed * Time.deltaTime, Space.World);
//        }
//    }

//    private bool IsTouchInAllowedArea(Vector2 screenPosition)
//    {
//        float screenWidth = Screen.width;
//        float screenHeight = Screen.height;

//        Rect allowedArea = new Rect(
//            screenWidth * 0.25f, // Start X: 25% from left
//            screenHeight * 0.75f, // Start Y: 75% from bottom (top 25%)
//            screenWidth * 0.5f, // Width: 50% of screen
//            screenHeight * 0.25f // Height: 25% of screen
//        );
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


using UnityEngine;

public class dartBlueScript : MonoBehaviour
{
    public float throwForce = 100f;
    public Rigidbody2D rb;
    public bool isThrown = false;
    private bool hasScored = false;
    private bool isStuck = false; // NEW: Only true when dart hits the board

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isThrown)
            {
                isThrown = true;
                rb.AddForce(Vector2.up * throwForce);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Score detection
        if (!hasScored)
        {
            ScoreValueScript scorePart = collision.GetComponent<ScoreValueScript>();
            if (scorePart != null)
            {
                hasScored = true;
                Debug.Log("Score: " + scorePart.value);
            }
        }

        // Stick to dartboard
        if (collision.CompareTag("dartBoard"))
        {
            StickDart(collision.transform);
        }

        // Hit another dart
        if (collision.CompareTag("dart"))
        {
            dartBlueScript otherDart = collision.GetComponent<dartBlueScript>();

            // Destroy only if the other dart is already stuck
            if (otherDart != null && otherDart.isStuck)
            {
                Debug.Log("Hit a stuck dart! Destroying this dart.");
                Destroy(gameObject);
            }
        }
    }

    private void StickDart(Transform parent)
    {
        isThrown = false;
        isStuck = true; // Now this dart can block others
        transform.SetParent(parent);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }
}