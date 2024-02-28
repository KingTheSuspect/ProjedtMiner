using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPoint : MonoBehaviour
{
    public NPCController NPC;
    public ManagerController manager;
    public bool managerExists;
    
    public static event Action OnNpcCreated;
    
    


    public void spawnNPC()
    {
        
        Instantiate(NPC, gameObject.transform.position, Quaternion.identity);
        
        Debug.Log(managerExists);
        
        if (managerExists)
        {
            
            OnNpcCreated?.Invoke();
        }
       
        
    }

    public void Manager()
    {
        //Instantiate(manager, gameObject.transform.position, Quaternion.identity);
        managerExists = true;
    }
    
    
    
    
}
