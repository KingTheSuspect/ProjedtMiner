using System;
using System.Collections;
using UnityEngine;

public class DirectionSetter : MonoBehaviour
{
    public bool isClicked = false;

    // Add an event that is raised when the object is clicked
    public static event Action<DirectionSetter> OnClicked;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
        
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    isClicked = true;
                   

                    // Raise the OnClicked event
                    OnClicked?.Invoke(this);

                    // Start the ResetIsClicked coroutine
                    StartCoroutine(ResetIsClicked());
                }
            }
        }
    }

    IEnumerator ResetIsClicked()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Reset isClicked to false
        isClicked = false;
    }
}