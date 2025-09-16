using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demo : MonoBehaviour
{

    public float force = 70f; // force applied to the ball
    private Rigidbody rb;
    private bool thrown = false;
    public float angle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // On click/tap throw once
        if (!thrown && Input.GetMouseButtonDown(0))
        {
            ThrowBall();
        }
    }

    void ThrowBall()
    {
        thrown = true;

        // Use the object's current Y rotation
        Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }
}
