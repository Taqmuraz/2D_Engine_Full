using System.Collections.Generic;

namespace _2DEngine.Game
{
	public class SoldierController : PhysicsCharacterController
	{
		protected override int GetDirectionOffset()
		{
			return -1;
		}

		protected override string GetInitAnimation()
		{
			return "Idle";
		}

		protected override string GetAnimationsPath()
		{
			return "./Data/Textures/SR";
		}
		protected override CharacterAnimation CreateAnimation(string animationName)
		{
			return new CharacterAnimation(animationName, !animationName.StartsWith("Death"));
		}

		protected override Rect CreateWorldRect()
		{
			return Rect.FromCenterAndSize(transform.position + new Vector2(0f, 0.3f), new Vector2(1.75f, 1.75f));
		}

		protected override IEnumerable<Collider> CreateColliders()
		{
			var collider = gameObject.AddComponent<CircleCollider>();
			collider.radius = 0.15f;
			yield return collider;
		}
	}
}
