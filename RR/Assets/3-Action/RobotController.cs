using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotController : MonoBehaviour
{
    private PlayerInputManager input;
    private CharacterController _controller;
    private Animator _animator;

    [SerializeField] private float moveSpeed = 8;
    [SerializeField] private float sprintSpeed = 16;
    [SerializeField] private GameObject mainCam;
    [SerializeField] private Transform cameraFollowTarget;
    [SerializeField] private float lookSensitivity = 0.06f;
    private float xRotation, yRotation;
    
    private float currentSpeed = 0;
    private float speedSmoothVelocity;
    [SerializeField] private float speedSmoothTime = 0.1f;

    void Start()
    {
        input = GetComponent<PlayerInputManager>();
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        xRotation = 0;
        yRotation = 0;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 inputDirection = new Vector3(input.move.x, 0, input.move.y);
        float targetRotation = 0;
        float targetSpeed = 0;

        if (input.move != Vector2.zero)
        {
            targetSpeed = input.sprint ? sprintSpeed : moveSpeed;
            targetRotation = Quaternion.LookRotation(inputDirection).eulerAngles.y + mainCam.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20 * Time.deltaTime);
        }

        // Speed interpolation
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        // Update animation speed
        float animationSpeed = (input.move != Vector2.zero) ? currentSpeed / sprintSpeed * 6 : 0;
        _animator.SetFloat("Speed", animationSpeed);

        Vector3 targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;

        // Move character
        if (input.move != Vector2.zero)
        {
            _controller.Move(targetDirection * currentSpeed * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    void CameraRotation()
    {
        xRotation -= input.look.y * lookSensitivity;
        yRotation += input.look.x * lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -70, 70);
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        cameraFollowTarget.rotation = rotation;
    }
}
