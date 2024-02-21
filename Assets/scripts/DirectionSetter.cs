using System;
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
                    Debug.Log("Clicked");

                    // Raise the OnClicked event
                    OnClicked?.Invoke(this);
                }
            }
        }
    }
}