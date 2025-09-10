using UnityEngine;

public class dart : MonoBehaviour
{
    // Start is called before the first frame update
    public static float forceFactor = 10.0f;
    public float factor = 100.0f;
    Vector3 initPos;
    float startTime;
    Vector3 startPos;
    public Rigidbody rigidbody;
    public bool throwAllowed;
    public bool ballThrown;
    public AudioClip dartSound;
    private AudioSource audioSource;
    private dart_spawner spawner;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        throwAllowed = true;
        ballThrown = false;
        rigidbody = GetComponent<Rigidbody>();
        initPos = transform.localPosition;

        spawner = FindAnyObjectByType<dart_spawner>();
    }
    void OnMouseDown()
    {
        ballThrown = false;
        startTime = Time.time;
        startPos = Input.mousePosition;
        startPos.z = transform.position.z - Camera.main.transform.position.z;
        startPos = Camera.main.ScreenToWorldPoint(startPos);
        

    }
    //void OnMouseUp()
    //{
    //    if (!throwAllowed)
    //        return;
    //    throwAllowed = false;
    //    var endPos = Input.mousePosition;
    //    endPos.z = transform.position.z - Camera.main.transform.position.z;
    //    endPos = Camera.main.ScreenToWorldPoint(endPos);
    //    var force = endPos - startPos;
    //    force.z = force.magnitude * forceFactor;
    //    force /= (Time.time - startTime);
    //    rigidbody.AddForce(force * factor);
    //    Invoke("ballIsmoving", 2.0f);
    //}
    void OnMouseUp()
    {
        if (!throwAllowed)
            return;

        throwAllowed = false;

        //rigidbody.constraints = RigidbodyConstraints.None;
        //rigidbody.isKinematic = false;
        //rigidbody.useGravity = true;

        var endPos = Input.mousePosition;
        endPos.z = transform.position.z - Camera.main.transform.position.z;
        endPos = Camera.main.ScreenToWorldPoint(endPos);

        // Raw drag direction (x,y)
        var force = endPos - startPos;

        // Scale by swipe speed
        force /= (Time.time - startTime);

        // Always push forward in Z (since camera is behind dart)
        force.z = Mathf.Abs(force.magnitude) * 1.2f;  // 1.2f = forward boost

        // Add a little arc
        force.y += Mathf.Abs(force.magnitude) * 0.2f;  // 0.2f tweak for arc height

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.PlayOneShot(dartSound);


        rigidbody.AddForce(force * factor);
        
        Invoke("ballIsmoving", 2.0f);
    }


    //private void Update()
    //{
    //    // If dart is moving, rotate nose towards velocity
    //    if (rigidbody != null && rigidbody.velocity.magnitude > 0.1f)
    //    {
    //        // Calculate angle in 3D
    //        Quaternion rotation = Quaternion.LookRotation(rigidbody.velocity, Vector3.up);
    //        transform.rotation = rotation;
    //    }
    //}

    public void ballIsmoving()
    {
        ballThrown = true;
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("dartHolder"))
        {
            return;
        }

        if (collision.gameObject.CompareTag("balloon"))
        {
            collision.gameObject.GetComponent<balloon>().pop();
        }

        if (spawner != null)
            spawner.spawnDart(2f);

        Destroy(gameObject);
    }

    void ReturnBall()
    {
        throwAllowed = true;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        transform.localPosition = initPos;
    }
}

