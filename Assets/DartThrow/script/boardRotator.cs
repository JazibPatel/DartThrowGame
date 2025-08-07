//using UnityEngine;

//public class boardRotator : MonoBehaviour
//{
//    public float baseSpeed = 50f;
//    private float rotationSpeed;
//    private int direction = 1; // 1 = CW, -1 = CCW
//    private float rotatedAngle = 0f;

//    void Start()
//    {
//        // Set rotation speed based on difficulty
//        if (SceneLoader.instance != null)
//        {
//            if (SceneLoader.instance.numOfPlayers == 2)
//            {
//                // Duo → Make it fast
//                rotationSpeed = baseSpeed * 4f;
//            }
//            else
//            {
//                switch (SceneLoader.instance.difficulty)
//                {
//                    case "easy": rotationSpeed = baseSpeed * 3f; break;    // 10 deg/sec
//                    case "medium": rotationSpeed = baseSpeed * 4f; break;  // 30 deg/sec
//                    case "hard": rotationSpeed = baseSpeed * 5f; break;    // 50 deg/sec
//                    default: rotationSpeed = baseSpeed * 2f; break;
//                }
//            }
//        }
//        else
//        {
//            rotationSpeed = baseSpeed; // fallback
//        }
//    }

//    void Update()
//    {
//        float step = rotationSpeed * direction * Time.deltaTime;
//        transform.Rotate(0f, 0f, step);
//        rotatedAngle += Mathf.Abs(step);

//        if (rotatedAngle >= 360f)
//        {
//            rotatedAngle = 0f;
//            direction *= -1;
//        }
//    }

//    // ✅ This MUST be public
//    public int GetCurrentTopScore()
//    {
//        ScoreValueScript[] zones = GetComponentsInChildren<ScoreValueScript>();
//        ScoreValueScript topZone = null;
//        float maxDot = -1f;

//        foreach (var zone in zones)
//        {
//            Vector3 toZone = (zone.transform.position - transform.position).normalized;
//            float dot = Vector3.Dot(transform.up, toZone); // checks alignment with 'up'

//            if (dot > maxDot)
//            {
//                maxDot = dot;
//                topZone = zone;
//            }
//        }

//        Debug.Log("Top Score Zone: " + (topZone != null ? topZone.value : 0));
//        Debug.DrawLine(transform.position, transform.position + transform.up * 2f, Color.green); // board up
//        Debug.DrawLine(transform.position, topZone.transform.position, Color.red); // topZone dir

//        return topZone != null ? topZone.value : 0;
//    }
//}

using UnityEngine;

public class boardRotator : MonoBehaviour
{
    public float baseSpeed = 50f;
    private float rotationSpeed;
    private int direction = 1; // 1 = clockwise, -1 = counterclockwise
    private float rotatedAngle = 0f;

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

    // Return the value of the zone currently at the top (up direction)
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

    // Predict which zone will be closest to the board's "up" after {seconds} and how close (angle in degrees)
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

