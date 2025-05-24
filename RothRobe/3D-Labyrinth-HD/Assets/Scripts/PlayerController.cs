using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float rotationY = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleMovement()
    {
        // Bewegung im lokalen Raum des Spielers
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        rotationY += lookInput.x * mouseSensitivity;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }

    // Diese Methoden werden vom PlayerInput-Component automatisch aufgerufen
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
            moveInput = context.ReadValue<Vector2>();
        else if (context.canceled)
            moveInput = Vector2.zero;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
            lookInput = context.ReadValue<Vector2>();
        else if (context.canceled)
            lookInput = Vector2.zero;
    }
}