using System.Collections.Generic;

namespace _2DEngine.Game
{
	public class TankPlayerController : TankController
	{
		int moveDirection;
		float force;

		[BehaviourEvent]
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.A)) moveDirection--;
			if (Input.GetKeyDown(KeyCode.D)) moveDirection++;
			if (Input.GetKey(KeyCode.W)) force += 0.1f;
			if (Input.GetKey(KeyCode.S)) force -= 0.1f;

			force -= force * 0.5f * Time.deltaTime;

			force = force.Clamp(-5f, 5f);

			transform.transformDirection = moveDirection;
			Vector2 move = transform.forward;

			Vector2 look = Camera.ScreenToWorld(Input.mousePosition) - Camera.cameraPosition;

			moveVelocity = move * force;

			towerTransform.forward = look;
			animationSpeed = force.Abs() * 1.25f;

			Camera.cameraPosition = transform.position;

			if (Input.GetKeyDown(KeyCode.E)) Camera.cameraSize *= 0.5f;
			if (Input.GetKeyDown(KeyCode.Q)) Camera.cameraSize *= 2f;

			if (force < 0) PlayBodyAnimation("T34_MoveBack");
			else if (force == 0) PlayBodyAnimation("T34_Body");
			else PlayBodyAnimation("T34_MoveFront");
		}
	}

	public class TankController : Component
	{
		class TankBodyController : PhysicsCharacterController
		{
			public new Vector2 moveVelocity { get => base.moveVelocity; set => base.moveVelocity = value; }

			protected override string GetAnimationsPath()
			{
				return "./Data/Textures/T34_Body";
			}

			protected override Rect CreateWorldRect()
			{
				return Rect.FromCenterAndSize(transform.position + new Vector2(0f, 0.5f), new Vector2(5f, 5f));
			}

			protected override string GetInitAnimation()
			{
				return "T34_Body";
			}

			protected override int GetDirectionOffset()
			{
				return -2;
			}
			public void Play(string animation)
			{
				animator.Play(animation);
			}

			protected override IEnumerable<Collider> CreateColliders()
			{
				for (int i = 0; i < 5; i++)
				{
					var c = gameObject.AddComponent<CircleCollider>();
					c.radius = 0.8f;
					c.center = new Vector2(0f, (i - 1) * -0.25f);
					yield return c;
				}
			}
		}
		class TankTowerController : CharacterController
		{
			protected override string GetAnimationsPath()
			{
				return "./Data/Textures/T34_Tower";
			}

			protected override Rect CreateWorldRect()
			{
				return Rect.FromCenterAndSize(transform.position + new Vector2(0f, 0.5f), new Vector2(5f, 5f));
			}

			protected override string GetInitAnimation()
			{
				return "T34_Tower";
			}

			protected override int GetDirectionOffset()
			{
				return -2;
			}
		}

		protected Transform towerTransform => tower.transform;

		TankBodyController body;
		TankTowerController tower;

		protected float animationSpeed { get => body.animationSpeed; set => body.animationSpeed = value; }
		protected Vector2 moveVelocity { get => body.moveVelocity; set => body.moveVelocity = value; }

		protected void PlayBodyAnimation(string animation)
		{
			body.Play(animation);
		}


		[BehaviourEvent]
		void OnPreRender()
		{
			towerTransform.position = transform.position;
		}

		[BehaviourEvent]
		void Start()
		{
			body = gameObject.AddComponent<TankBodyController>();
			tower = new GameObject(name + "_Tower").AddComponent<TankTowerController>();
		}
	}
}
