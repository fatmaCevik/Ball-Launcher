using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float respawnDelay;
    [SerializeField] private float detachDelay;

    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSpringJoint;
    private Camera mainCamera;
    private bool isDragging;
    
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }
    
    void Update()
    {
        if (currentBallRigidbody == null)
            return;

        BallMoving();
    }

    private void BallMoving()
    {
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if(isDragging)
            {
                LaunchBall();
            }

            isDragging = false; //Eðer dokunmuyorsak sürükleme yanlýþa eþittir.
            //currentBallRigidbody.isKinematic = false;
            return;
        }

        isDragging = true; // Eðer dokunuyorsak sürükleme doðruya eþittir. 
        currentBallRigidbody.isKinematic = true;
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
        //Debug.Log(worldPosition);
        currentBallRigidbody.position = worldPosition;
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity); // Prefabý somutlaþtýrma. 

        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot; // Ball'ý pivota baðlar. Spring Joint 2D nin Connected Rigidbody eriþim ile atama. 
    }

    private void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;

        //Invoke("DetachBall", detachDelay); ==> Dize kullanmak hataya elveriþlidir. 
        Invoke(nameof(DetachBall), detachDelay);
    }

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;

        Invoke(nameof(SpawnNewBall), respawnDelay);
    }
}
 