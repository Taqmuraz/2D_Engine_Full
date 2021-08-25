using System.Collections.Generic;
using System.Linq;

namespace _2DEngine.Game
{
	public abstract class PhysicsCharacterController : CharacterController
	{
		protected Vector2 moveVelocity { get; set; }
		protected ReadOnlyArray<Collider> colliders { get; private set; }

		protected abstract IEnumerable<Collider> CreateColliders();

		[BehaviourEvent]
		void Start()
		{
			colliders = CreateColliders().ToArray();
		}
		[BehaviourEvent]
		void Update()
		{
			Vector2 summ = new Vector2();

			for (int c = 0; c < colliders.Length; c++)
			{
				if (colliders[c].HasCollision(out Collision collision))
				{
					for (int i = 0; i < collision.contancsCount; i++)
					{
						summ += collision[i].hitNormal;
					}
					//Debug.DrawRay(transform.position, summ.normalized, new Color32(1f, 1f, 1f, 1f));
					//Debug.DrawRay(transform.position, total.normalized, new Color32(1f, 1f, 0f, 1f));
				}
			}

			Vector2 total = (summ.normalized + moveVelocity.normalized) * Time.deltaTime * moveVelocity.length;

			transform.position += total;
		}
	}
}
