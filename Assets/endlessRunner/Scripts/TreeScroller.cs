using UnityEngine;

public class TreeScroller : MonoBehaviour
{
    public float resetZ = 40f;   // position to reset tree ahead
    public float minZ = -20f;    // when tree goes below this, reset
    public PlayerManager owner;

    void Update()
    {
        // Move only on Z axis
        transform.Translate(Vector3.back * owner.treeSpeed * Time.deltaTime, Space.World);

        // Reset on minZ
        if (transform.position.z < minZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, resetZ);
        }
    }

}
