using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boardRotator : MonoBehaviour
{
    public float rotationSpeed = 100f;
    private bool rotating = false;
    private float targetAngle;
    private float rotatedAngle;
    private int direction;

    void Update()
    {

        if (!rotating)
        {
            StartNewRotation();
        }
        else
        {
            float rotationStep = direction * rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, 0f, rotationStep);
            rotatedAngle += Mathf.Abs(rotationStep);

            if (rotatedAngle >= targetAngle)
            {
                rotating = false;
            }
        }
    }

    void StartNewRotation()
    {
        // Choose a random angle (120, 180, or 360)
        int[] angles = { 120, 180, 90 };
        targetAngle = angles[Random.Range(0, angles.Length)];

        // Randomly choose direction: 1 (clockwise), -1 (anticlockwise)
        direction = Random.value > 0.5f ? 1 : -1;

        rotatedAngle = 0f;
        rotating = true;
    }
}