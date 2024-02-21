using System.Collections;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // The planet's transform (center of orbit)
    public float smoothingFactor = 5f; // Smoothing factor for momentum
    public float pinchZoomSpeed = 5f; // Pinch zoom speed
    public float multiplier = 5f;

    private Vector3 lastMousePosition;
    private Vector3 swipeVelocity;
    private bool isMouseDragging = false;
    private bool isZooming = false;
    
    private float orthoZoomSpeed = 0.1f;
    private float perspectiveZoomSpeed = 0.1f;
    void Update()
    {
        // Check for mouse input
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
            isMouseDragging = true;
            swipeVelocity = Vector3.zero;
        }
        else if (Input.GetMouseButton(0))
        {
            // Calculate swipe velocity
            Vector3 delta = Input.mousePosition - lastMousePosition;
            swipeVelocity = delta / (Time.deltaTime * multiplier);
            lastMousePosition = Input.mousePosition;

            if (isMouseDragging)
            {
                OrbitCamera(swipeVelocity.x, swipeVelocity.y);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Release the mouse drag
            isMouseDragging = false;
            // Start coroutine to apply momentum after releasing the mouse button
            StartCoroutine(ApplyMomentumCoroutine());
        }
        
        
        if (Input.touchCount == 2)
        {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // If the camera is orthographic...
                if (Camera.main.orthographic)
                {
                    // ... change the orthographic size based on the change in distance between the touches.
                    Camera.main.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                    // Make sure the orthographic size never drops below zero.
                    Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, 0.1f);
                }
                else
                {
                    // Otherwise change the field of view based on the change in distance between the touches.
                    Camera.main.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                    // Clamp the field of view to make sure it's between 0 and 180.
                    Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 0.1f, 179.9f);
                }
        }
        
        

        // Simulate pinch zoom with scroll wheel
        float scrollWheelDelta = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollWheelDelta) > 0.0f)
        {
            HandlePinchZoom(scrollWheelDelta);
        }
    }

    void OrbitCamera(float swipeVelocityX, float swipeVelocityY)
    {
        // Orbit the camera around the planet based on swipe velocity
        float rotationAmountX = swipeVelocityX * Time.deltaTime;
        float rotationAmountY = -swipeVelocityY * Time.deltaTime; // Reverse Y-axis rotation for more intuitive control
        transform.RotateAround(target.position, Vector3.up, rotationAmountX);
        transform.RotateAround(target.position, transform.right, rotationAmountY);
    }

    IEnumerator ApplyMomentumCoroutine()
    {
        // Gradually reduce the rotation speed over time
        while (!isMouseDragging && (Mathf.Abs(swipeVelocity.x) > 0.01f || Mathf.Abs(swipeVelocity.y) > 0.01f))
        {
            swipeVelocity = Vector3.Lerp(swipeVelocity, Vector3.zero, smoothingFactor * Time.deltaTime);
            OrbitCamera(swipeVelocity.x, swipeVelocity.y);
            yield return null;
        }
    }

    void HandlePinchZoom(float scrollDelta)
    {
        // Simulate pinch zoom with the mouse scroll wheel
        Camera.main.fieldOfView -= scrollDelta * pinchZoomSpeed * Time.deltaTime * 100f;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 10f, 80f);
    }
}
