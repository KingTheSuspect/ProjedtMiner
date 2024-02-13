using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    
    public Transform gravityTarget;
    
    public Transform moveToTarget;

    public float power = 15000f;
    public float torque = 500f;

    public float gravity = 9.81f;
    private Rigidbody rb;

    public float autoOrientSpeed = 5f;

    public bool isMining = false;
    
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        ProccessGravity();
        
        if (!isMining)
        {
            MoveTowardsTarget();
        
        }
        else
        {
            
            rb.velocity = Vector3.zero;
            
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    

    void ProccessGravity()
    {
        var position = transform.position;
        Vector3 diff = position - gravityTarget.position;
        rb.AddForce(- diff.normalized * (gravity * rb.mass));
        Debug.DrawRay(position, diff.normalized, Color.red);
        
        autoOrient(-diff);
    }

    void autoOrient(Vector3 down)
    {
        var rotation = transform.rotation;
        Quaternion orientationDirection = Quaternion.FromToRotation(-transform.up, down) * rotation;
        rotation = Quaternion.Slerp(rotation, orientationDirection, autoOrientSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }
    
    void MoveTowardsTarget()
    {
        
        Vector3 targetDirection = moveToTarget.position - transform.position;

        // Normalize the direction to get a unit vector
        Vector3 normalizedDirection = targetDirection.normalized;

        // Apply a constant force towards the target
        rb.AddForce(normalizedDirection * power);
        
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;
            
        
        gameObject.GetComponent<Transform>().LookAt(moveToTarget);
       
        float stoppingDistance = 1.3f;
        
       
    }




    
    
}
