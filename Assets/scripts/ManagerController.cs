using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MangerController : MonoBehaviour
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
        
        resourceObjects = GameObject.FindGameObjectsWithTag("Resource");
        
        npcObjects = GameObject.FindGameObjectsWithTag("Player");
        
        npcObjectsList = new List<GameObject>(npcObjects);
        
        gravityTarget = GameObject.Find("Sphere").GetComponent<Transform>();
       // anim = GetComponentInChildren<Animator>();
        //cooldown = GetComponentInChildren<TextMeshPro>();
        
        
        
    }
    
    private void OnEnable()
    {
        // Subscribe to the OnResourceCreated event
        spawnPoint.OnNpcCreated += UpdateNpcList;
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnResourceCreated event
        spawnPoint.OnNpcCreated -= UpdateNpcList;
    }

    private void UpdateNpcList()
    {
        
        npcObjects = GameObject.FindGameObjectsWithTag("Player");
        this.npcObjectsList = new List<GameObject>(npcObjects);

        foreach (var obj in npcObjectsList )
        {
            Debug.Log(obj.name);
        }
    }
    
    
}
