namespace _2DEngine.Game
{
	public static class Random
	{
		static System.Random random = new System.Random();

		public static float Range(float min, float max)
		{
			return (float)(min + random.NextDouble() * (max - min));
		}
		public static int Range(int min, int max)
		{
			return random.Next(min, max);
		}
	}

	public class SoldierBOTController : SoldierController
	{

	}

	public class SoldierPlayerController : SoldierController
	{
		[BehaviourEvent]
		void Update()
		{
			Vector2 lookDir = Input.GetWASD().normalized;

			if (Input.GetKeyDown(KeyCode.E)) Camera.cameraSize *= 0.5f;
			if (Input.GetKeyDown(KeyCode.Q)) Camera.cameraSize *= 2f;

			transform.forward = lookDir;

			moveVelocity = lookDir * 1.5f;
			Camera.cameraPosition = transform.position;

			string anim = "Idle";
			if (Input.GetKey(KeyCode.MouseR))
			{
				if (lookDir.length != 0) anim = "Walk_Weapon";
				else anim = "Idle_Weapon";
			}
			else if (lookDir.length != 0) anim = "Walk";

			animator.Play(anim);
		}
	}
}
