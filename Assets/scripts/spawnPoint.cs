using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPoint : MonoBehaviour
{
    public NPCController NPC;
    //public ManagerController manager;

    public static event Action OnNpcCreated;

    
    
    public void spawnNPC()
    {
        
        Instantiate(NPC, gameObject.transform.position, Quaternion.identity);
        OnNpcCreated?.Invoke();
    }
    
    
    
    
}
