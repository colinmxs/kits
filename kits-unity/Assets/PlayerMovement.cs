﻿using System;
using UnityEngine;
using static PlayerController;

public class PlayerMovement : MonoBehaviour
{
		public PlayerController controller;
		public Animator animator;


		public float runSpeed = 40f;

		float horizontalMove = 0f;
		bool jump = false;

		private GameObject focus;

    internal bool Focus(GameObject gameObject)
    {
				focus = gameObject;
				return true;
    }

		internal bool LoseFocus(GameObject gameObject)
    {
				if(focus == gameObject)
        {
						focus = null;
						return true;
				}
				return false;
		}

    bool crouch = false;
		bool sprint = false;
    GameObject pickup = null;

		// Update is called once per frame
		void Update()
		{
				horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
				var vert = Input.GetAxisRaw("Vertical");

				if (horizontalMove != 0) animator.SetBool("IsRunning", true);
				else animator.SetBool("IsRunning", false);

				if (Input.GetButtonDown("Jump"))
				{
						animator.SetBool("IsJumping", true);
						jump = true;
				}

				if (Input.GetAxisRaw("Sprint") > 0 || Input.GetButtonDown("Sprint")) sprint = true;
				else if (Input.GetButtonUp("Sprint")) sprint = false;
				else if (sprint && Input.GetAxisRaw("Sprint") == 0) sprint = false;

				if (Input.GetButtonDown("Crouch"))
				{
						crouch = true;
				}
				else if (Input.GetButtonUp("Crouch"))
				{
						crouch = false;
				}

				if (Input.GetButtonDown("Pick Up") && focus != null && pickup == null)
				{
						var pickedUp = controller.PickUp(focus);
            if (pickedUp)
            {
								pickup = focus;
						}
				}
        else if (Input.GetButtonDown("Toss") && pickup != null)
        {

            var tossed = controller.Toss(pickup, vert);

            if (tossed)
            {
                pickup = null;
            }
        }
    }

		public void SetIsJumping(bool isJumping)
    {
				animator.SetBool("IsJumping", isJumping);
    }

		void FixedUpdate()
		{
				// Move our character
				var moveRequest = new MoveRequest
				{
						Move = horizontalMove * Time.fixedDeltaTime,
						Crouch = crouch,
						Jump = jump,
						Sprint = sprint
				};

				controller.Move(moveRequest);
				jump = false;
		}
}
