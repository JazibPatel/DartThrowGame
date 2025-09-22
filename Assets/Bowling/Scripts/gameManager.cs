using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager Instance;

    [Header("Prefabs")]
    public GameObject ballPrefab;
    public GameObject pinPrefab;

    [Header("Spawn Points")]
    public Transform ballSpawn;
    public Transform[] pinSpawns; // 10 pin spawn points

    [Header("Obstacles")]
    public GameObject[] obstacles; // Assign 5 obstacles in Inspector

    //[Header("Background")]
    //public Transform backgroundTransform;
    //private bool backgroundFlipped = false;

    private int currentPlayer = 1;
    private GameObject currentBall;
    private GameObject[] currentPins;

    public Material player1BallMaterial;
    public Material player2BallMaterial;

    private Camera camMain;
    private bool cameraFlip;
    public int cameraFlipCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        camMain = Camera.main;
        SpawnBallAndPins();
        UpdateObstacles(); // initialize
    }

    public int GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void EndTurn()
    {
        StartCoroutine(SwitchTurn());
    }

    private IEnumerator SwitchTurn()
    {
        yield return new WaitForSeconds(2f);

        if (!cameraFlip)
        {
            camMain.transform.DORotateQuaternion(Quaternion.Euler(77.237f, 0, 180f), 1.0f);
        }
        else
        {
            camMain.transform.DORotateQuaternion(Quaternion.Euler(77.237f, 0, 0), 1.0f);
        }

        cameraFlip = !cameraFlip;
        cameraFlipCount++;
        Debug.Log("Count : " + cameraFlipCount);
        bowlingScoreManager.Instance.checkWinner();

        //// Flip background scale
        //Vector3 scale = backgroundTransform.localScale;
        //scale.x *= -1;
        //scale.y *= -1;
        //backgroundTransform.localScale = scale;

        //// Manually adjust background Y position
        //Vector3 pos = backgroundTransform.localPosition;
        //pos.y = backgroundFlipped ? 177f : -177f;
        //backgroundTransform.localPosition = pos;

        //backgroundFlipped = !backgroundFlipped;

        // Destroy old ball & pins
        if (currentBall)
            Destroy(currentBall);
        if (currentPins != null)
        {
            foreach (var pin in currentPins)
                if (pin)
                    Destroy(pin);
        }

        // Switch player
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
        Debug.Log("Now Player " + currentPlayer + "'s turn!");

        // Spawn new ball & pins
        SpawnBallAndPins();

        // ✅ Update obstacles after each camera flip
        UpdateObstacles();
    }

    private void SpawnBallAndPins()
    {
        // Spawn Ball
        currentBall = Instantiate(ballPrefab, ballSpawn.position, ballSpawn.rotation);

        // ✅ Change ball material based on player
        Renderer ballRenderer = currentBall.GetComponent<Renderer>();
        if (ballRenderer != null)
        {
            if (currentPlayer == 1)
                ballRenderer.material = player1BallMaterial;
            else
                ballRenderer.material = player2BallMaterial;
        }

        // Spawn Pins
        currentPins = new GameObject[pinSpawns.Length];
        for (int i = 0; i < pinSpawns.Length; i++)
        {
            currentPins[i] = Instantiate(pinPrefab, pinSpawns[i].position, pinSpawns[i].rotation);
        }
    }

    private void UpdateObstacles()
    {
        foreach (var obs in obstacles)
        {
            if (obs)
                obs.SetActive(false);
        }

        // Determine which obstacle to show
        int index = cameraFlipCount / 2; // 0-1 -> 0, 2-3 -> 1, etc.

        if (index >= 0 && index < obstacles.Length)
        {
            obstacles[index].SetActive(true);
        }
    }
}
