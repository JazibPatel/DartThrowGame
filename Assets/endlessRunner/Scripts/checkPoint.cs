using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class checkPoint : MonoBehaviour
{
    public float resetZ = 40f;   // position to reset ahead
    public float minZ = -5f;    // reset trigger point
    public int checkpoint = 6;

    public TextMeshPro c;
    //public TextMeshPro c2;
    public TextMeshPro finishLineText;

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
        c.text = "Check Point - " +  checkpoint.ToString();

        //Debug.Log("checkpoint : " + checkpoint);

        if(checkpoint < 1)
        {
            c.gameObject.SetActive(false);

            if (finishLineText != null)
                finishLineText.gameObject.SetActive(true);

            // Start coroutine to handle finish sequence
            StartCoroutine(FinishSequence());
        }

        //if (checkpoint < 0) {

        //   // Debug.Log("Winner : " + owner.name);
        //    result.instance.CheckWinner();
        //}

        isWaiting = false; // allow movement again
    }

    IEnumerator FinishSequence()
    {
        // Wait 2 seconds
        yield return new WaitForSeconds(2f);

        // Now check winner
        result.instance.CheckWinner();
    }

}
