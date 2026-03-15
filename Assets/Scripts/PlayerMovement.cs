using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float acceleration = 30f;
    public float deceleration = 60f;
    public float rotationSpeed = 10f;

    [Header("Jump")]
    public float jumpForce = 7f;
    public float fallMultiplier = 2.5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.25f;
    public LayerMask groundLayer;

    bool isGrounded;
    bool isJumping;
    bool controlLocked;

    float jumpLockTimer = 0f;
    public float minAirTime = 0.15f;

    Vector3 jumpMomentum;

    [Header("Dodge")]
    public float dodgeDistance = 6f;
    public float dodgeDuration = 0.35f;
    public float dodgeCooldown = 1f;
    public AnimationCurve dodgeCurve; // Curve: X=0..1 (time), Y=0..1 (speed multiplier)

    bool dodgeBuffered;
    float dodgeBufferTime = 0.2f;
    float dodgeBufferTimer;
    float lastDodgeTime;

    bool isDodging = false;

    [Header("Combat")]
    public bool isLockedOn = false;

    [Header("References")]
    public Rigidbody rb;
    public Transform cameraTransform;

    float moveX;
    float moveZ;
    bool isSprinting;

    Vector3 currentVelocity;

    void Update()
    {
        if (isDodging) return;

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        isSprinting = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            dodgeBuffered = true;
            dodgeBufferTimer = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping)
        {
            Jump();
        }

        if (dodgeBuffered)
        {
            if (Time.time - dodgeBufferTimer > dodgeBufferTime)
                dodgeBuffered = false;
            else
                TryDodge();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
            isLockedOn = !isLockedOn;
    }

    void FixedUpdate()
    {
        if (jumpLockTimer > 0f)
        {
            jumpLockTimer -= Time.fixedDeltaTime;
        }

        CheckGround();

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

        if (!isDodging)
        {
            if (controlLocked)
            {
                // Maintain stored horizontal momentum while airborne
                rb.linearVelocity = new Vector3(
                    jumpMomentum.x,
                    rb.linearVelocity.y,
                    jumpMomentum.z
                );
            }
            else
            {
                MovePlayer();
                RotatePlayer();
            }
        }
    }

    void MovePlayer()
    {
        float speed = isSprinting ? sprintSpeed : moveSpeed;

        Vector3 direction = GetCameraRelativeDirection();
        Vector3 targetVelocity = direction.normalized * speed;

        if (direction.magnitude < 0.1f)
        {
            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                Vector3.zero,
                deceleration * Time.fixedDeltaTime
            );
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                targetVelocity,
                acceleration * Time.fixedDeltaTime
            );
        }

        rb.linearVelocity = new Vector3(
            currentVelocity.x,
            rb.linearVelocity.y,
            currentVelocity.z
        );
    }

    void RotatePlayer()
    {
        Vector3 direction = GetCameraRelativeDirection();

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void TryDodge()
    {
        if (Time.time - lastDodgeTime < dodgeCooldown) return;

        Vector3 direction = transform.forward;

        StartCoroutine(Dodge(direction));

        lastDodgeTime = Time.time;
        dodgeBuffered = false;
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (isGrounded && isJumping && jumpLockTimer <= 0f)
        {
            isJumping = false;
            controlLocked = false;
        }
    }

    void Jump()
    {
        isJumping = true;
        controlLocked = true;
        jumpLockTimer = minAirTime;

        // Store horizontal momentum at takeoff
        jumpMomentum = new Vector3(
            rb.linearVelocity.x,
            0f,
            rb.linearVelocity.z
        );

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    IEnumerator Dodge(Vector3 direction)
    {  
        isDodging = true;

        // Store current horizontal momentum (for air dodges)
        Vector3 momentum = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.linearVelocity = Vector3.zero;
        currentVelocity = Vector3.zero;

        Quaternion dodgeRotation = transform.rotation;

        float elapsed = 0f;

        Vector3 startPos = transform.position;

        // Detect collisions in dodge path
        Vector3 capsuleTop = transform.position + Vector3.up * 0.9f;
        Vector3 capsuleBottom = transform.position + Vector3.up * 0.1f;
        float capsuleRadius = 0.4f;
        float dodgeDistanceAdjusted = dodgeDistance;

        RaycastHit hit;
        if (Physics.CapsuleCast(
            capsuleTop,
            capsuleBottom,
            capsuleRadius,
            direction,
            out hit,
            dodgeDistance,
            ~0,
            QueryTriggerInteraction.Ignore))
        {
            dodgeDistanceAdjusted = Mathf.Max(hit.distance - 0.05f, 0f);
        }

        // Use an animation curve for smooth roll (fast start, slow end)
        AnimationCurve dodgeCurve = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(0.2f, 0.5f),
            new Keyframe(0.6f, 0.9f),
            new Keyframe(1f, 1f)
        );

        Vector3 dodgeStartPos = transform.position;
        Vector3 dodgeEndPos = dodgeStartPos + direction.normalized * dodgeDistanceAdjusted;

        // Store current Y for air dodge
        float fixedY = transform.position.y;

        while (elapsed < dodgeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dodgeDuration);
            float curveT = dodgeCurve.Evaluate(t);

            // Calculate position along curve
            Vector3 targetPos = Vector3.Lerp(dodgeStartPos, dodgeEndPos, curveT);

            // Maintain Y-position if midair
            if (!isGrounded)
                targetPos.y = fixedY;

            rb.MovePosition(targetPos);

            transform.rotation = dodgeRotation;

            yield return null;
        }

        // After dodge, restore horizontal momentum (for air dash)
        if (!isGrounded)
        {
            rb.linearVelocity = new Vector3(momentum.x, rb.linearVelocity.y, momentum.z);
        }

        isDodging = false;
    }

    Vector3 GetCameraRelativeDirection()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        return camForward * moveZ + camRight * moveX;
    }
}