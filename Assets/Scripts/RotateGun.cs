using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public GrapplingGun gh;

    private Quaternion rotation;
    private float rotationSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        // grappling gun looks at grapple location if hooked
        if (!gh.hooked)
        {
            rotation = transform.parent.rotation;
        }
        else
        {
            rotation = Quaternion.LookRotation(gh.getGrapplePos() - transform.position);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
}
