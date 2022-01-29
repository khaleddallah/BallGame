using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    public float ballTouchSpeed=5.0f;
    public float maxSpeed=10f;
    Rigidbody rb;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision other){
        if(other.transform.CompareTag("Wall")){
            Reflect(other.transform.forward);
        }
    }


    public void Reflect(Vector3 direction){
        Debug.Log("reflect "+rb.velocity.magnitude);
        if(rb.velocity.magnitude<maxSpeed){
            rb.AddForce(direction * ballTouchSpeed, ForceMode.Impulse);
        }
    }


}
