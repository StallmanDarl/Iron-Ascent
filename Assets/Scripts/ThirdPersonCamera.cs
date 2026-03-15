using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform target;

    [Header("Camera Settings")]
    public float distance = 5f;         // Max camera distance
    public float height = 2f;           // Pivot height above player
    public float mouseSensitivity = 3f;
    public float followSpeed = 10f;     // How fast pivot follows player
    public float collisionBuffer = 0.2f; 
    public float cameraRadius = 0.3f;   // SphereCast radius

    [Header("Collision")]
    public LayerMask cameraCollisionMask;

    private float currentX;
    private float currentY = 15f;
    private Vector3 cameraPivot;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cameraPivot = target.position; // Initial pivot at player position
    }

    void Update()
    {
        // Mouse input
        currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
        currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        currentY = Mathf.Clamp(currentY, -25f, 60f);
    }

    void LateUpdate()
    {
        // Smoothly follow player
        cameraPivot = Vector3.Lerp(cameraPivot, target.position, followSpeed * Time.deltaTime);
        Vector3 pivot = cameraPivot + Vector3.up * height;

        // Rotation based on mouse
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = pivot + rotation * new Vector3(0, 0, -distance);
        Vector3 direction = desiredPosition - pivot;

        // SphereCast for collision
        RaycastHit hit;
        float targetDistance = distance;

        if (Physics.SphereCast(pivot, cameraRadius, direction.normalized, out hit, distance, cameraCollisionMask))
        {
            // Clamp camera to hit distance but not behind player
            targetDistance = Mathf.Clamp(hit.distance - (collisionBuffer + cameraRadius), 0.1f, distance);
        }

        // Smooth distance adjustment (optional: prevents popping)
        float smoothDistance = Mathf.Lerp(Vector3.Distance(transform.position, pivot), targetDistance, followSpeed * Time.deltaTime);

        // Apply final camera position
        transform.position = pivot + direction.normalized * smoothDistance;

        // Look at the pivot
        transform.LookAt(pivot);
    }
}