

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float WalkSpeed = 6f;
    [SerializeField] float gravity = -13.0f;

    [SerializeField] float verticalSpeed = -1f;

    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.3f;

    [SerializeField] bool lockCursor = true;

    float velocityY = 0.0f;

    CharacterController controller;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    public VariableJoystick variableJoystick;
    
    public Animator Animator;
    public SpeedValue SpeedValue;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        UpdateMovement();

        //if (SpeedValue.velocity.magnitude > 0.1f)
        //{
        //    Animator.SetBool("Walk", true);
        //}
        //else
        //{
        //    Animator.SetBool("Walk", false);
        //}
        //Animator.SetFloat("Speed", SpeedValue.velocity.magnitude / 15);
    }

    void UpdateMovement()
    {
        Vector2 targenDir = new Vector2(variableJoystick.Direction.x, variableJoystick.Direction.y);

        currentDir = Vector2.SmoothDamp(currentDir, targenDir, ref currentDirVelocity, moveSmoothTime);

        if (controller.isGrounded)
        {
            velocityY = 0.0f;
        }
        velocityY += gravity * Time.deltaTime;
        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * WalkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
    }
}
