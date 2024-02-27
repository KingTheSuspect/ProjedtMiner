using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] public List<DirectionSetter> Resources = new List<DirectionSetter>() ;
    
    private Transform gravityTarget;
    
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
    
    public bool isMoving = false;
    
    public bool npcIsClicked = false;
    void Start()
    {
        gravityTarget = GameObject.Find("Sphere").GetComponent<Transform>();
        anim = GetComponentInChildren<Animator>();
        carry = GetComponentInChildren<TextMeshPro>();
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame

    private void Update()
    {
        
        carry.transform.LookAt(Camera.main.transform);
        carry.transform.Rotate(0, 180, 0);
        
        if (Resource != null && Resource.isClicked)
        {
            npcIsClicked = true;
            
        }
    }

    void FixedUpdate()
    {
        
        ProccessGravity();
        
        if(cycleFinished) // if cycle is finished, reset the animation to Idle
        {
            npcIsClicked = false;
            anim.SetBool("direction", false);
            cycleFinished = false;
        }
        
       
        if (!isMining && npcIsClicked) //move to resource
        {
            anim.SetBool("direction", true);
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
        Vector3 gravityDirection = (gravityTarget.position - transform.position).normalized;

        // Calculate the direction from the object to the target
        Vector3 targetDirection = (moveToTarget.position - transform.position).normalized;

        // Project the target direction onto the plane that's perpendicular to the gravity direction
        Vector3 forwardDirection = Vector3.ProjectOnPlane(targetDirection, gravityDirection).normalized;

        // Move the object along the forward direction
        transform.Translate(forwardDirection * 6 * Time.deltaTime, Space.World);
        rb.constraints = RigidbodyConstraints.None;

        // Look at the target
        Quaternion targetRotation = Quaternion.LookRotation(forwardDirection, -gravityDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * autoOrientSpeed);
       
        isMoving = true;
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
        if (!isMoving)
        {
            Resource = directionSetter;
            moveToTarget = Resource.transform.parent.GetComponentInChildren<TriggerManager>().transform;
        }
        
    }


    
    
}
