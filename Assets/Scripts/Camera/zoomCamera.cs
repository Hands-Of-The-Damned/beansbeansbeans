using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class zoomCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] CinemachineVirtualCamera npcCamera;
    [SerializeField] CinemachineVirtualCamera dealerCamera;
    CinemachineComponentBase componentBase;

    
    private int currentCamera;

    // Update is called once per frame
    void Update()
    {
        if(componentBase == null)
        {
            componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        }

        if (NpcController.zoomActive == "y")
        {
            
            virtualCamera.enabled = false;
            npcCamera.enabled = true;
        }
        else if (NpcController.zoomActive == "n")
        {

            npcCamera.enabled = false;
            virtualCamera.enabled = true;
        }

        if(dealerController.zoomActive == "y")
        {
            virtualCamera.enabled = false;
            dealerCamera.enabled = true;
        }
        else if (dealerController.zoomActive == "y")
        {
            dealerCamera.enabled = false;
            virtualCamera.enabled = true;
        }
    }
}
