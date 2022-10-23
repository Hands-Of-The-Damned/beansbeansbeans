using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class majorArcanaInHand : MonoBehaviour
{
    private bool _IsReversed = false;
    public string CardName { get; set; }

    public void reverseCard()
    {
        _IsReversed = !_IsReversed;
    }
}
