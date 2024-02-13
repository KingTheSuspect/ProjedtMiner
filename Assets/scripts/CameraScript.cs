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
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Calculate pinch zoom
            float pinchZoomDelta = Vector2.Distance(touch0.position, touch1.position) -
                                   Vector2.Distance(touch0.position - touch0.deltaPosition, touch1.position - touch1.deltaPosition);

            if (Mathf.Abs(pinchZoomDelta) > 0.0f)
            {
                HandlePinchZoom(pinchZoomDelta * pinchZoomSpeed);
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
