using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerapan : MonoBehaviour
{
    public float speed;
    public float maxRotation;
    public float minRotation;
    private float dir = 1;
    
    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 1f;
        Vector3 rotation = transform.localEulerAngles;

        if (rotation.y <= minRotation)
        {
            dir = 1;
        }
        else if (rotation.y >= maxRotation)
        {
            dir = -1;
        }
        rotation.y += dir * speed * Time.deltaTime;
        
        transform.localEulerAngles = rotation;
    }
}