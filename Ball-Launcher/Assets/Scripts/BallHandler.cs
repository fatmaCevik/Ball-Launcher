using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D currentBallRigidbody;
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
    }
    
    void Update()
    {
        BallMoving();
    }

    private void BallMoving()
    {
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            currentBallRigidbody.isKinematic = false;
            return;
        }

        currentBallRigidbody.isKinematic = true;
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
        //Debug.Log(worldPosition);
        currentBallRigidbody.position = worldPosition;
    }
}
 