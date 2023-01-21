//Jordan Newkirk

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used for the Draggable objects
public interface IDrag
{
    public bool IsDraggable { get; }

    public bool Dragging { get; set; }

    public void OnPointerEnter(Vector3 position);

    public void OnPointerExit(Vector3 position);

    public void OnBeginDrag(Vector3 position);

    public void OnDrag(Vector3 deltaPostition, IDrop droppable);

    public void OnEndDrag(Vector3 position, IDrop droppable);
}

