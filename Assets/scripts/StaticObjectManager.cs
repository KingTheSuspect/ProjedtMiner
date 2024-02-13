using System.Collections.Generic;
using UnityEngine;

public class StaticObjectPlacer : MonoBehaviour
{
    public List<GameObject> BaseObjects = new List<GameObject>();
    public int numberOfObjects = 100;
    public float sphereRadius = 5f;
    public float autoOrientSpeed = 5f;
    private Transform Target;
    void Start()
    {
        Target = GameObject.Find("Sphere").GetComponent<Transform>();
        PlaceObjectsOnSphere();
    }

    void PlaceObjectsOnSphere()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            float latitude = Random.Range(0f, Mathf.PI); // Random latitude between 0 and PI
            float longitude = Random.Range(0f, 2 * Mathf.PI); // Random longitude between 0 and 2*PI

            // Convert spherical coordinates to Cartesian coordinates
            float x = sphereRadius * Mathf.Sin(latitude) * Mathf.Cos(longitude);
            float y = sphereRadius * Mathf.Sin(latitude) * Mathf.Sin(longitude);
            float z = sphereRadius * Mathf.Cos(latitude);

            // Instantiate object at the calculated position
            Instantiate(BaseObjects[i], new Vector3(x, y, z), Quaternion.identity);
            autoOrient();
        }
    }
    
    void autoOrient()
    {
        var position = transform.position;
        Vector3 down = position - Target.position;
        
        var rotation = transform.rotation;
        Quaternion orientationDirection = Quaternion.FromToRotation(-transform.up, down) * rotation;
        rotation = Quaternion.Slerp(rotation, orientationDirection, autoOrientSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }
}