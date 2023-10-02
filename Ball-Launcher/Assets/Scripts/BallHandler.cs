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

            isDragging = false; //E�er dokunmuyorsak s�r�kleme yanl��a e�ittir.
            //currentBallRigidbody.isKinematic = false;
            return;
        }

        isDragging = true; // E�er dokunuyorsak s�r�kleme do�ruya e�ittir. 
        currentBallRigidbody.isKinematic = true;
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
        //Debug.Log(worldPosition);
        currentBallRigidbody.position = worldPosition;
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity); // Prefab� somutla�t�rma. 

        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot; // Ball'� pivota ba�lar. Spring Joint 2D nin Connected Rigidbody eri�im ile atama. 
    }

    private void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;

        //Invoke("DetachBall", detachDelay); ==> Dize kullanmak hataya elveri�lidir. 
        Invoke(nameof(DetachBall), detachDelay);
    }

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;

        Invoke(nameof(SpawnNewBall), respawnDelay);
    }
}
 