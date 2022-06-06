using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayer : MonoBehaviour
{


    public VariableJoystick variableJoystick;

   

    
    public float SmoothRotate;

    public bool HorizontalR;
    public bool VerticalR;

    private void Start()
    {
       
        
    }

    void Update()
    {
        ControllPlayer();

    }
    void ControllPlayer()
    {
        Vector3 movement = new Vector3(variableJoystick.Direction.x, 0.0f, variableJoystick.Direction.y);






        //transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), SmoothRotate);
        }
    }




}