using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public Vector2 move;
    public Vector2 look;
    public bool sprint;
    public bool jump;
    
    void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }
    
    void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }

    void OnSprint(InputValue value)
    {
        sprint = value.isPressed;
    }

    void OnJump(InputValue value)
    {
        jump = value.isPressed;
    }

}
