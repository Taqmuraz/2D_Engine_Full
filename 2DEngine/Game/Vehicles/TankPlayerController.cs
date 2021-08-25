namespace _2DEngine.Game
{
	public class TankPlayerController : TankController
	{
		int moveDirection;
		float force;
		int cameraSize;

		[BehaviourEvent]
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.A)) moveDirection--;
			if (Input.GetKeyDown(KeyCode.D)) moveDirection++;
			if (Input.GetKey(KeyCode.W)) force += 0.1f;
			if (Input.GetKey(KeyCode.S)) force -= 0.1f;

			force -= force * 0.5f * Time.deltaTime;

			force = force.Clamp(-5f, 5f);

			transform.rotation = moveDirection * 45f;
			Vector2 move = transform.forward;

			Vector2 look = Camera.ScreenToWorld(Input.mousePosition) - Camera.cameraPosition;

			moveVelocity = move * force;

			towerTransform.forward = look;
			animationSpeed = force.Abs() * 1.25f;

			Camera.cameraPosition = transform.position;

			if (Input.GetKeyDown(KeyCode.E)) cameraSize--;
			if (Input.GetKeyDown(KeyCode.Q)) cameraSize++;

			cameraSize = cameraSize.Clamp(0, 10);

			float c = 5f * Mathf.Pow(2f, cameraSize);
			Camera.cameraSizeWithAspect = new Vector2(c, c);

			if (force < 0) PlayBodyAnimation("T34_MoveBack");
			else if (force == 0) PlayBodyAnimation("T34_Body");
			else PlayBodyAnimation("T34_MoveFront");
		}

		protected override string GetModelName()
		{
			return "T34";
		}
	}
}
