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
    private InputAction _attackAction; // Attack input action

    [SerializeField] private float moveSpeed = 8;
    [SerializeField] private float sprintSpeed = 16;
    [SerializeField] private GameObject mainCam;
    [SerializeField] private Transform cameraFollowTarget;
    [SerializeField] private float lookSensitivity = 0.06f;
    [SerializeField] private float jumpHeight = 10.0f;
    [SerializeField] private Transform groundCheck;  // Ground check object
    [SerializeField] private Vector3 groundCheckSize = new Vector3(0.5f, 0.1f, 0.5f);  // Ground check size
    [SerializeField] private LayerMask groundMask;  // Ground layer

    private float xRotation, yRotation;
    private float currentSpeed = 0;
    private float speedSmoothVelocity;
    [SerializeField] private float speedSmoothTime = 0.1f;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float gravityValue = -20f;
    private bool isDead = false; // Variable to check if the player is dead

    private bool isJumping = false;
    private bool isFalling = false;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _lookAction = _playerInput.actions["Look"];
        _sprintAction = _playerInput.actions["Sprint"];
        _attackAction = _playerInput.actions["Attack"]; // Attack input action

        xRotation = 0;
        yRotation = 0;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        groundedPlayer = Physics.CheckBox(groundCheck.position, groundCheckSize / 2, Quaternion.identity, groundMask);
        bool isJumpLanding = _animator.GetCurrentAnimatorStateInfo(0).IsName("JumpLand");

        Vector3 slopeNormal;
        bool isOnSlope = IsOnSlope(out slopeNormal);

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            _animator.SetBool("Grounded", true);
            _animator.SetBool("FreeFall", false);
            _animator.SetBool("Jump", false);
            isJumping = false;
            isFalling = false;
        }
        else if (!groundedPlayer && !isOnSlope)
        {
            _animator.SetBool("Grounded", false);
        }

        Vector2 inputMove = _moveAction.ReadValue<Vector2>();
        Vector3 inputDirection = new Vector3(inputMove.x, 0, inputMove.y);
        float targetRotation = 0;
        float targetSpeed = 0;

        if (inputMove != Vector2.zero && !isJumpLanding)
        {
            bool isSprinting = _sprintAction.IsPressed();
            targetSpeed = isSprinting ? sprintSpeed : moveSpeed;
            targetRotation = Quaternion.LookRotation(inputDirection).eulerAngles.y + mainCam.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20 * Time.deltaTime);
        }

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        float animationSpeed = (inputMove != Vector2.zero) ? currentSpeed / sprintSpeed * 6 : 0;
        _animator.SetFloat("Speed", animationSpeed);

        Vector3 targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;

        if (inputMove != Vector2.zero && !isJumpLanding)
        {
            if (isOnSlope)
            {
                targetDirection = Vector3.ProjectOnPlane(targetDirection, slopeNormal).normalized;
                _controller.Move(targetDirection * (currentSpeed * 0.75f) * Time.deltaTime); // Speed is reduced slightly on slopes for better control
            }
            else
            {
                _controller.Move(targetDirection * currentSpeed * Time.deltaTime);
            }
        }

        if (groundedPlayer && _jumpAction.triggered)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravityValue);
            _animator.SetBool("Jump", true);
            isJumping = true;
        }

        if (!groundedPlayer && !isOnSlope && !isJumping)
        {
            _animator.SetBool("FreeFall", true);
            isFalling = true;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        _controller.Move(playerVelocity * Time.deltaTime);

        if (_attackAction.triggered)
        {
            _animator.SetTrigger("Attack");
        }
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

    // Function to handle death
    public void Die()
    {
        isDead = true;
        _animator.SetTrigger("Die");
        _controller.enabled = false; // Disable character controller to stop movement
    }

    // OnTriggerEnter function to detect collision with DMG object
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DMG"))
        {
            Debug.Log("Temas Etti");
            Die();
        }
    }
    
    private bool IsOnSlope(out Vector3 slopeNormal)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _controller.height / 2 * 1.5f, groundMask))
        {
            slopeNormal = hit.normal;
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            Debug.Log("Slope Angle: " + slopeAngle);
            return slopeAngle > 0 && slopeAngle <= _controller.slopeLimit;
        }
        slopeNormal = Vector3.up;
        Debug.Log("No slope detected");
        return false;
    }
}
