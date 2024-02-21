using UnityEngine;
using UnityEditor;

public class PlaceOnSphereEditorScript : Editor
{
    [MenuItem("GameObject/Place On Sphere", false, 0)]
    static void PlaceOnSphere(MenuCommand menuCommand)
    {
        // Get the selected GameObject
        GameObject selectedObject = menuCommand.context as GameObject;

        // Specify the sphere to place the object on
        Transform sphere = GameObject.Find("Sphere").transform; // Replace "YourSphere" with the name of your sphere

        // Calculate the direction from the sphere's center to the object
        Vector3 directionFromSphereToObj = selectedObject.transform.position - sphere.position;

        // Create a rotation that aligns the object's up direction with the direction from the sphere's center to the object
        Quaternion rotation = Quaternion.LookRotation(selectedObject.transform.forward, directionFromSphereToObj.normalized);

        // Apply the rotation to the object
        selectedObject.transform.rotation = rotation;
    }
}