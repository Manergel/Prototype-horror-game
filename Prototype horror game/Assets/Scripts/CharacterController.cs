using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 6.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float sphereRadius = 0.1f;
    [SerializeField] private float sphereOffset = 0.1f;

    [SerializeField] private Transform cameraTransform;

    private bool isJumping = false;
    private bool isRunning = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        bool jump = Input.GetButtonDown("Jump");
        bool run = Input.GetKey(KeyCode.LeftShift);

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement.Normalize();

        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        Vector3 movementDirection = cameraForward * movement.z + cameraTransform.right * movement.x;

        float speed = isRunning ? runSpeed : walkSpeed;
        movementDirection *= speed;

        rb.velocity = new Vector3(movementDirection.x, rb.velocity.y, movementDirection.z);

        if (jump && !isJumping)
        {
            if (CheckGrounded())
            {
                rb.AddForce(new Vector3(0.0f, jumpForce, 0.0f), ForceMode.Impulse);
                isJumping = true;
            }
        }

        if (run && !isRunning)
        {
            isRunning = true;
        }
        else if (!run && isRunning)
        {
            isRunning = false;
        }
    }

    private bool CheckGrounded()
    {
        Vector3 sphereCenter = transform.position + Vector3.down * sphereOffset;
        Collider[] colliders = Physics.OverlapSphere(sphereCenter, sphereRadius);
        return colliders.Length > 1;
    }
    void LateUpdate()
    {
        isJumping = false;
    }

    void OnDrawGizmos()
    {
        Vector3 sphereCenter = transform.position + Vector3.down * sphereOffset;
    }
}