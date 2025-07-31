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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dartRedManagerScript : MonoBehaviour
{
    public Transform dartHolder;     // Where the thrown dart will be parented
    public Transform dartQueue;      // Parent of 5 dart icons
    public List<SpriteRenderer> dartIcons = new List<SpriteRenderer>();

    private int currentDartIndex = 0;

    void Start()
    {
        // Fill dartIcons list automatically from dartQueue children
        foreach (Transform child in dartQueue)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                dartIcons.Add(sr);
            }
        }

        // Activate the first dart
        currentDart();
    }

    void Update()
    {
        // Left mouse click to throw
        if (Input.GetMouseButtonDown(0))
        {
            if (currentDartIndex < dartIcons.Count)
            {
                dartRedHolder();
            }
        }
    }

    public void currentDart()
    {
        // Enable the first child (ready to throw)
        if (transform.childCount > 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void dartRedHolder()
    {
        // Move the thrown dart to holder
        Transform dart = transform.GetChild(0);
        dart.SetParent(dartHolder);

        // Change the corresponding queue dart color to black
        if (currentDartIndex < dartIcons.Count)
        {
            dartIcons[currentDartIndex].color = Color.black;
        }

        // Move to next dart
        currentDartIndex++;

        // Wait and then get the next dart (if any left)
        StartCoroutine(getNextDart());
    }

    IEnumerator getNextDart()
    {
        yield return new WaitForSeconds(1f);

        if (currentDartIndex < dartIcons.Count)
        {
            currentDart();
        }
        else
        {
            Debug.Log("All darts used!");
        }
    }
}

