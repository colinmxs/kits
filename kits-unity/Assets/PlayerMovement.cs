using UnityEngine;
using static PlayerController;

public class PlayerMovement : MonoBehaviour
{
		public PlayerController controller;

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

		// Update is called once per frame
		void Update()
		{
				horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
				var vert = Input.GetAxisRaw("Vertical");

				if (Input.GetButtonDown("Jump")) jump = true;

				if (Input.GetAxisRaw("Sprint") > 0 || Input.GetButtonDown("Sprint")) sprint = true;
				
				if (Input.GetButton("Crouch")) crouch = true;

				if (Input.GetButtonDown("Toss")) controller.Toss(vert);

				if (Input.GetButtonDown("Pick Up") && focus != null) controller.PickUp(focus);
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
				sprint = false;
				crouch = false;
		}
}
