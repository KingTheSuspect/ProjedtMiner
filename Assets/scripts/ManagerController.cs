using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ManagerController : MonoBehaviour
{
    private Transform gravityTarget;
    
    public Transform moveToTarget;
    
    public float gravity = 9.81f;
   // private Rigidbody rb;

    public float autoOrientSpeed = 15f;

    public bool isMining;

    private TextMeshPro cooldown;

    private DirectionSetter Resource;
    
    public Animator anim;
    
    public bool cycleFinished;
    
    public bool isMoving = false;
    
    public bool npcIsClicked = false;

    public spawnPoint sp;

    public Rigidbody rb;

    public GameObject[] resourceObjects;
    public GameObject[] npcObjects;
    public List<GameObject> npcObjectsList;
    void Start()
    {
        sp = GameObject.Find("spawnObject").GetComponent<spawnPoint>();
        
        resourceObjects = GameObject.FindGameObjectsWithTag("Resource");
        
        npcObjects = GameObject.FindGameObjectsWithTag("Player");
        
        npcObjectsList = new List<GameObject>(npcObjects);
        
        gravityTarget = GameObject.Find("Sphere").GetComponent<Transform>();

        anim = GetComponentInChildren<Animator>();


    }

    private void FixedUpdate()
    {
        ProccessGravity();
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
    
    
    private void OnEnable()
    {
        spawnPoint.OnNpcCreated += UpdateNpcList;
        NPCController.OnCycleFinished += AssignRandomTargetToNPC;
        
    }

    private void OnDisable()
    {
        spawnPoint.OnNpcCreated -= UpdateNpcList;
        NPCController.OnCycleFinished -= AssignRandomTargetToNPC;
    }

    private void UpdateNpcList()
    {
        StartCoroutine(DelayedUpdate());
    }

    private IEnumerator DelayedUpdate()
    {
        // Wait until next frame
        yield return null;

        npcObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (var obj in npcObjects)
        {
            Debug.Log(obj.name);
            obj.GetComponent<NPCController>().mode = false;
        }
    }
    
    private void AssignRandomTargetToNPC(NPCController npc)
    {
        int i = UnityEngine.Random.Range(0, resourceObjects.Length);
        npc.moveToTarget = resourceObjects[i].transform.parent.GetComponentInChildren<TriggerManager>().transform;
    }
    
    
}
