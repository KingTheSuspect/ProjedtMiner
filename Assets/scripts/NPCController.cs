using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] public List<DirectionSetter> Resources = new List<DirectionSetter>() ;
    
    public Transform gravityTarget;
    
    public Transform moveToTarget;

    public float power = 50000f;
    public float torque = 500f;

    public float gravity = 9.81f;
    private Rigidbody rb;

    public float autoOrientSpeed = 15f;

    public bool isMining = false;

    private TextMeshPro carry;

    private DirectionSetter Resource;
    
    public Animator anim;
    
    public bool cycleFinished;
    
    public bool ResourcesSet;
    void Start()
    {
        //Resource = GameObject.Find("Resource").GetComponentInChildren<DirectionSetter>();
        anim = GetComponentInChildren<Animator>();
        carry = GetComponentInChildren<TextMeshPro>();
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame

    private void Update()
    {
        
        carry.transform.LookAt(Camera.main.transform);
        carry.transform.Rotate(0, 180, 0);
        
    }

    void FixedUpdate()
    {
        
        ProccessGravity();
        
        if(cycleFinished) // if cycle is finished, reset the animation to Idle
        {
            Resource.isClicked = false;
            anim.SetBool("direction", false);
            cycleFinished = false;
        }
        
       
        if (!isMining && Resource != null && Resource.isClicked) //move to resource
        {
            anim.SetBool("direction", true);
            MoveTowardsTarget();
            

        }
        
        
        else //if mining, stop moving
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
        
        rb.constraints = RigidbodyConstraints.None;
            
        
        gameObject.GetComponent<Transform>().LookAt(moveToTarget);
       
       
        
       
    }

    void OnEnable()
    {
        DirectionSetter.OnClicked += HandleDirectionSetterClicked;
    }

    void OnDisable()
    {
        DirectionSetter.OnClicked -= HandleDirectionSetterClicked;
    }

    void HandleDirectionSetterClicked(DirectionSetter directionSetter)
{
    Resource = directionSetter;
    moveToTarget = Resource.transform.parent.GetComponentInChildren<TriggerManager>().transform;
}


    
    
}
