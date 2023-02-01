//Code created with guidance from a youtube tutorial. URL: https://youtu.be/f473C43s8nE
//Code sourced from DaveGameDevelopment.
//Comments and minor edits done by Michael Kerr.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamWorld : MonoBehaviour
{
    // Mouse Sensitivity
    public float sensitivityX;
    public float sensitivityY;

    // Actual Camera Transform
    public Transform cameraOrientation;

    // Current Rotation
    private float _xRotation;
    private float _yRotation;


    // Start is called before the first frame update
    void Start()
    {
        //lock + hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        //need mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        //handle Rotation along Y axis
        _yRotation += mouseX;
        
        //handle Rotation along X axis
        _xRotation -= mouseY;

        //ensure rotation along X axis does not go over 90 degrees
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        //Rotate Camera and Player
        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        cameraOrientation.rotation = Quaternion.Euler(0, _yRotation, 0);


    }
}
