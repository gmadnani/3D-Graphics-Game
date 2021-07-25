using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public Transform playerDirection;


    float xRotate = 0f;

    // Start is called before the first frame update
    void Start()
    {   
        //hiding cursor and making it the crosshair
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotate -= mouseY;
        xRotate = Mathf.Clamp(xRotate, -90f, 90f);
        
        //rotate player
        transform.localRotation = Quaternion.Euler(xRotate, 0f, 0f);
        playerDirection.Rotate(Vector3.up * mouseX);
        
    }
}
