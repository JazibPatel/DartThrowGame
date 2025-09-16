//using UnityEngine;
//using UnityEngine.UI;

//public class dartSpawner : MonoBehaviour
//{
//    [Header("Dart Prefabs")]
//    public GameObject redDartPrefab;
//    public GameObject blueDartPrefab;

//    [Header("Spawn Points")]
//    public Transform redSpawnPoint;
//    public Transform blueSpawnPoint;

//    [Header("Queue References")]
//    public Transform redQueue;
//    public Transform blueQueue;

//    private int redQueueIndex = 0;
//    private int blueQueueIndex = 0;

//    public void SpawnDart(int playerNumber)
//    {
//        if (scoreManager.instance == null) return;

//        if (playerNumber == 1)
//        {
//            if (redQueueIndex >= scoreManager.instance.maxDarts) return;
//            Instantiate(redDartPrefab, redSpawnPoint.position, redSpawnPoint.rotation);
//        }
//        else
//        {
//            if (blueQueueIndex >= scoreManager.instance.maxDarts) return;
//            Instantiate(blueDartPrefab, blueSpawnPoint.position, blueSpawnPoint.rotation);
//        }
//    }

//    public void UpdateQueue(int playerNumber)
//    {
//        if (playerNumber == 1 && redQueue != null && redQueueIndex < redQueue.childCount)
//        {
//            Image img = redQueue.GetChild(redQueueIndex).GetComponent<Image>();
//            if (img != null) img.color = Color.black;

//            redQueueIndex++;
//            Debug.Log("Red Queue Index = " + redQueueIndex);
//        }
//        else if (playerNumber == 2 && blueQueue != null && blueQueueIndex < blueQueue.childCount)
//        {
//            Image img = blueQueue.GetChild(blueQueueIndex).GetComponent<Image>();
//            if (img != null) img.color = Color.black;

//            blueQueueIndex++;
//            Debug.Log("Blue Queue Index = " + blueQueueIndex);
//        }
//    }
//}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class dartSpawner : MonoBehaviour
{
    [Header("Dart Prefabs")]
    public GameObject redDartPrefab;
    public GameObject blueDartPrefab;

    [Header("Spawn Points")]
    public Transform redSpawnPoint;
    public Transform blueSpawnPoint;

    [Header("Queue References")]
    public Transform redQueue;
    public Transform blueQueue;

    private int redQueueIndex = 0;
    private int blueQueueIndex = 0;

    public void SpawnDart(int playerNumber)
    {
        if (scoreManager.instance == null) return;

        if (playerNumber == 1)
        {
            if (redQueueIndex >= scoreManager.instance.maxDarts) return;
            Instantiate(redDartPrefab, redSpawnPoint.position, redSpawnPoint.rotation);
        }
        else
        {
            if (blueQueueIndex >= scoreManager.instance.maxDarts) return;
            Instantiate(blueDartPrefab, blueSpawnPoint.position, blueSpawnPoint.rotation);
        }
    }

    public void UpdateQueue(int playerNumber)
    {
        if (playerNumber == 1 && redQueue != null && redQueueIndex < redQueue.childCount)
        {
            Image img = redQueue.GetChild(redQueueIndex).GetComponent<Image>();
            if (img != null) img.color = Color.black;

            redQueueIndex++;
            Debug.Log("Red Queue Index = " + redQueueIndex);
            StartCoroutine(SpawnWithDelay(playerNumber, 0.5f)); // Delay to prevent overlap
        }
        else if (playerNumber == 2 && blueQueue != null && blueQueueIndex < blueQueue.childCount)
        {
            Image img = blueQueue.GetChild(blueQueueIndex).GetComponent<Image>();
            if (img != null) img.color = Color.black;

            blueQueueIndex++;
            Debug.Log("Blue Queue Index = " + blueQueueIndex);
            StartCoroutine(SpawnWithDelay(playerNumber, 0.5f)); // Delay to prevent overlap
        }
    }

    private IEnumerator SpawnWithDelay(int playerNumber, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnDart(playerNumber);
    }
}