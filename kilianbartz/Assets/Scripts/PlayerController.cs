using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Fly mode parameters
    public float moveSpeed = 10f;
    public float fastMultiplier = 2f;
    public float sensitivity = 2f;
    public float zoomSpeed = 5f;

    // Walk mode parameters
    public float walkSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float pushForce = 15f;

    private float _currentSpeed;
    private float _rotationY;
    private float _rotationX;
    private bool _isFlyMode = false;
    private Vector3 _velocity;
    private CharacterController _controller;

    [Header("Flashlight Settings")]
    public Light flashlight;
    public KeyCode toggleKey = KeyCode.F;
    public bool startWithFlashlightOn = true;
    public AudioSource foodstepAudio;

    public Camera MinimapCamera;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Initialize flashlight
        if (flashlight != null)
        {
            flashlight.enabled = startWithFlashlightOn;
        }
    }

    void Update()
    {
        if (GameManager.Instance.gameState != GameState.Playing)
        {
            return;
        }
        HandleModeToggle();

        if (_isFlyMode)
        {
            HandleFlyMode();
        }
        else
        {
            HandleWalkMode();
        }
        HandleFlashlight();
        if (MinimapCamera != null)
        {
            MinimapCamera.transform.position = transform.position + new Vector3(0, 6, 0);
            MinimapCamera.transform.rotation = Quaternion.Euler(90, transform.eulerAngles.y, 0);
        }
    }
    public void EnableControls(bool enable)
    {
        enabled = enable;
    }
    void HandleFlashlight()
    {
        if (Input.GetKeyDown(toggleKey) && flashlight != null)
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }

    void HandleModeToggle()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            _isFlyMode = !_isFlyMode;
            Cursor.lockState = _isFlyMode ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = _isFlyMode;

            if (!_isFlyMode)
            {
                _velocity = Vector3.zero;
            }
        }
    }

    void HandleFlyMode()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButton(1))
        {
            HandleRotation();
            HandleFlyMovement();
        }

        HandleSpeed();
    }

    void HandleWalkMode()
    {
        HandleRotation();
        HandleWalkMovement();
        HandleJump();
    }

    void HandleRotation()
    {
        _rotationX += Input.GetAxis("Mouse X") * sensitivity;
        _rotationY -= Input.GetAxis("Mouse Y") * sensitivity;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        transform.rotation = Quaternion.Euler(_rotationY, _rotationX, 0f);
    }

    void HandleFlyMovement()
    {
        Vector3 move = Vector3.zero;
        _currentSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * fastMultiplier : moveSpeed;

        if (Input.GetKey(KeyCode.W)) move += transform.forward;
        if (Input.GetKey(KeyCode.S)) move -= transform.forward;
        if (Input.GetKey(KeyCode.D)) move += transform.right;
        if (Input.GetKey(KeyCode.A)) move -= transform.right;
        if (Input.GetKey(KeyCode.E)) move += Vector3.up;
        if (Input.GetKey(KeyCode.Q)) move -= Vector3.up;

        if (move != Vector3.zero)
        {
            transform.position += move.normalized * _currentSpeed * Time.deltaTime;
        }
    }

    void HandleWalkMovement()
    {
        if (_controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move.y = 0;

        _controller.Move(move.normalized * walkSpeed * Time.deltaTime);

        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
        if (move != Vector3.zero)
        {
            foodstepAudio.enabled = true;
        }
        else
        {
            foodstepAudio.enabled = false;
        }
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && _controller.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void HandleSpeed()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        moveSpeed = Mathf.Clamp(moveSpeed + scroll * zoomSpeed, 1f, 100f);
    }

    // Send ball flying
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ball"))
        {
            Rigidbody ballRb = hit.gameObject.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                // Push the ball in the direction the player is moving
                Vector3 force = hit.moveDirection * pushForce;
                ballRb.AddForce(force, ForceMode.Impulse);
            }
        }
    }
}