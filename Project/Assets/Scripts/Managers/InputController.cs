using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions
{
    None,
    Left,
    Right,
    Up,
    Down
}

public class InputController : MonoBehaviour
{
    private Vector2 initialPosition = Vector2.zero;
    private float minDist = 5;
    private GridObject selectedObject;
    private Camera cam;


    public Directions currentDirection;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            if (initialPosition == Vector2.zero)
                BeginClick(Input.GetTouch(0).position);

            HoldingClick(Input.GetTouch(0).position);

        }
        else
        {
            ReleaseClick();
        }
#endif
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            if (initialPosition == Vector2.zero)
                BeginClick(Input.mousePosition);

            HoldingClick(Input.mousePosition);

        }
        else
        {
            ReleaseClick();
        }
#endif

    }



    private void BeginClick(Vector3 initialPos)
    {
        var ray = cam.ScreenPointToRay(initialPos);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 30);

        RaycastHit hit;
        int layerMask = 1 << 6;
        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            if (hit.collider != null)
            {
                selectedObject = hit.collider.gameObject.GetComponent<GridObject>();
                initialPosition = initialPos;
            }
        }
    }

    private void HoldingClick(Vector3 currentPos)
    {
        var difX = initialPosition.x - currentPos.x;
        var difY = initialPosition.y - currentPos.y;

        if (currentDirection == Directions.None)
        {
            if (difX > minDist)
            {
                Debug.Log("Move left");
                currentDirection = Directions.Left;
            }
            else if (difX < -minDist)
            {
                Debug.Log("Move Right");
                currentDirection = Directions.Right;
            }
            else if (difY > minDist)
            {
                Debug.Log("Move down");
                currentDirection = Directions.Down;
            }
            else if (difY < -minDist)
            {
                Debug.Log("Move up");
                currentDirection = Directions.Up;
            }

            if(currentDirection != Directions.None)
            selectedObject.MoveToDirection(currentDirection);
        }

    }

    private void ReleaseClick()
    {
        initialPosition = Vector2.zero;
        currentDirection = Directions.None;
    }
}
