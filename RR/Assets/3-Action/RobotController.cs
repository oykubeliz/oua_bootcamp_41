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

    [SerializeField] private float moveSpeed = 4;
    [SerializeField] private GameObject mainCam;
    [SerializeField] private Transform cameraFollowTarget;
    [SerializeField] private float lookSensitivity = 0.06f; // Kamera hareket hızı için sensitivity değişkeni
    private float xRotation, yRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInputManager>();
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        // Kamera rotasyonunu sıfırla
        xRotation = 0;
        yRotation = 0;

        // Mouse işaretçisini gizle ve kilitle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 0;
        Vector3 inputDirection = new Vector3(input.move.x, 0, input.move.y);
        float targetRotation = 0;
        if (input.move != Vector2.zero)
        {
            speed = moveSpeed;
            targetRotation = Quaternion.LookRotation(inputDirection).eulerAngles.y + mainCam.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 20 * Time.deltaTime);
        }
        _animator.SetFloat("Speed", input.move.magnitude);
        Vector3 targetDirection = Quaternion.Euler(0, targetRotation, 0) * Vector3.forward;
        _controller.Move(targetDirection * speed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    void CameraRotation()
    {
        xRotation -= input.look.y * lookSensitivity; // Mouse yukarı hareket ettiğinde kamerayı yukarı bakacak şekilde değiştirildi
        yRotation += input.look.x * lookSensitivity; // Mouse sağa/sola hareketi aynı şekilde kaldı
        xRotation = Mathf.Clamp(xRotation, -70, 70);
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        cameraFollowTarget.rotation = rotation;
    }
}
