using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject bottom;
    public Rigidbody rigid;
    public ParticleSystem dust;
    public float speed = 5f;
    public float speedWhenFalling = 1f;
    public float maxVelocityChange;

    public float jumpForce = 3f;

    //change this for the diff mode <------------------------------------------------------
    //Steps to change from hook to gun: GrapplingHook to GrapplingGun, Untick and tick hook and gun, move pistol to gh spot in script
    //Steps to change gun to hook: Untick and tick gun and hook, GrapplingGun to GrapplingHook, move player to gh spot in script
    public GrapplingGun gh;
    public RaycastHit hit;

    public float grappleForce;
    public float topSpeed;

    public float distToGround = 0.1f;

    public bool grounded;


    //Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    //ground check
    bool isGround()
    {
        return Physics.SphereCast(bottom.transform.position, bottom.GetComponent<SphereCollider>().radius, Vector3.down, out hit, distToGround);
    }

    //activate particle system
    void CreateDust()
    {
        dust.Play();
    }

    //trigger for particles on landing
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("Ground"))
            CreateDust();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        grounded = isGround();

        //checking if jump is possible
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        Vector3 move = transform.right * x + transform.forward * y;
       
        // movement for player when it is grounded or not hooked
        if (!gh.hooked || grounded) 
        {
            // Understanding movement by using add force velocity change from: https://answers.unity.com/questions/1515686/calculate-player-velocity-change-while-taking-into.html
            // Add velocity change for movement on the local horizontal plane
            Vector3 targetVelocity = (move) * speed;
            Vector3 localVelocity = transform.InverseTransformDirection(rigid.velocity);
            Vector3 velocityChange = transform.InverseTransformDirection(targetVelocity) - localVelocity;

            // The velocity change is clamped to the control velocity
            // Move with a value of "speed" when grounded, but if the player is in the air and is not hooked, reduce amount of movement
            velocityChange = Vector3.ClampMagnitude(velocityChange, grounded ? speed : 1f);
            velocityChange.y = 0;
            velocityChange = transform.TransformDirection(velocityChange);
            rigid.AddForce(velocityChange, ForceMode.VelocityChange);

            gh.UpdateJoints();
        }
        // add movement force when the player is hooked
        else
        {
            rigid.AddForce(move * grappleForce);
        }

        //cap max speed when grappling
        if (rigid.velocity.magnitude > topSpeed && gh.hooked)
        {
            rigid.velocity = rigid.velocity.normalized * topSpeed;
        }

    }
}