using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    // for physics of grapple
    private LineRenderer lr;
    public Transform shootOutPosition, cameraTransform, player;
    private SpringJoint joints;
    [SerializeField] private Vector3 shootPoint;

    // variables related to the hook
    public GameObject hook;
    public GameObject hookHolder;
    public GameObject hookedObj;
    public Transform hookEnd;
    public float hookSpeed;
    public float maxDistance;

    // states
    public bool fired = false;
    public bool hooked = false;

    // physics parameters swing hook
    public float springSwing = 5;
    public float damperSwing = 7;
    public float massScaleSwing = 5;
    public float maxDistanceSpringSwing = 1;
    public float minDistanceSpringSwing = 1;

    // physics parameters grappling hook
    public float springGrapple = 15;
    public float damperGrapple = 1;
    public float massScaleGrapple = 5;
    public float maxDistanceSpringGrapple = 0.7f;
    public float minDistanceSpringGrapple = 0.1f;

    private bool isSpringMode = true;

    // Start is called before the first frame update
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // release hook
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && hooked)
        {
            StopGrapple();
            ReturnHook();
        }
        // shoot hook
        else if (Input.GetMouseButtonDown(0) && !fired)
        {
            fired = true;
            shootPoint = cameraTransform.forward;
            lr.enabled = true;
            isSpringMode = true;
        }
        else if (Input.GetMouseButtonDown(1) && !fired)
        {
            fired = true;
            shootPoint = cameraTransform.forward;
            lr.enabled = true;
            isSpringMode = false;
        }

        // hook moves towards the direction of player
        if (fired && !hooked)
        {
            Vector3 targetPosition = hook.transform.position + shootPoint * maxDistance;
            hook.transform.position = Vector3.MoveTowards(hook.transform.position, targetPosition, maxDistance * Time.deltaTime);

            float currDistance = Vector3.Distance(hook.transform.position, shootOutPosition.position);

            if (currDistance >= maxDistance)
            {
                ReturnHook();
            }
        }

        // start grapple when hooked
        if (hooked)
        {
            StartGrapple();
        }
    }

    void LateUpdate()
    {
        DrawRope();
    }

    // return the hook to player
    public void ReturnHook()
    {
        hook.transform.parent = hookHolder.transform;
        hook.transform.rotation = hookHolder.transform.rotation;
        hook.transform.position = hookHolder.transform.position;
        fired = false;
        hooked = false;
        lr.enabled = false;
    }

    // start spring joint
    void StartGrapple()
    {
        float distanceFromPoint = Vector3.Distance(player.position, hookEnd.position);
        if (!joints)
        {
            hook.transform.parent = hookedObj.transform;
            joints = player.gameObject.AddComponent<SpringJoint>();
            joints.autoConfigureConnectedAnchor = false;
            joints.connectedAnchor = hookEnd.position;
            // change parameter depending on mode
            if (isSpringMode)
            {
                joints.spring = springSwing;
                joints.damper = damperSwing;
                joints.massScale = massScaleSwing;

                joints.maxDistance = distanceFromPoint * maxDistanceSpringSwing;
                joints.minDistance = distanceFromPoint * minDistanceSpringSwing;
            }
            else
            {
                joints.spring = springGrapple;
                joints.damper = damperGrapple;
                joints.massScale = massScaleGrapple;

                joints.maxDistance = distanceFromPoint * maxDistanceSpringGrapple;
                joints.minDistance = distanceFromPoint * minDistanceSpringGrapple;
            }
        }

        // never make the rope longer if grapple mode
        if (!isSpringMode)
        {
            joints.maxDistance = distanceFromPoint * maxDistanceSpringGrapple;
        }
    }

    // update rope length
    public void UpdateJoints()
    {
        // when grappled and on ground
        if (joints)
        {
            float distanceFromPoint = Vector3.Distance(player.position, hookEnd.position);
            if (isSpringMode)
            {
                joints.maxDistance = distanceFromPoint * maxDistanceSpringSwing;
                joints.minDistance = distanceFromPoint * minDistanceSpringSwing;
            }
            else
            {
                joints.maxDistance = distanceFromPoint * maxDistanceSpringGrapple;
                joints.minDistance = distanceFromPoint * minDistanceSpringGrapple;
            }
        }
    }

    // remove spring joint
    void StopGrapple()
    {
        Destroy(joints);
    }

    void DrawRope()
    {
        if (!fired)
        {
            return;
        }
        lr.SetPosition(0, shootOutPosition.position);
        lr.SetPosition(1, hookEnd.position);
        lr.enabled = true;
    }

    // check if hook collided with something hookable
    void OnTriggerEnter(Collider other)
    {
        if (fired)
        {
            if (other.tag == "hookable")
            {
                hooked = true;
                hookedObj = other.gameObject;
            }
            else
            {
                ReturnHook();
            }
        }
    }
}
