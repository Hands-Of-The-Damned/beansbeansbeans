/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class zoomCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam;
    //[SerializeField] CinemachineVirtualCamera npcCamera;
    [SerializeField] CinemachineVirtualCamera dealerCamera;
    [SerializeField] CinemachineVirtualCamera spriteCamera;
    

    public GameObject betBowl;
    

    // Update is called once per frame
    void Update()
    {
        
        Cursor.visible = true;

      //  if(NpcController.npcZoom == "y")
      //  {
        //    vcam.enabled = false;
        //    npcCamera.enabled = true;
        //}
        //else if(NpcController.npcZoom == "n")
        //{
        //    npcCamera.enabled = false;
        //    vcam.enabled = true;
        //}

        if(dealerController.zoomActive == "y")
        {
            vcam.enabled = false;
            dealerCamera.enabled = true;
        }
        else if(dealerController.zoomActive == "n")
        {
            dealerCamera.enabled = false;
            vcam.enabled = true;
        }

        if(BowlController.target == "y")
        {
            vcam.Follow = betBowl.transform;
            vcam.LookAt = betBowl.transform;
        }

        else if(BowlController.target == "n")
        {
            vcam.Follow = vcam.transform;
            vcam.LookAt = null;
        }

        if(spriteController.zoom == "y")
        {
            vcam.enabled = false;
            spriteCamera.enabled = true;
        }
        else if(spriteController.zoom == "n")
        {
            spriteCamera.enabled = false;
            vcam.enabled = true;
        }
    }
}
*/