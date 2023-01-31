//Jordan Newkirk

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{
    // Layer of the objects to be detected.
    [SerializeField]
    private LayerMask raycastMask;

    [SerializeField, Range(0.0f, 10.0f)]
    private float dragSpeed = 1.0f;

    [SerializeField, Range(0.0f, 10.0f)]
    private float height = 0.1f;

    [SerializeField]
    private Vector2 cardSize;

    private IDrag currentDrag;

    private IDrag possibleDrag;

    private Transform currentDragTransform;

    private Vector3 oldMouseWorldPosition;

    // How many impacts of the beam we want to obtain.
    private const int HitsCount = 5;

    // Information on the impacts of shooting a ray.
    private readonly RaycastHit[] raycastHits = new RaycastHit[HitsCount];

    private readonly RaycastHit[] cardHits = new RaycastHit[4];

    // Ray created from the camera to the projection of the mouse
    // coordinates on the scene.
    private Ray mouseRay;


    private Vector3 MousePositionToWorldPoint()
    {
        Vector3 mousePosition = Input.mousePosition;
        if (Camera.main.orthographic == false)
            mousePosition.z = 10.0f;

        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void ResetCursor() => Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

    /// <summary>
    /// Returns the Transfrom of the object closest to the origin
    /// of the ray.
    /// </summary>
    /// <returns>Transform or null if there is no impact.</returns>
    private Transform MouseRaycast()
    {
        Transform hit = null;

        // Fire the ray!
        if (Physics.RaycastNonAlloc(mouseRay,
                                    raycastHits,
                                    Camera.main.farClipPlane,
                                    raycastMask) > 0)
        {
            // We order the impacts according to distance.
            System.Array.Sort(raycastHits, (x, y) => x.distance.CompareTo(y.distance));

            // We are only interested in the first one.
            hit = raycastHits[0].transform;
        }

        return hit;
    }

    private IDrop DetectDroppable()
    {
        IDrop droppable = null;

        Vector3 position = currentDragTransform.position;
        Vector2 halfSize = cardSize * 0.5f;
        Vector3[] cardCorner =
        {
            new(position.x + halfSize.x, position.y, position.z - halfSize.y),
            new(position.x + halfSize.x, position.y, position.z + halfSize.y),
            new(position.x - halfSize.x, position.y, position.z - halfSize.y),
            new(position.x - halfSize.x, position.y, position.z + halfSize.y)
        };

        int cardHitIndex = 0;
        System.Array.Clear(cardHits, 0, cardHits.Length);

        for (int i = 0; i < cardCorner.Length; ++i)
        {
            Ray ray = new(cardCorner[i], Vector3.down);

            int hits = Physics.RaycastNonAlloc(ray, raycastHits, Camera.main.farClipPlane, raycastMask);


            if (hits > 0)
            {
                System.Array.Sort(raycastHits, (x, y) => x.transform != null ? x.distance.CompareTo(y.distance) : -1);

                cardHits[cardHitIndex++] = raycastHits[0];
            }
        }

        if (cardHitIndex > 0)
        {
            System.Array.Sort(cardHits, (x, y) => x.transform != null ? x.distance.CompareTo(y.distance) : -1);

            if (cardHits[0].transform != null)
                droppable = cardHits[0].transform.GetComponent<IDrop>();
        }
        return droppable;
    }


    public IDrag DetectDraggable()
    {
        IDrag draggable = null;

        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Transform hit = MouseRaycast();
        if(hit != null)
        {
            draggable = hit.GetComponent<IDrag>();
            if (draggable is { IsDraggable: true })
                currentDragTransform = hit;
            else
                draggable = null;
                   
        }
        return draggable;
    }

    //Once per frame
    void Update()
    {
        if(currentDrag == null)
        {
            IDrag draggable = DetectDraggable();

            if (Input.GetMouseButtonDown(0) == true)
            {
                if (draggable != null)
                {
                    currentDrag = draggable;
                    //currentDragTransform = hit;
                    oldMouseWorldPosition = MousePositionToWorldPoint();

                    Cursor.visible = false;

                    Cursor.lockState = CursorLockMode.Confined;

                    currentDrag.Dragging = true;
                    currentDrag.OnBeginDrag(new Vector3(raycastHits[0].point.x, raycastHits[0].point.y + height, raycastHits[0].point.z));

                }
            }

            else
            {
                if (draggable != null && possibleDrag == null)
                {
                    possibleDrag = draggable;
                    possibleDrag.OnPointerEnter(raycastHits[0].point);
                }

                if (draggable == null && possibleDrag != null)
                {
                    possibleDrag.OnPointerExit(raycastHits[0].point);
                    possibleDrag = null;

                    //ResetCursor();
                }
            }
        }
        else
        {
            IDrop droppable = DetectDroppable();

            if(Input.GetMouseButton(0) == true)
            {
                Vector3 mouseWorldPosition = MousePositionToWorldPoint();
                Vector3 offset = (mouseWorldPosition - oldMouseWorldPosition) * dragSpeed;

                currentDrag.OnDrag(offset, droppable);

                oldMouseWorldPosition = mouseWorldPosition;
            }
            else if(Input.GetMouseButtonUp(0) == true)
            {
                currentDrag.Dragging = false;
                currentDrag.OnEndDrag(raycastHits[0].point, droppable);
                currentDrag = null;
                currentDragTransform = null;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void OnEnable()
    {
        possibleDrag = null;
        currentDragTransform = null;

        ResetCursor();
    }

}
