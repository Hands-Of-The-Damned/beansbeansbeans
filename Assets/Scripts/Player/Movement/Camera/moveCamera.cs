using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to make Camera stay at player position
public class moveCamera : MonoBehaviour
{
    public Transform Camera;
    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.position;
    }
}
