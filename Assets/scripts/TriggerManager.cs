using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    private Transform Target;
    public bool isResource;

    void Start()
    {
        Target = isResource ? GameObject.Find("baseTrigger").GetComponent<Transform>() : GameObject.Find("mineTrigger").GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PerformMining(other));
        }
    }

    private IEnumerator PerformMining(Collider other)
    {
        GravityManager gravityManager = other.GetComponent<GravityManager>();
        Animator animator = other.GetComponentInChildren<Animator>();

        if (gravityManager != null && animator != null)
        {
            gravityManager.isMining = true;
            animator.SetBool("mine", true);

            // Wait for 5 seconds
            yield return new WaitForSeconds(5f);

            gravityManager.isMining = false;
            animator.SetBool("mine", false);
            gravityManager.moveToTarget = Target;
        }
    }
}

