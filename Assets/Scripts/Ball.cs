using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    public float ballTouchSpeed=5.0f;
    public float maxSpeed=10f;
    public float reflectRandomPower = 0.5f;
    Rigidbody rb;
    private Vector3 initPosition;

    void Start(){
        rb = GetComponent<Rigidbody>();
        initPosition = rb.position ;
    }

    void OnCollisionEnter(Collision other){
        if(other.transform.CompareTag("Wall")){
            // Debug.Log("other.transform.forward = "+other.transform.forward);
            Debug.Log(":::"+other.GetContact(0).normal);
            Debug.Log("##"+rb.velocity);
            Vector3 temp;
            // float rand0 = Random.Range(0.0f,1.0f);
            // float rand1 = Random.Range(0.0f,1.0f);
            if(other.transform.localScale.z>other.transform.localScale.x){
                temp = new Vector3(-rb.velocity.x, 0, rb.velocity.z).normalized;// + Random.onUnitSphere*reflectRandomPower;
                temp.y = 0;
                // temp.y = initPosition.y;
            }
            else{
                temp = new Vector3(rb.velocity.x, 0, -rb.velocity.z).normalized;// + Random.onUnitSphere*reflectRandomPower;
                temp.y = 0;
                // temp.y = initPosition.y;
            }
            Reflect(temp);
        }
    }


    public void Reflect(Vector3 direction){
        Debug.Log("reflect "+direction);
        
        if(rb.velocity.magnitude<maxSpeed){
            rb.AddForce(direction * ballTouchSpeed, ForceMode.Impulse);
        }
        else{
            rb.AddForce(direction * ballTouchSpeed/2, ForceMode.Impulse);
        }
    }


}
