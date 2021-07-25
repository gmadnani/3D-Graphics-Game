using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameObject previousCheckpoint;
    public GameObject checkpoint;
    public float resetHeight;
    public GameObject deathCounter;

    // Update is called once per frame
    void Update()
    {
        // If the player falls out of the map, respawn the player at the current
        // checkpoint's spawn location
        if (this.transform.position.y <= resetHeight)
        {
            Transform spawnLoc = checkpoint.transform.Find("SpawningPosition").transform;
            this.transform.position = spawnLoc.position;
            this.transform.rotation = spawnLoc.rotation;
            DeathCounter script = deathCounter.GetComponent<DeathCounter>();
            script.totalDeaths += 1;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "checkpoint")
        {
            // If the current check point has not been assign, set them. 
            // Used at the start of the game, where the player has not
            // reached a checkpoint
            if (checkpoint == null)
            {
                checkpoint = other.gameObject;

                // Light up the campfire and play the fire particle system
                checkpoint.GetComponentInChildren<ParticleSystem>().Play();
                checkpoint.GetComponentInChildren<Light>().enabled = true;

                previousCheckpoint = checkpoint;
            }
            // If the current campfire collider that the player is in isn't the same as the
            // previous campfire, set the new campfire to the current campfire and disable
            // the previous campfire
            else if (other.gameObject.GetInstanceID() != previousCheckpoint.GetInstanceID())
            {
                checkpoint = other.gameObject;

                checkpoint.GetComponentInChildren<ParticleSystem>().Play();
                checkpoint.GetComponentInChildren<Light>().enabled = true;

                previousCheckpoint.GetComponentInChildren<ParticleSystem>().Stop();
                previousCheckpoint.GetComponentInChildren<Light>().enabled = false;

                previousCheckpoint = checkpoint;
            }
        }
    }
}
