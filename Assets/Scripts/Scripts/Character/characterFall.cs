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
            movement.y += -9.8f;
        else
        {
            SoundManager.PlayOneShot(SoundManager.FallingObject, this.transform.position);
            enabled = false;
        }
        
        characterController.Move(movement.normalized * fallSpeed * Time.deltaTime);
    }
}