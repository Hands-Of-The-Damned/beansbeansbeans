//Michael "Mickey" Kerr
//2022

using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{

    //Rotates object to camera on y axis
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
    }
}
