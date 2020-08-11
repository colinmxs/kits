using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
		[SerializeField] private float m_TossForce = 1000f;              // Amount of force added to the tossed object when the player tosses.
		[SerializeField] private float m_JumpForce = 400f;              // Amount of force added when the player jumps.
		[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;      // Amount of maxSpeed applied to crouching movement. 1 = 100%
		[Range(0, 1)] [SerializeField] private float m_SprintSpeed = 2f;      // Amount of maxSpeed applied to crouching movement. 1 = 100%
		[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
		[SerializeField] private bool m_AirControl = false;             // Whether or not a player can steer while jumping;
		[SerializeField] private LayerMask m_WhatIsGround;              // A mask determining what is ground to the character
		[SerializeField] private Transform m_GroundCheck;             // A position marking where to check if the player is grounded.
		[SerializeField] private Transform m_CeilingCheck;              // A position marking where to check for ceilings
		[SerializeField] private Transform m_PickupCheck;              // A position marking where to check for ceilings
		[SerializeField] private Collider2D m_CrouchDisableCollider;        // A collider that will be disabled when crouching
		
		const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
		private bool m_Grounded;            // Whether or not the player is grounded.
		const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
		private Rigidbody2D m_Rigidbody2D;
		private bool m_FacingRight = true;  // For determining which way the player is currently facing.
		private Vector3 m_Velocity = Vector3.zero;

		[Header("Events")]
		[Space]

		public UnityEvent OnLandEvent;

		[System.Serializable]
		public class BoolEvent : UnityEvent<bool> { }

		public BoolEvent OnCrouchEvent;
		private bool m_wasCrouching = false;

    private void Awake()
		{
				m_Rigidbody2D = GetComponent<Rigidbody2D>();

				if (OnLandEvent == null)
						OnLandEvent = new UnityEvent();

				if (OnCrouchEvent == null)
						OnCrouchEvent = new BoolEvent();
		}

		private void FixedUpdate()
		{				
				bool wasGrounded = m_Grounded;
				m_Grounded = false;

				// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
				// This can be done using layers instead but Sample Assets will not overwrite your project settings.
				Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
				for (int i = 0; i < colliders.Length; i++)
				{
						if (colliders[i].gameObject != gameObject)
						{
								m_Grounded = true;
								if (!wasGrounded)
                {
										OnLandEvent.Invoke();
										Debug.Log("grounded");
								}
						}
				}
		}

		public class MoveRequest
    {
				public float Move { get; set; }
				public bool Crouch { get; set; }
				public bool Jump { get; set; }
				public bool Sprint { get; set; }
    }
		public void Move(MoveRequest request)
		{
				// If crouching, check to see if the character can stand up
				if (!request.Crouch)
				{
						// If the character has a ceiling preventing them from standing up, keep them crouching
						if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
						{
								request.Crouch = true;
						}
				}

				//only control the player if grounded or airControl is turned on
				if (m_Grounded || m_AirControl)
				{						
						// If crouching
						if (request.Crouch)
						{
								if (!m_wasCrouching)
								{
										m_wasCrouching = true;
										OnCrouchEvent.Invoke(true);
								}

								// Reduce the speed by the crouchSpeed multiplier
								request.Move *= m_CrouchSpeed;

								// Disable one of the colliders when crouching
								if (m_CrouchDisableCollider != null)
										m_CrouchDisableCollider.enabled = false;
						}
						else
						{
								// Enable the collider when not crouching
								if (m_CrouchDisableCollider != null)
										m_CrouchDisableCollider.enabled = true;

								if (m_wasCrouching)
								{
										m_wasCrouching = false;
										OnCrouchEvent.Invoke(false);
								}

                if (request.Sprint)
								{ 	
										request.Move *= m_SprintSpeed;
                }
						}

						// Move the character by finding the target velocity
						Vector3 targetVelocity = new Vector2(request.Move * 10f, m_Rigidbody2D.velocity.y);
						// And then smoothing it out and applying it to the character
						m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

						// If the input is moving the player right and the player is facing left...
						if (request.Move > 0 && !m_FacingRight)
						{
								// ... flip the player.
								Flip();
						}
						// Otherwise if the input is moving the player left and the player is facing right...
						else if (request.Move < 0 && m_FacingRight)
						{
								// ... flip the player.
								Flip();
						}
				}
				// If the player should jump...
				if (m_Grounded && request.Jump)
				{
						// Add a vertical force to the player.
						m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
						m_Grounded = false;
				}				
		}

		public bool PickUp(GameObject obj)
    {
				Debug.Log("PickUp");
				obj.gameObject.transform.SetParent(m_PickupCheck, false);
				obj.gameObject.transform.localPosition = Vector3.zero;
        var rigidBody = obj.GetComponent<Rigidbody2D>();
        rigidBody.simulated = false;
        return true;
    }

		public bool Toss(GameObject obj, float vert)
    {
				Debug.Log("vert::" + vert);

				var yMod = vert;
				var xMod = 1f - Math.Abs(yMod);				

				obj.gameObject.transform.SetParent(null, true);
				var rigidBody = obj.GetComponent<Rigidbody2D>();
				rigidBody.velocity = m_Rigidbody2D.velocity;
				rigidBody.angularVelocity = 0f;
				rigidBody.simulated = true;
				rigidBody.AddForce(new Vector2(m_FacingRight ? m_TossForce*xMod : -1*m_TossForce*xMod, m_TossForce*yMod));
				return true;
		}


		private void Flip()
		{
				// Switch the way the player is labelled as facing.
				m_FacingRight = !m_FacingRight;

				// Multiply the player's x local scale by -1.
				Vector3 theScale = transform.localScale;
				theScale.x *= -1;
				transform.localScale = theScale;
		}
}