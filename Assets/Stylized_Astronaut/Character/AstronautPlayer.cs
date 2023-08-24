using UnityEngine;
using System.Collections;

namespace AstronautPlayer
{
    public class AstronautPlayer : MonoBehaviour
    {
        private Animator anim;
        private CharacterController controller;

        public float speed = 60.0f;
        public float turnSpeed = 400.0f;
        private Vector3 moveDirection = Vector3.zero;
        public float gravity = 20.0f;
        public float jumpForce = 8.0f;
        private bool canJump = false;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            anim = gameObject.GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (Input.GetKey("w"))
            {
                anim.SetInteger("AnimationPar", 1);
            }
            else
            {
                anim.SetInteger("AnimationPar", 0);
            }

            if (controller.isGrounded)
            {
                moveDirection = transform.forward * Input.GetAxisRaw("Vertical") * speed;
                canJump = true;
            }
            else
            {
                canJump = false;
            }

            float turn = Input.GetAxisRaw("Horizontal");
            transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
            controller.Move(moveDirection * Time.deltaTime);
            moveDirection.y -= gravity * Time.deltaTime;

            if (canJump && Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }

        private void Jump()
        {
            moveDirection.y = jumpForce;
        }
    }
}