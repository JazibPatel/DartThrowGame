using UnityEngine;

public class roadBarrier : MonoBehaviour
{
    public float resetZ = 40f;   // position to reset tree ahead
    public float minZ = -20f;    // when tree goes below this, reset
    public float fixedY;         // locked Y position
    public PlayerManager owner;

    private float[] lanes;       // dynamic based on difficulty
    private float minSpacing;    // z offset min
    private float maxSpacing;    // z offset max

    void Start()
    {
        fixedY = transform.position.y; // remember starting height
        ApplyDifficulty();
    }

    void Update()
    {
        // Move only on Z axis
        transform.Translate(Vector3.back * owner.barrierSpeed * Time.deltaTime, Space.World);

        // Reset on minZ
        if (transform.position.z < minZ)
        {
            float randomX = lanes[Random.Range(0, lanes.Length)];
            float randomZOffset = Random.Range(minSpacing, maxSpacing);
            transform.position = new Vector3(randomX, fixedY, resetZ + randomZOffset);
        }
    }

    void ApplyDifficulty()
    {
        // Get difficulty from SceneLoader
        string diff = SceneLoader.instance.difficulty.ToLower();

        if (diff == "easy")
        {
            if (Random.value > 0.5f)
                lanes = new float[] { -1f, 0f };  // left + middle
            else
                lanes = new float[] { 0f, 1f };   // middle + right

            minSpacing = 6f;
            maxSpacing = 10f;
        }
        else if (diff == "medium")
        {
            lanes = new float[] { -1f, 0f, 1f }; // 3 lanes
            minSpacing = 3f;
            maxSpacing = 7f;
            resetZ = 30f;
        }
        else if (diff == "hard")
        {
            lanes = new float[] { -1f, 0f, 1f }; // still 3 lanes
            minSpacing = 0.8f;
            maxSpacing = 2f;
            resetZ = 15f;
        }
        else
        {
            lanes = new float[] { -1f, 0f, 1f }; // still 3 lanes
            minSpacing = 0.8f;
            maxSpacing = 2f;
            resetZ = 30f;
        }
    }
}
