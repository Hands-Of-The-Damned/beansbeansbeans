using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera main;
    [SerializeField] CinemachineVirtualCamera left;
    [SerializeField] CinemachineVirtualCamera right;
    [SerializeField] CinemachineVirtualCamera down;
    [SerializeField] CinemachineVirtualCamera NpcCamera;
    [SerializeField] CinemachineVirtualCamera dealerCamera;

    Ray ray;
    RaycastHit hit;

    private bool dealerCameraActive;

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Setting up ray cast to be able to point to any object in the world.
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.name); //running debug to see all object that the mouse hovers over

            //Setting up zoom on different objects in the world
            if(hit.collider.name == "Dealer" && Input.GetMouseButtonDown(0))
            {
                //dealerCameraActive = true;
                SwitchPriority("dealer");
            }
            else if(hit.collider.name == "Dealer" && Input.GetMouseButtonDown(1))
            {
                //dealerCameraActive = false;
                SwitchPriority("m");
            }

            if(hit.collider.name == "NpcSprite" && Input.GetMouseButtonDown(0))
            {
                SwitchPriority("npc");
            }
            else if(hit.collider.name == "NpcSprite" && Input.GetMouseButtonDown(1))
            {
                SwitchPriority("l");
            }
        }
    }

    private void OnMouseOver()
    {
        
    }

    //This function was made to be able to switch the priorities of the camera to be able to switch the active camera.
    public void SwitchPriority(string cameraToChange)
    {
        if(cameraToChange == "l")
        {
            main.Priority = 0;
            right.Priority = 0;
            down.Priority = 0;
            dealerCamera.Priority = 0;
            NpcCamera.Priority = 0;
            left.Priority = 1;
        }
        else if(cameraToChange == "r")
        {
            main.Priority = 0;
            left.Priority = 0;
            down.Priority = 0;
            dealerCamera.Priority = 0;
            NpcCamera.Priority = 0;
            right.Priority = 1;
        }
        else if(cameraToChange == "d")
        {
            main.Priority = 0;
            left.Priority = 0;
            right.Priority = 0;
            dealerCamera.Priority = 0;
            NpcCamera.Priority = 0;
            down.Priority = 1;
        }
        else if(cameraToChange == "m")
        {
            left.Priority = 0;
            right.Priority = 0;
            down.Priority = 0;
            dealerCamera.Priority = 0;
            NpcCamera.Priority = 0;
            main.Priority = 1;
        }

        else if(cameraToChange == "dealer")
        {
            main.Priority = 0;
            left.Priority = 0;
            right.Priority = 0;
            down.Priority = 0;
            NpcCamera.Priority = 0;
            dealerCamera.Priority = 1;
        }

        else if(cameraToChange == "npc")
        {
            main.Priority = 0;
            left.Priority = 0;
            right.Priority = 0;
            down.Priority = 0;
            dealerCamera.Priority = 0;
            NpcCamera.Priority = 1;
        }
    }

    
}
