using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    [Header("Speeds")]
    public float roadSpeed = 0.2f;
    public float treeSpeed = 2f;
    public float barrierSpeed = 2f;
    public float checkpointSpeed = 2f;

    [Header("Acceleration")]
    public float roadAcceleration = 0.01f;
    public float treeAcceleration = 0.5f;
    public float barrierAcceleration = 0.5f;
    public float checkpointAcceleration = 0.5f;

    [Header("Limits")]
    public float maxRoadSpeed = 0.7f;
    public float maxTreeSpeed = 70f;
    public float maxBarrierSpeed = 70f;
    public float maxCheckpointSpeed = 70f;

    private bool isStopped = false;

    void Update()
    {
        if (isStopped) return;

        // Road
        roadSpeed += roadAcceleration * Time.deltaTime;
        roadSpeed = Mathf.Clamp(roadSpeed, 0.1f, maxRoadSpeed);

        // Tree
        treeSpeed += treeAcceleration * Time.deltaTime;
        treeSpeed = Mathf.Clamp(treeSpeed, 2f, maxTreeSpeed);

        // Barrier
        barrierSpeed += barrierAcceleration * Time.deltaTime;
        barrierSpeed = Mathf.Clamp(barrierSpeed, 2f, maxBarrierSpeed);

        // Checkpoint
        checkpointSpeed += checkpointAcceleration * Time.deltaTime;
        checkpointSpeed = Mathf.Clamp(checkpointSpeed, 2f, maxCheckpointSpeed);
    }

    public void StopForSeconds(float seconds)
    {
        StartCoroutine(StopRoutine(seconds));
    }

    private IEnumerator StopRoutine(float seconds)
    {
        isStopped = true;

        float oldRoad = roadSpeed;
        float oldTree = treeSpeed;
        float oldBarrier = barrierSpeed;
        float oldCheckpoint = checkpointSpeed;

        // Freeze everything except barriers (make them very slow so truck can pass)
        roadSpeed = 0f;
        treeSpeed = 0f;
        checkpointSpeed = 0f;

        // 🔹 Keep barriers moving slowly backward
        barrierSpeed = -Mathf.Abs(oldBarrier * 0.5f);  // 20% of normal speed

        yield return new WaitForSeconds(seconds);

        isStopped = false;

        roadSpeed = oldRoad;
        treeSpeed = oldTree;
        barrierSpeed = oldBarrier;
        checkpointSpeed = oldCheckpoint;
    }
}
