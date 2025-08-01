//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class dartRedManagerScript : MonoBehaviour
//{
//    public Transform dartHolder;
//    public SpriteRenderer dartOut;

//    // Start is called before the first frame update
//    void Start()
//    {
//        currentDart();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            dartRedHolder();
//        }
//    }

//    public void currentDart()
//    {
//        transform.GetChild(0).gameObject.SetActive(true);
//    }

//    public void dartRedHolder()
//    {
//        transform.GetChild(0).gameObject.transform.SetParent(dartHolder.transform);
//        dartOut.color = Color.black;
//        StartCoroutine(getNextDart());
//    }

//    IEnumerator getNextDart()
//    {
//        yield return new WaitForSeconds(1f);
//        currentDart();
//    }
//}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dartRedManagerScript : MonoBehaviour
{
    public Transform dartHolder;
    public Transform dartQueue; // Parent of queue darts
    private int currentDartIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentDart();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;

            // Only allow throw if clicked in bottom 20% of the screen
            if (mousePos.y <= Screen.height * 0.2f)
            {
                dartRedHolder();
            }
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 touchPos = Input.GetTouch(0).position;

            if (touchPos.y <= Screen.height * 0.2f)
            {
                dartRedHolder();
            }
        }
    }

    public void currentDart()
    {
        // Only activate next dart if there is at least 1 child
        if (transform.childCount > 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("No darts left!");
        }
    }

    public void dartRedHolder()
    {
        if (transform.childCount == 0 || currentDartIndex >= dartQueue.childCount)
            return;

        transform.GetChild(0).gameObject.transform.SetParent(dartHolder.transform);

        // Change color of the first dart in the queue to black
        if (currentDartIndex < dartQueue.childCount)
        {
            SpriteRenderer sr = dartQueue.GetChild(currentDartIndex).GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = Color.black;
            }
            currentDartIndex++;
        }

        StartCoroutine(getNextDart());
    }

    IEnumerator getNextDart()
    {
        yield return new WaitForSeconds(1f);
        currentDart();
    }
}
