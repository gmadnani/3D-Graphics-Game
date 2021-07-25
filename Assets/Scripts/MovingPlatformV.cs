using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformV : MonoBehaviour
{
    public float speed = 5;
    private bool swit = true;
    private float ypos;
    // Update is called once per frame
    private void Start()
    {
        ypos = this.transform.position.y;
    }
    void Update()
    {

        if (this.transform.position.y > (ypos + 6))
        {
            swit = false;
        }
        if (this.transform.position.y < (ypos - 6))
        {
            swit = true;
        }
        if (swit)
        {
            this.transform.localPosition += Vector3.up * Time.deltaTime * speed;
        }
        else
        {
            this.transform.localPosition -= Vector3.up * Time.deltaTime * speed;
        }
    }
}
