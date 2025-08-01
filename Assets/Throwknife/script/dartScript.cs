//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class dartScript : MonoBehaviour
//{
//    public Rigidbody2D rb;
//    public float throwSpeed = 10f;
//    public bool isThrown = false;

//    private bool hasScored = false;

//    void Update()
//    {
//        // Mouse click
//        if (!isThrown && Input.GetMouseButtonDown(0) && IsTouchInAllowedArea(Input.mousePosition))
//        {
//            isThrown = true;
//        }

//        // Mobile touch
//        if (!isThrown && Input.touchCount > 0)
//        {
//            Touch touch = Input.GetTouch(0);
//            if (touch.phase == TouchPhase.Began && IsTouchInAllowedArea(touch.position))
//            {
//                isThrown = true;
//            }
//        }

//        // Move dart upward
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
//        //Debug.Log("Hit Object: " + collision.name + " | Tag: " + collision.tag);

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

//            if (rb != null)
//            {
//                rb.velocity = Vector2.zero;
//                rb.isKinematic = true;
//            }
//        }
//    }
//}



using UnityEngine;

public class dartScript : MonoBehaviour
{
    public float throwForce = 100f;
    public Rigidbody2D rb;
    public bool isThrown = false;
    private bool hasScored = false;
    private bool isStuck = false; // NEW: Only true when dart hits the board
    private static int dartsThrown = 0;       // Shared among all darts
    public static int maxDarts = 5;           // Limit to 5 darts
    public GameObject hitEffect;


    private void Update()
    {
        if (dartsThrown >= maxDarts || isThrown) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;

            if (!isThrown && mousePos.y <= Screen.height * 0.2f)
            {
                isThrown = true;
                rb.AddForce(Vector2.up * throwForce);
            }
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 touchPos = Input.GetTouch(0).position;

            if (touchPos.y <= Screen.height * 0.2f)
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
            dartScript otherDart = collision.GetComponent<dartScript>();

            // Destroy only if the other dart is already stuck
            if (otherDart != null && otherDart.isStuck)
            {
                Debug.Log("Hit a stuck dart! Playing effect and destroying this dart.");

                // Spawn VFX at dart position
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, transform.position, Quaternion.identity);
                }

                // Optionally add a tiny delay for better visibility
                Destroy(gameObject, 0.1f);
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

