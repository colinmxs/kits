﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
		public PlayerController controller;
		public Animator animator;


		public float runSpeed = 40f;

		float horizontalMove = 0f;
		bool jump = false;
		bool crouch = false;

		// Update is called once per frame
		void Update()
		{
				horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

				if (horizontalMove != 0) animator.SetBool("IsRunning", true);
				else animator.SetBool("IsRunning", false);

				if (Input.GetButtonDown("Jump"))
				{
						animator.SetTrigger("Jump");
						jump = true;
				}

				if (Input.GetButtonDown("Crouch"))
				{
						crouch = true;
				}
				else if (Input.GetButtonUp("Crouch"))
				{
						crouch = false;
				}

		}

		void FixedUpdate()
		{
				// Move our character
				controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
				jump = false;
		}
}