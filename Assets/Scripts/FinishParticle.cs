using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishParticle : MonoBehaviour
{
    public ParticleSystem ps1, ps2, ps3, ps4;
    public Transform c1, c2, c3, c4;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ps1.transform.position = c1.position;
            ps1.Play();
            ps2.transform.position = c2.position;
            ps2.Play();
            ps3.transform.position = c3.position;
            ps3.Play();
            ps4.transform.position = c4.position;
            ps4.Play();
        }
    }


}
