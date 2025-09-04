using UnityEngine;

public class roadBarrier : MonoBehaviour
{
    public float resetZ = 40f;   // position to reset tree ahead
    public float minZ = -20f;    // when tree goes below this, reset
    public float fixedY = -225.94f;        // locked Y position
    public PlayerManager owner;
    void Start()
    {
        fixedY = transform.position.y; // remember starting height
    }

    void Update()
    {
        // Move only on Z axis
        transform.Translate(Vector3.back * owner.barrierSpeed * Time.deltaTime, Space.World);

        // Force Y
        transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);

        // Debug print
        //Debug.Log(gameObject.name + " Y = " + transform.position.y);

        // Reset on minZ
        if (transform.position.z < minZ)
        {
            transform.position = new Vector3(transform.position.x, fixedY, resetZ);
        }
    }

}
