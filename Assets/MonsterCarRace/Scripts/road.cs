using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class road : MonoBehaviour
{
    public PlayerManager owner;
    // Update is called once per frame
    void Update()
    {

        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material mat = mr.material;

        Vector2 offset = mat.mainTextureOffset;
        offset.y += owner.roadSpeed * Time.deltaTime;
        mat.mainTextureOffset = offset;

    }

}
