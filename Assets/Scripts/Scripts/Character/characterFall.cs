﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterFall : MonoBehaviour
{
    private CharacterController characterController;
    public float fallSpeed;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }


    void Update()
    {
        Vector3 movement = Vector3.zero;

        if (!characterController.isGrounded)
            movement.y += Physics.gravity.y;
        else
            enabled = false;
        
        characterController.Move(movement.normalized * fallSpeed * Time.deltaTime);
    }
}