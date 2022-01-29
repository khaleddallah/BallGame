using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PController : MonoBehaviour
{
    public float normalMoveSpeed=5f;
    public float quickMoveSpeed=8f;

    public float horizontalSpeed=5f;
    public float rotateSpeed=2.0f;
    public float ballTouchSpeed=5.0f;
    public float shootSpeed=30.0f;
    public float smoothTime=0.1f;
    

    private Rigidbody rb;
    private Vector3 velocity;
    private float moveSpeed;


    private float currentAngle;

    private float forwardInputSmooth;
    private float forwardInputVelocity;

    private float horizontalInputSmooth;
    private float horizontalInputVelocity;

    private GameObject currentBall;

    private bool shootTrig;
    private Camera cam;

    private float initAngle;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 

        shootTrig = false;
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        initAngle = transform.eulerAngles.y;
        currentAngle = initAngle;
    }


    void Update()
    {
        InputMove();
        InputCtrlBall();
    }


    void InputMove(){
        // assign the speed
        if(Input.GetKey(KeyCode.LeftShift)){
            moveSpeed = quickMoveSpeed;
        }
        else{
            moveSpeed = normalMoveSpeed;
        }

        Vector3 direction = (new Vector3(-Input.GetAxisRaw("Horizontal"), 0, -Input.GetAxisRaw("Vertical"))).normalized;
        forwardInputSmooth = Mathf.SmoothDamp(forwardInputSmooth, direction.magnitude, ref forwardInputVelocity, smoothTime);
        velocity = direction * moveSpeed * forwardInputSmooth;
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
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
        // rb.MoveRotation(Quaternion.Euler(Vector3.up* initAngle));
    }


    void CtrlBall(){
        if(currentBall){
            if(shootTrig){
                // shootTrig=false;
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
        }
    }

    void OnTriggerStay(Collider other){
        if(other.transform.CompareTag("Ball")){
            Debug.Log("Stay");
            if(!shootTrig){
                currentBall = other.gameObject;
                currentBall.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(Vector3.up* initAngle));
                currentBall.GetComponent<Rigidbody>().MovePosition(new Vector3(rb.position.x, rb.position.y, rb.position.z-0.5f));
            }
        }
    }


    void OnCollisionEnter(Collision other){
        if(other.transform.CompareTag("Ball")){
            // other.gameObject.GetComponent<Ball>().Reflect(transform.forward);
        }
    }






}
