using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class RobotController : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _lookAction;
    private InputAction _sprintAction;

    [SerializeField] private float moveSpeed = 8;
    [SerializeField] private float sprintSpeed = 16;
    [SerializeField] private GameObject mainCam;
    [SerializeField] private Transform cameraFollowTarget;
    [SerializeField] private float lookSensitivity = 0.06f;
    [SerializeField] private float jumpHeight = 10.0f;
    [SerializeField] private Transform groundCheck;  // Ground check object
    [SerializeField] private float groundDistance = 0.1f;  // Ground check radius
    [SerializeField] private LayerMask groundMask;  // Ground layer

    private float xRotation, yRotation;
    private float currentSpeed = 0;
    private float speedSmoothVelocity;
    [SerializeField] private float speedSmoothTime = 0.1f;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float gravityValue = -20f;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _lookAction = _playerInput.actions["Look"];
        _sprintAction = _playerInput.actions["Sprint"];

        xRotation = 0;
        yRotation = 0;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        groundedPlayer = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 inputMove = _moveAction.ReadValue<Vector2>();
        Vector3 inputDirection = new Vector3(inputMove.x, 0, inputMove.y);
        float targetRotation = 0;
        float targetSpeed = 0;

        if (inputMove != Vector2.zero)
        {
            bool isSprinting = _sprintAction.IsPressed();
            targetSpeed = isSprinting ? sprintSpeed : moveSpeed;
            targetRotation = Quaternion.LookRotation(inputDirection).eulerAngles.y + mainCam.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20 * Time.deltaTime);
        }

        // Speed interpolation
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        // Update animation speed
        float animationSpeed = (inputMove != Vector2.zero) ? currentSpeed / sprintSpeed * 6 : 0;
        _animator.SetFloat("Speed", animationSpeed);

        Vector3 targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;

        // Move character
        if (inputMove != Vector2.zero)
        {
            _controller.Move(targetDirection * currentSpeed * Time.deltaTime);
        }

        // Check for jump input
        if (groundedPlayer && _jumpAction.triggered)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        _controller.Move(playerVelocity * Time.deltaTime);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    void CameraRotation()
    {
        Vector2 inputLook = _lookAction.ReadValue<Vector2>();
        xRotation -= inputLook.y * lookSensitivity;
        yRotation += inputLook.x * lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -70, 70);
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        cameraFollowTarget.rotation = rotation;
    }
}
