using UnityEngine;

public class MoveByKeyboard : MonoBehaviour
{
    private Rigidbody rb;

    [Space(1)]
    [Range(0f,15f)]
    [Tooltip("Slide to choose player speed")]
    [Header(" Movement speed of player")]
    [SerializeField] private float speed;

    public ForceMode fM;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 velocity = new Vector3(h,0,v);
       // velocity = transform.TransformDirection(velocity);  
        velocity *= speed;
        velocity -= rb.velocity;

        rb.AddForce(velocity,fM);
        
    }


}
