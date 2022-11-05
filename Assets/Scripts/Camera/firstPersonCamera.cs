using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstPersonCamera : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 2f;
    float cameraVerticleRotation = 0f;

    public int zoom = 5;
    public int normal = 60;
    public float smooth = 5;

    private bool isZoomed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraVerticleRotation -= inputY;
        cameraVerticleRotation = Mathf.Clamp(cameraVerticleRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticleRotation;

        player.Rotate(Vector3.up * inputX);

        if (NpcController.zoomActive == "y")
        {
            isZoomed = !isZoomed;
        }

        if(isZoomed)
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, zoom, Time.deltaTime * smooth);
        } else
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, normal, Time.deltaTime * smooth);
        }

    }
}
