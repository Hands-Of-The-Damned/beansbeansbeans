using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDrop
{
    public bool IsDroppable { get; }

    public bool AcceptDrop(IDrag drag);

    public void OnDrop(IDrag drag);
}
