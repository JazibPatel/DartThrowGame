using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject dartPrefab;
    public Transform spawnPoint;

    private int currentDartCount = 0;
    public int maxDarts = 5;

    private GameObject currentDart;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        SpawnNextDart();
        Debug.Log("Dart spawned at: " + spawnPoint.position);

    }

    public void SpawnNextDart()
    {
        if (currentDartCount >= maxDarts)
        {
            Debug.Log("All darts thrown!");
            return;
        }

        currentDart = Instantiate(dartPrefab, spawnPoint.position, Quaternion.identity);
        currentDartCount++;
    }
}
