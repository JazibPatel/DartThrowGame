using UnityEngine;

public class boardRotator : MonoBehaviour
{
    public float baseSpeed = 120f;
    private float rotationSpeed;
    private int direction = 1; // 1 = CW, -1 = CCW
    private float rotatedAngle = 0f;

    void Start()
    {
        // Set rotation speed based on difficulty
        if (SceneLoader.instance != null)
        {
            if (SceneLoader.instance.numOfPlayers == 2)
            {
                // Duo → Same as Medium
                rotationSpeed = baseSpeed * 1.0f;
            }
            else
            {
                switch (SceneLoader.instance.difficulty)
                {
                    case "Easy": rotationSpeed = baseSpeed * 0.6f; break;
                    case "Medium": rotationSpeed = baseSpeed * 1.0f; break;
                    case "Hard": rotationSpeed = baseSpeed * 1.4f; break;
                    default: rotationSpeed = baseSpeed; break;
                }
            }
        }
        else
        {
            rotationSpeed = baseSpeed; // fallback
        }
    }

    void Update()
    {
        float step = rotationSpeed * direction * Time.deltaTime;
        transform.Rotate(0f, 0f, step);
        rotatedAngle += Mathf.Abs(step);

        // Once we rotate full 360° → reverse direction
        if (rotatedAngle >= 360f)
        {
            rotatedAngle = 0f;
            direction *= -1;
        }
    }

    // Get the score value currently at top (Y-axis)
    public int GetCurrentTopScore()
    {
        ScoreValueScript[] zones = GetComponentsInChildren<ScoreValueScript>();
        ScoreValueScript topZone = null;
        float highestY = float.MinValue;

        foreach (var zone in zones)
        {
            Vector3 worldPos = zone.transform.position;
            if (worldPos.y > highestY)
            {
                highestY = worldPos.y;
                topZone = zone;
            }
        }

        return (topZone != null) ? topZone.value : 0;
    }
}
