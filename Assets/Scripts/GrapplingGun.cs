using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrapplingGun : MonoBehaviour
{
    // for physics of grapple
    private LineRenderer lr;
    public LayerMask grappable;
    public Transform shootOutPosition, cameraTransform, player;
    private Vector3 grapplePos;
    private SpringJoint joints;

    // variables related to the hook
    public GameObject gun;
    public float maxDistance;

    // states
    public bool hooked = false;
    private bool isSpringMode = true;

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

    // misc
    public AudioSource shootingSound;
    public GameObject crosshair;

    // Start is called before the first frame update
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        // change color of crosshair when something is grappable now
        if (!hooked)
        {
            RaycastHit hit;
            Image crossh = crosshair.GetComponent<Image>();
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, maxDistance, grappable))
            {
                crossh.color = Color.green;
            }
            else
            {
                crossh.color = Color.white;
            }
        }

        // shoot grappling gun in swing mode
        if (Input.GetMouseButtonDown(0) && !hooked)
        {
            isSpringMode = true;
            if (!lr.enabled)
            {
                lr.enabled = true;
            }
            StartGrapple();
        }
        // shoot grappling gun in grapple mode
        else if (Input.GetMouseButtonDown(1) && !hooked)
        {
            isSpringMode = false;
            if (!lr.enabled)
            {
                lr.enabled = true;
            }
            StartGrapple();
        }
        // release grappling gun
        else if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && hooked)
        {
            StopGrapple();
            hooked = false;
        }

        // make grapple mode rope never increase in length
        if (!isSpringMode && hooked && joints)
        {
            joints.maxDistance = Vector3.Distance(this.gameObject.transform.position, grapplePos) * maxDistanceSpringGrapple;
        }
    }

    void LateUpdate()
    {
        DrawRope();
    }

    // create the spring joint and line renderer at raycast
    void StartGrapple()
    {
        RaycastHit hit;
        // check if there is a grappable object in range
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, maxDistance, grappable))
        {
            // play shooting sound, create spring joint
            shootingSound.Play();
            hooked = true;
            grapplePos = hit.point;
            joints = player.gameObject.AddComponent<SpringJoint>();
            joints.autoConfigureConnectedAnchor = false;
            joints.connectedAnchor = grapplePos;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePos);
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

            lr.positionCount = 2;
        }
    }

    // update the distance of rope
    public void UpdateJoints()
    {
        // check if gun is currently hooked and change min and max parameter of spring joints
        if (joints && hooked)
        {
            float distanceFromPoint = Vector3.Distance(player.position, grapplePos);
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

    // release the spring joint and line renderer
    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joints);
    }

    // draw the rope / line renderer
    void DrawRope()
    {
        if (!joints)
        {
            return;
        }
        lr.SetPosition(0, shootOutPosition.position);
        lr.SetPosition(1, grapplePos);
    }

    // get current grapple position
    public Vector3 getGrapplePos()
    {
        return grapplePos;
    }
}
