using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] public Material[] cardMat;
    Renderer cardRend;

    private int currentModel;

    private void Start()
    {
        cardRend = GetComponent<Renderer>();
        cardRend.enabled = true;
        cardRend.sharedMaterial = cardMat[0];
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cardRend.sharedMaterial = cardMat[currentModel];
            currentModel++;

            if(currentModel >= cardMat.Length)
            {
                currentModel = 0;
            }
        }
    }
}
