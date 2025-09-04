using System.Collections;
using UnityEngine;
using TMPro;

public class checkPoint : MonoBehaviour
{
    public float resetZ = 40f;   // position to reset ahead
    public float minZ = -20f;    // reset trigger point
    public int checkpoint = 6;

    public TextMeshPro c;
    public TextMeshPro c2;

    private bool isWaiting = false;

    public PlayerManager owner;

    void Update()
    {
        // Move only if not waiting
        if (!isWaiting)
        {
            transform.Translate(Vector3.back * owner.checkpointSpeed * Time.deltaTime, Space.World);

            // If it goes behind camera
            if (transform.position.z < minZ)
            {
                StartCoroutine(ResetAfterDelay());
            }
        }
    }

    IEnumerator ResetAfterDelay()
    {
        isWaiting = true; // stop movement

        yield return new WaitForSeconds(30f); // wait 30 sec

        // Reset position and score
        transform.position = new Vector3(transform.position.x, transform.position.y, resetZ);
    
        checkpoint--;
        c.text = checkpoint.ToString();
        c2.text = checkpoint.ToString();

        Debug.Log("checkpoint : " + checkpoint);

        isWaiting = false; // allow movement again
    }
}
