using UnityEngine;
using Unity.Cinemachine;

[ExecuteAlways]
public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform target;

    [Header("Camera Settings")]
    public float distance = 5f;
    public float height = 2f;
    public float mouseSensitivity = 3f;
    public float followSpeed = 10f;
    public float collisionBuffer = 0.2f;
    public float cameraRadius = 0.3f;
    public float minPitch = -25f;
    public float maxPitch = 60f;

    [Header("Collision")]
    public LayerMask cameraCollisionMask;

    float currentX;
    float currentY = 15f;
    Vector3 cameraPivot;
    float currentDistance;

    CinemachineCamera virtualCamera;
    CinemachineBrain brain;

    void Awake()
    {
        EnsureRig();
        cameraPivot = target != null ? target.position : transform.position;

        if (currentDistance <= 0f)
        {
            currentDistance = distance;
        }
    }

    void Start()
    {
        if (Application.isPlaying)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void OnValidate()
    {
        distance = Mathf.Max(0.5f, distance);
        cameraRadius = Mathf.Max(0.05f, cameraRadius);
        followSpeed = Mathf.Max(0.01f, followSpeed);
        EnsureRig();
        SyncRig();
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
            currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            currentY = Mathf.Clamp(currentY, minPitch, maxPitch);
        }
    }

    void LateUpdate()
    {
        EnsureRig();
        SyncRig();
    }

    void EnsureRig()
    {
        if (target == null)
        {
            return;
        }

        brain = GetComponent<CinemachineBrain>();
        if (brain == null)
        {
            brain = gameObject.AddComponent<CinemachineBrain>();
        }

        Transform rigTransform = transform.Find("CM Orbit Camera");
        if (rigTransform == null)
        {
            GameObject rigObject = new GameObject("CM Orbit Camera");
            rigTransform = rigObject.transform;
            rigTransform.SetParent(transform, false);
        }

        virtualCamera = rigTransform.GetComponent<CinemachineCamera>();
        if (virtualCamera == null)
        {
            virtualCamera = rigTransform.gameObject.AddComponent<CinemachineCamera>();
        }

        virtualCamera.Priority.Value = 100;
    }

    void SyncRig()
    {
        if (virtualCamera == null || target == null)
        {
            return;
        }

        cameraPivot = Vector3.Lerp(cameraPivot, target.position, followSpeed * Time.deltaTime);
        Vector3 pivot = cameraPivot + Vector3.up * height;
        Quaternion orbitRotation = Quaternion.Euler(currentY, currentX, 0f);
        Vector3 desiredDirection = orbitRotation * Vector3.back;

        float targetDistance = distance;
        RaycastHit hit;
        if (Physics.SphereCast(pivot, cameraRadius, desiredDirection, out hit, distance, cameraCollisionMask))
        {
            targetDistance = Mathf.Clamp(hit.distance - (collisionBuffer + cameraRadius), 0.15f, distance);
        }

        if (currentDistance <= 0f)
        {
            currentDistance = targetDistance;
        }

        float damping = Application.isPlaying ? Time.deltaTime * followSpeed : 1f;
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, damping);

        Vector3 finalPosition = pivot + desiredDirection.normalized * currentDistance;
        Quaternion finalRotation = Quaternion.LookRotation(pivot - finalPosition, Vector3.up);

        virtualCamera.transform.SetPositionAndRotation(finalPosition, finalRotation);
    }
}
