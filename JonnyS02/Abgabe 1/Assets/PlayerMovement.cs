using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;
    public Light flashLight;
    public bool hasFlashlight = false;

    private bool isLightOn = false;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private float currentWalkSpeed;
    private float currentRunSpeed;
    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentWalkSpeed = walkSpeed;
        currentRunSpeed = runSpeed;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? currentRunSpeed : currentWalkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? currentRunSpeed : currentWalkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight;
            currentWalkSpeed = crouchSpeed;
            currentRunSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            currentWalkSpeed = walkSpeed;
            currentRunSpeed = runSpeed;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F wurde gedrückt");

            if (hasFlashlight)
            {
                isLightOn = !isLightOn;
                flashLight.enabled = isLightOn;
                Debug.Log("Taschenlampe: " + (isLightOn ? "An" : "Aus"));
            }
            else
            {
                Debug.Log("Spieler hat die Taschenlampe noch nicht!");
            }
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    public void PickupFlashlight()
    {
        hasFlashlight = true;
        isLightOn = !isLightOn;
        flashLight.enabled = isLightOn;
        Debug.Log("Taschenlampe wurde eingesammelt – Licht jetzt verfügbar.");
    }
}
