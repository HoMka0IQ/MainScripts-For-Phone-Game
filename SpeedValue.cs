using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedValue : MonoBehaviour
{
    public Vector3 pos, velocity;
   
    void Awake()
    {
        pos = transform.position;
    }

    void Update()
    {
        velocity = (transform.position - pos) / Time.deltaTime;
        pos = transform.position;
       
        
    }
}
