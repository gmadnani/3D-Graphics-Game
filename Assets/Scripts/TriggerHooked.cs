using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHooked : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    // only for grappling hook (not gun)
    void OnTriggerEnter(Collider other)
    {
        if (player.GetComponent<GrapplingHook>().fired)
        {
            if (other.tag == "hookable")
            {
                player.GetComponent<GrapplingHook>().hooked = true;
                player.GetComponent<GrapplingHook>().hookedObj = other.gameObject;
            }
            else
            {
                player.GetComponent<GrapplingHook>().ReturnHook();
            }
        }
    }
}
