using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public sealed class CardDrag : MonoBehaviour, IDrag
{
    public bool IsDraggable { get; private set; } = true;

    public bool Dragging { get; set; }

    private Vector3 dragOriginPosition;

    public void OnPointerEnter(Vector3 position) { }

    public void OnPointerExit(Vector3 position) { }

    public void OnBeginDrag(Vector3 position)
    {
        dragOriginPosition = transform.position;

        transform.position = new Vector3(transform.position.x, position.y, transform.position.z);
    }

    public void OnDrag(Vector3 deltaPosition, IDrop droppable)
    {
        deltaPosition.y = 0.0f;

        transform.position += deltaPosition;
    }

    public void OnEndDrag(Vector3 position, IDrop droppable)
    {
        if (droppable is { IsDroppable: true } && droppable.AcceptDrop(this) == true)
            transform.position = new Vector3(transform.position.x, position.y, transform.position.z);
        else
            transform.position = dragOriginPosition;
    }
}