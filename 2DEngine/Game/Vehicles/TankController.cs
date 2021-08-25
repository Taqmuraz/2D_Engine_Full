using System.Collections.Generic;

namespace _2DEngine.Game
{

	public abstract class TankController : Component
	{
		protected abstract string GetModelName();

		class TankBodyController : PhysicsCharacterController
		{
			public new Vector2 moveVelocity { get => base.moveVelocity; set => base.moveVelocity = value; }

			protected override string GetAnimationsPath()
			{
				return $"./Data/Textures/{(name.Contains("SR") ? "T34" : "Pantera")}_Body";
			}

			protected override Rect CreateWorldRect()
			{
				return Rect.FromCenterAndSize(transform.position + new Vector2(0f, 1f), new Vector2(5f, 5f));
			}

			protected override string GetInitAnimation()
			{
				return $"{(name.Contains("SR") ? "T34" : "Pantera")}_Body";
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
				var box = gameObject.AddComponent<BoxCollider>();
				box.size = new Vector2(2f, 3f);
				box.center = new Vector2(0f, -0.25f);
				yield return box;
			}
		}
		class TankTowerController : CharacterController
		{
			ITimedResolver fireResolver = new TimedResolver(5f);

			protected override string GetAnimationsPath()
			{
				return $"./Data/Textures/{(name.Contains("SR") ? "T34" : "Pantera")}_Tower";
			}

			protected override Rect CreateWorldRect()
			{
				return Rect.FromCenterAndSize(transform.position + new Vector2(0f, 1f), new Vector2(5f, 5f));
			}

			protected override string GetInitAnimation()
			{
				return $"{(name.Contains("SR") ? "T34" : "Pantera")}_Tower";
			}

			protected override int GetDirectionOffset()
			{
				return -2;
			}

			public void Fire()
			{
				if (fireResolver.resolving)
				{
					Bullet bullet = new GameObject("Tank bullet").AddComponent<Bullet>();
					bullet.damage = 75f;
					bullet.endDistance = 1000f;
					bullet.length = 1f;

					Ray ray;
					if (Collider.Raycast(ray = new Ray(transform.position, transform.forward), out RaycastHit hit))
					{
						bullet.target = hit.collider.gameObject.GetComponent<IDamagable>();
						bullet.endDistance = hit.distance;
					}

					bullet.transform.position = ray.origin + ray.direction * 2f;
					bullet.transform.forward = ray.direction;

					fireResolver.Reset();
				}
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

		protected void Fire()
		{
			tower.Fire();
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
