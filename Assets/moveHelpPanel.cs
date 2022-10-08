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
        moveHelpUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        moveHelpDown();
    }
    void Start()
    {
        moveHelpDown();
    }

    private void moveHelpUp()
    {
        gameObject.transform.position = gameObject.transform.position = new Vector3(100, 270, 0);
    }
    private void moveHelpDown()
    {
        gameObject.transform.position = gameObject.transform.position = new Vector3(-83,270,0);
    }
}
