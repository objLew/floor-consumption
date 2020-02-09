﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 0.0f;

    private float disableTime = 5;
    private float collisionDisableTimer = 0;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        //so doesn't move when colliding. Box collider acts as trigger
        characterController.detectCollisions = false;   
        //Physics.IgnoreLayerCollision(7, 10, false);

    }

    void Update()
    {

        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);


        //Acts as timer to check collision so it turns it on every x seconds to check for a new collision
        collisionDisableTimer -= 0.5f * Time.deltaTime;
        print(collisionDisableTimer);

        if(collisionDisableTimer <= 0)
        {
            Physics.IgnoreLayerCollision(8, 9, false);
            print("Are collisions between 8 and 9 being ignored?   " + Physics.GetIgnoreLayerCollision(8, 9));

        }
    }

    //Detect collisions between the GameObjects with Colliders attached
    void OnControllerColliderHit(ControllerColliderHit collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.layer == 9)
        {
            Physics.IgnoreLayerCollision(8, 9, true);
            print("Are collisions between 8 and 9 being ignored?   " + Physics.GetIgnoreLayerCollision(8, 9));
            

            collisionDisableTimer = disableTime;
        }
    }
}
