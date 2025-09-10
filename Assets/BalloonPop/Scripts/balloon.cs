using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balloon : MonoBehaviour
{
    public AudioClip balloonPopSound;
    public GameObject splashPrefab;

    public void pop()
    {

        AudioSource.PlayClipAtPoint(balloonPopSound, transform.position);

        if (splashPrefab != null)
        {
            GameObject splash = Instantiate(splashPrefab, transform.position, Quaternion.identity);
            Destroy(splash, 0.5f);
        }

        Destroy(gameObject);
    }
}
