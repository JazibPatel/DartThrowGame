using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dart_spawner : MonoBehaviour
{
    public GameObject dartPerfab;
    public Transform spawnPoint;

    public void spawnDart(float delay = 0f)
    {

        Invoke(nameof(spawnNow), delay);

    }

   private void spawnNow()
    {
        Instantiate(dartPerfab, spawnPoint.position, spawnPoint.rotation);
    }
}
