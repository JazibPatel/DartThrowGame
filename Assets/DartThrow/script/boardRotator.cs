

using UnityEngine;


public class boardRotator : MonoBehaviour
{
    public float baseSpeed = 50f;
    private float rotationSpeed;

    private int direction = 1;
    private float rotatedAngle = 0f;
    private float targetAngle = 0f;

    // Possible rotation amounts
    private readonly float[] rotationOptions = { 60f, 120f, 180f };


    void Start()
    {
        // Set rotation speed based on game mode
        if (SceneLoader.instance != null)
        {
            if (SceneLoader.instance.numOfPlayers == 2)
            {
                rotationSpeed = baseSpeed * 4f;
            }

            else
            {
                switch (SceneLoader.instance.difficulty.ToLower())
                {

                    case "easy":
                        rotationSpeed = baseSpeed * 3f;
                        break;
                    case "medium":
                        rotationSpeed = baseSpeed * 4f;
                        break;
                    case "hard":
                        rotationSpeed = baseSpeed * 5f;
                        break;
                    default:
                        rotationSpeed = baseSpeed * 2f;
                        break;
                }
            }
        }
        else
        {
            rotationSpeed = baseSpeed; // fallback speed
        }

    }

    void Update()
    {
        float step = rotationSpeed * direction * Time.deltaTime;
        transform.Rotate(0f, 0f, step);
        rotatedAngle += Mathf.Abs(step);


        if (rotatedAngle >= 360f)
        {
            rotatedAngle = 0f;
            direction *= -1;
        }
    }

    private void PickNewRotation()
    {
        rotatedAngle = 0f;
        targetAngle = rotationOptions[Random.Range(0, rotationOptions.Length)];
        direction = Random.Range(0, 2) == 0 ? 1 : -1; // Random clockwise or counterclockwise
    }

    public int GetCurrentTopScore()
    {
        ScoreValueScript[] zones = GetComponentsInChildren<ScoreValueScript>();
        ScoreValueScript topZone = null;
        float maxDot = -1f;

        foreach (var zone in zones)
        {
            Vector3 toZone = (zone.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.up, toZone);

            if (dot > maxDot)
            {
                maxDot = dot;
                topZone = zone;
            }
        }

        return topZone != null ? topZone.value : 0;
    }
    public (int value, float angleToTop) ClosestZoneToTopAfter(float seconds)
    {
        float angle = rotationSpeed * direction * seconds;
        Quaternion futureRotation = transform.rotation * Quaternion.Euler(0, 0, angle);

        ScoreValueScript[] zones = GetComponentsInChildren<ScoreValueScript>();

        int bestValue = 0;
        float minAngle = 360f;

        foreach (var zone in zones)
        {
            Vector3 toZone = (zone.transform.position - transform.position).normalized;
            Vector3 futureUp = futureRotation * Vector3.up;

            // Clamp dot product to avoid errors from floating point precision
            float dot = Vector3.Dot(futureUp, toZone);
            dot = Mathf.Clamp(dot, -1f, 1f);

            // Calculate angle in degrees between futureUp and toZone

            float angleBetween = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (angleBetween < minAngle)
            {
                minAngle = angleBetween;
                bestValue = zone.value;
            }
        }

        return (bestValue, minAngle);
    }
}
