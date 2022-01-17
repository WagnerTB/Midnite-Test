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
    private float minDist = 2;
    private GridObject selectedObject;
    private Camera cam;

    public Directions currentDirection;

    private bool lockInputs = false;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();

        GameManager.OnGameStateChanged += GameStateChanged;
    }

    private void GameStateChanged(GameState gameState)
    {
        lockInputs = gameState != GameState.Play;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockInputs) return;

#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount > 0)
        {
            if (initialPosition == Vector2.zero)
                BeginClick(Input.GetTouch(0).position);

            if (selectedObject != null)
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

            if (selectedObject != null)
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
        Debug.DrawLine(ray.origin, ray.direction*30, Color.red, 5);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 300))
        {
            if (hit.collider != null && hit.collider.GetComponent<GridObject>() != null)
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
                currentDirection = Directions.Left;
            }
            else if (difX < -minDist)
            {
                currentDirection = Directions.Right;
            }
            else if (difY > minDist)
            {
                currentDirection = Directions.Down;
            }
            else if (difY < -minDist)
            {
                currentDirection = Directions.Up;
            }

            if (currentDirection != Directions.None)
            {
                selectedObject.MoveToDirection(currentDirection);
            }
        }

    }

    private void ReleaseClick()
    {
        initialPosition = Vector2.zero;
        currentDirection = Directions.None;

    }
}
