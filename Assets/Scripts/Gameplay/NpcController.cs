using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    
    public static string zoomActive = "n";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            zoomActive = "y";
        }
        if(Input.GetMouseButtonDown(1))
        {
            zoomActive = "n";
        }
               
    }
}
