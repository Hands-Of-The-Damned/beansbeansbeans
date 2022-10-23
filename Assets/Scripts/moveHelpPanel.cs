using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class moveHelpPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        moveHelpOut();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        moveHelpIn();
    }
    void Start()
    {
        moveHelpIn();
    }

    private void moveHelpOut()
    {
        gameObject.transform.position = gameObject.transform.position = new Vector3(100, 271, 0);
    }
    private void moveHelpIn()
    {
        //float horizontalPos = Input.GetAxis("Horizontal");
        //float verticalPos = Input.GetAxis("Vertical");
        gameObject.transform.position = gameObject.transform.position = new Vector3(-82, 271, 0);
        //Debug.Log(gameObject.transform.position);
    }
}
