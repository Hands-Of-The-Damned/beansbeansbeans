using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera main;
    [SerializeField] CinemachineVirtualCamera left;
    [SerializeField] CinemachineVirtualCamera right;
    [SerializeField] CinemachineVirtualCamera down;
    [SerializeField] CinemachineVirtualCamera NpcCamera;
    [SerializeField] CinemachineVirtualCamera NpcCamera1;
    [SerializeField] CinemachineVirtualCamera NpcCamera2;
    [SerializeField] CinemachineVirtualCamera NpcCamera3;
    [SerializeField] CinemachineVirtualCamera dealerCamera;
    [SerializeField] CinemachineVirtualCamera bowlCamera;

    Ray ray;
    RaycastHit hit;

    private bool dealerCameraActive;

    private bool bowlCamActive;

    public GameObject betBronze;
    public GameObject betSilver;
    public GameObject betGold;


    // Start is called before the first frame update
    void Start()
    {
        betBronze.SetActive(false);
        betSilver.SetActive(false);
        betGold.SetActive(false);
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
            //Dealer Zoom
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
            //Npc Zoom
            if(hit.collider.name == "NpcSprite" && Input.GetMouseButtonDown(0))
            {
                SwitchPriority("npc");
            }
            else if(hit.collider.name == "NpcSprite" && Input.GetMouseButtonDown(1))
            {
                SwitchPriority("l");
            }
            //Npc1 Zoom
            if (hit.collider.name == "NpcSprite1" && Input.GetMouseButtonDown(0))
            {
                SwitchPriority("npc1");
            }
            else if (hit.collider.name == "NpcSprite1" && Input.GetMouseButtonDown(1))
            {
                SwitchPriority("m");
            }
            //Npc2 Zoom
            if (hit.collider.name == "NpcSprite2" && Input.GetMouseButtonDown(0))
            {
                SwitchPriority("npc2");
            }
            else if (hit.collider.name == "NpcSprite2" && Input.GetMouseButtonDown(1))
            {
                SwitchPriority("m");
            }
            //Npc3 Zoom
            if (hit.collider.name == "NpcSprite3" && Input.GetMouseButtonDown(0))
            {
                SwitchPriority("npc3");
            }
            else if (hit.collider.name == "NpcSprite3" && Input.GetMouseButtonDown(1))
            {
                SwitchPriority("r");
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
            left.Priority = 1;
            right.Priority = 0;
            down.Priority = 0;
            dealerCamera.Priority = 0;
            NpcCamera.Priority = 0;
            NpcCamera1.Priority = 0;
            NpcCamera2.Priority = 0;
            NpcCamera3.Priority = 0;
            bowlCamera.Priority = 0;
            betBronze.SetActive(false);
            betSilver.SetActive(false);
            betGold.SetActive(false);
        }
        else if(cameraToChange == "r")
        {
            main.Priority = 0;
            left.Priority = 0;
            right.Priority = 1;
            down.Priority = 0;
            dealerCamera.Priority = 0;
            NpcCamera.Priority = 0;
            NpcCamera1.Priority = 0;
            NpcCamera2.Priority = 0;
            NpcCamera3.Priority = 0;
            bowlCamera.Priority = 0;
            betBronze.SetActive(false);
            betSilver.SetActive(false);
            betGold.SetActive(false);
        }
        else if(cameraToChange == "d")
        {
            main.Priority = 0;
            left.Priority = 0;
            right.Priority = 0;
            down.Priority = 1;
            dealerCamera.Priority = 0;
            NpcCamera.Priority = 0;
            NpcCamera1.Priority = 0;
            NpcCamera2.Priority = 0;
            NpcCamera3.Priority = 0;
            bowlCamera.Priority = 0;
            betBronze.SetActive(false);
            betSilver.SetActive(false);
            betGold.SetActive(false);
        }
        else if(cameraToChange == "m")
        {
            main.Priority = 1;
            left.Priority = 0;
            right.Priority = 0;
            down.Priority = 0;
            dealerCamera.Priority = 0;
            NpcCamera.Priority = 0;
            NpcCamera1.Priority = 0;
            NpcCamera2.Priority = 0;
            NpcCamera3.Priority = 0;
            bowlCamera.Priority = 0;
            betBronze.SetActive(false);
            betSilver.SetActive(false);
            betGold.SetActive(false);
        }

        else if(cameraToChange == "dealer")
        {
            main.Priority = 0;
            left.Priority = 0;
            right.Priority = 0;
            down.Priority = 0;
            dealerCamera.Priority = 1;
            NpcCamera.Priority = 0;
            NpcCamera1.Priority = 0;
            NpcCamera2.Priority = 0;
            NpcCamera3.Priority = 0;
            bowlCamera.Priority = 0;
            betBronze.SetActive(false);
            betSilver.SetActive(false);
            betGold.SetActive(false);
        }

        else if(cameraToChange == "npc")
        {
            main.Priority = 0;
            left.Priority = 0;
            right.Priority = 0;
            down.Priority = 0;
            dealerCamera.Priority = 0;
            NpcCamera.Priority = 1;
            NpcCamera1.Priority = 0;
            NpcCamera2.Priority = 0;
            NpcCamera3.Priority = 0;
            bowlCamera.Priority = 0;
            betBronze.SetActive(false);
            betSilver.SetActive(false);
            betGold.SetActive(false);
        }

        else if (cameraToChange == "npc1")
        {
            main.Priority = 0;
            left.Priority = 0;
            right.Priority = 0;
            down.Priority = 0;
            dealerCamera.Priority = 0;
            NpcCamera.Priority = 0;
            NpcCamera1.Priority = 1;
            NpcCamera2.Priority = 0;
            NpcCamera3.Priority = 0;
            bowlCamera.Priority = 0;
            betBronze.SetActive(false);
            betSilver.SetActive(false);
            betGold.SetActive(false);

        }

        else if (cameraToChange == "npc2")
        {
            main.Priority = 0;
            left.Priority = 0;
            right.Priority = 0;
            down.Priority = 0;
            dealerCamera.Priority = 0;
            NpcCamera.Priority = 0;
            NpcCamera1.Priority = 0;
            NpcCamera2.Priority = 1;
            NpcCamera3.Priority = 0;
            bowlCamera.Priority = 0;
            betBronze.SetActive(false);
            betSilver.SetActive(false);
            betGold.SetActive(false);

        }

        else if (cameraToChange == "npc3")
        {
            main.Priority = 0;
            left.Priority = 0;
            right.Priority = 0;
            down.Priority = 0;
            dealerCamera.Priority = 0;
            NpcCamera.Priority = 0;
            NpcCamera1.Priority = 0;
            NpcCamera2.Priority = 0;
            NpcCamera3.Priority = 1;
            bowlCamera.Priority = 0;
            betBronze.SetActive(false);
            betSilver.SetActive(false);
            betGold.SetActive(false);

        }

        else if (cameraToChange == "bowl")
        {
            main.Priority = 0;
            left.Priority = 0;
            right.Priority = 0;
            down.Priority = 0;
            dealerCamera.Priority = 0;
            NpcCamera.Priority = 0;
            NpcCamera1.Priority = 0;
            NpcCamera2.Priority = 0;
            NpcCamera3.Priority = 0;
            bowlCamera.Priority = 1;
            betBronze.SetActive(true);
            betSilver.SetActive(true);
            betGold.SetActive(true);
            bowlCamActive = true;
        }
    }

    
}
