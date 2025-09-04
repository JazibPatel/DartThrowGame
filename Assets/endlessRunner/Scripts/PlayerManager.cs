//using System;
//using System.Collections;
//using UnityEngine;

//public class gameManager : MonoBehaviour
//{
//    public static gameManager Instance;

//    [Header("Speeds")]
//    //public float obstacleSpeed = 5f;
//    //public float increaseSpawn = 2f;
//    public float roadSpeed = 0.2f;
//    public float treespeed = 2f;
//    public float barrierSpeed = 2f;
//    public float checkPointSpeed = 2f;

//    //private float defaultObstacleSpeed;
//    //private float defaultObstacleSpaw;
//    private float defaultRoadSpeed;
//    private float defaultTreeSpeed;
//    private float defaultBarrierSpeed;
//    private float defaultCheckPointSpeed;

//    [Header("Difficulty Settings")]
//    //public float obstacleAcceleration = 0.1f;          
//    //public float increaseSpawnAcceleration = 1f;
//    public float roadAcceleration = 0.01f;     
//    public float increaseTreeSpeed = 1f;
//    public float increaseBarrierSpeed = 1f;
//    public float increasCheckPointSpeed = 1f;

//    void Awake()
//    {
//        Instance = this;
//    }

//    void Start()
//    {
//        //defaultObstacleSpeed = obstacleSpeed;
//        //defaultObstacleSpaw = increaseSpawn;
//        defaultRoadSpeed = roadSpeed;
//        defaultTreeSpeed = increaseTreeSpeed;
//        defaultBarrierSpeed = increaseBarrierSpeed;
//        defaultCheckPointSpeed = increasCheckPointSpeed;
//    }

//    public void StopGameForSeconds(float seconds)
//    {
//        StartCoroutine(StopRoutine(seconds));
//        //StartCoroutine(IncreaseDifficultyOverTime());
//    }

//    private void Update()
//    {
//        //obstacleSpeed += obstacleAcceleration * Time.deltaTime;
//        //increaseSpawn -= increaseSpawnAcceleration * Time.deltaTime;
//        //increaseSpawn = Mathf.Max(0.3f, increaseSpawn); // minimum 0.3s delay
//        roadSpeed += roadAcceleration * Time.deltaTime;
//        roadSpeed = Mathf.Clamp(roadSpeed, 0.10f, 0.70f);
//        treespeed += increaseTreeSpeed * Time.deltaTime;
//        treespeed = Mathf.Clamp(treespeed, 2f, 70f);
//        barrierSpeed += barrierSpeed * Time.deltaTime;
//        checkPointSpeed += increasCheckPointSpeed * Time.deltaTime;
//        checkPointSpeed = Mathf.Clamp(checkPointSpeed, 2f, 70f);

//    }
//    IEnumerator StopRoutine(float seconds)
//    {
//        //obstacleSpeed = 0f;
//        //increaseSpawn = 0f;
//        roadSpeed = 0f;
//        treespeed = 0f;
//        barrierSpeed = 0f;
//        checkPointSpeed = 0f;

//        //FindObjectOfType<obstacleSpawn>().PauseSpawning();

//        yield return new WaitForSeconds(seconds);

//        //obstacleSpeed = defaultObstacleSpeed;
//        //increaseSpawn = defaultObstacleSpaw;
//        roadSpeed = defaultRoadSpeed;
//        treespeed *= defaultTreeSpeed;
//        barrierSpeed *= defaultBarrierSpeed;
//        checkPointSpeed *= defaultCheckPointSpeed;

//        //FindObjectOfType<obstacleSpawn>().ResumeSpawning();
//    }
//}


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

        roadSpeed = treeSpeed = barrierSpeed = checkpointSpeed = 0f;

        yield return new WaitForSeconds(seconds);

        isStopped = false;

        roadSpeed = oldRoad;
        treeSpeed = oldTree;
        barrierSpeed = oldBarrier;
        checkpointSpeed = oldCheckpoint;
    }
}
