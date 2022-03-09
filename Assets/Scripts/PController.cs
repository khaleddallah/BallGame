using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PController : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed=2.0f;
    public float ballTouchSpeed=5.0f;
    public float shootSpeed=30.0f;
    public float smoothTime=0.1f;

    public float maxAngle = 90.0f;
    public float maxMovement = 10.0f;

    public float offsetBall = 1f;
    private Rigidbody rb;
    private Vector3 velocity;

    private float angleInput;
    private float currentAngle;

    private float horizontalInputSmooth;
    private float horizontalInputVelocity;

    private GameObject currentBall;

    private bool shootTrig;
    private Camera cam;

    private float initAngle;
    private Vector3 initPosition;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 

        shootTrig = false;
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        initAngle = transform.eulerAngles.y;
        currentAngle = initAngle;
        initPosition = rb.position;
    }


    void Update()
    {
        InputMove();
        InputCtrlBall();
    }


    void InputMove(){
        float horizontalInput = -Input.GetAxisRaw("Horizontal");
        horizontalInputSmooth = Mathf.SmoothDamp(horizontalInputSmooth, horizontalInput, ref horizontalInputVelocity, smoothTime);
        
        velocity = Vector3.right * moveSpeed * horizontalInputSmooth;



        if(currentBall){
            angleInput = 0;
            if(Input.GetKey(KeyCode.Z)) angleInput += -1;
            if(Input.GetKey(KeyCode.C)) angleInput += 1; 

            float nextAngle = currentAngle + (angleInput*rotateSpeed);
            if( Mathf.Abs(nextAngle-initAngle) >= maxAngle ){
                Debug.Log(".,.");
            }
            else{
                currentAngle = nextAngle;
            }
        }
        


    }


    void InputCtrlBall(){
        if(currentBall && Input.GetKeyDown(KeyCode.Space)){
            shootTrig = true;
        }
    }


    void FixedUpdate(){
        Move();
        CtrlBall();
    }


    void Move(){
        float newXPos = rb.position.x + velocity.x * Time.fixedDeltaTime ;
        if( newXPos<maxMovement && newXPos>-maxMovement ){
            rb.MovePosition(new Vector3(newXPos, initPosition.y, initPosition.z));
        }
        if( (currentAngle-initAngle) <maxAngle && (currentAngle-initAngle)>-maxAngle){
            rb.MoveRotation(Quaternion.Euler(Vector3.up* currentAngle));
        }
    }


    void CtrlBall(){
        if(currentBall){
            if(shootTrig){
                Debug.Log("Shoot");
                currentBall.GetComponent<Rigidbody>().AddForce(transform.forward * shootSpeed, ForceMode.Impulse);
            }
        }
    }


    void OnTriggerEnter(Collider other){
        if(other.transform.CompareTag("Ball")){
            Debug.Log("BallTrigEnter");
            currentBall = other.gameObject;
        }
    }


    void OnTriggerExit(Collider other){
        if(other.transform.CompareTag("Ball")){
            Debug.Log("Ex");
            currentBall = null;
            shootTrig = false;
            currentAngle = initAngle;
        }
    }

    void OnTriggerStay(Collider other){
        if(other.transform.CompareTag("Ball")){
            // Debug.Log("Stay");
            if(!shootTrig){
                currentBall = other.gameObject;
                currentBall.transform.position = transform.position + transform.forward*offsetBall; // new Vector3(transform.position.x, currentBall.transform.position.y, transform.position.z-offsetBall);
                currentBall.transform.eulerAngles = transform.eulerAngles;
                // currentBall.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(Vector3.up* initAngle));
                // currentBall.GetComponent<Rigidbody>().MovePosition(new Vector3(rb.position.x, rb.position.y, rb.position.z-0.5f));
            }
        }
    }


    void OnCollisionEnter(Collision other){
        if(other.transform.CompareTag("Ball")){
            // other.gameObject.GetComponent<Ball>().Reflect(transform.forward);
        }
    }


}
