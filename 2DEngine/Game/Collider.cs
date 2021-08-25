using System.Collections.Generic;

namespace _2DEngine.Game
{
	public struct ContactData
	{
		public Vector2 hitPoint { get; set; }
		public Vector2 hitNormal { get; set; }
		public Collider otherCollider { get; set; }
	}
	public class Collision
	{
		List<ContactData> contacts;

		public ContactData this[int index]
		{
			get
			{
				if (contacts == null) throw new System.IndexOutOfRangeException();
				else return contacts[index];
			}
		}
		public int contancsCount => contacts == null ? 0 : contacts.Count;

		public void AddContact(ContactData contact)
		{
			if (contacts == null) contacts = new List<ContactData>();

			contacts.Add(contact);
		}
	}

	public abstract class Collider : Component
	{
		static List<Collider> colliders = new List<Collider>();
		static Dictionary<Collider, Collision> collisions = new Dictionary<Collider, Collision>();

		static readonly Collision emptyCollision = new Collision();

		[BehaviourEvent]
		void Start()
		{
			colliders.Add(this);
		}
		[BehaviourEvent]
		void OnDestroy()
		{
			colliders.Remove(this);
		}

		static Collider()
		{
			nonAllocColliders = new Collider[150];
		}

		static Collider[] nonAllocColliders;

		public static void UpdatePhysics()
		{
			int collidersCount = colliders.Count;
			collisions.Clear();

			if (nonAllocColliders == null) return;

			for (int i = 0; i < nonAllocColliders.Length; i++)
			{
				if (i < collidersCount) nonAllocColliders[i] = colliders[i];
				else nonAllocColliders[i] = null;
			}

			for (int i = 0; i < collidersCount; i++)
			{
				Collision collision = new Collision();
				var collider = nonAllocColliders[i];

				for (int j = i + 1; j < collidersCount; j++)
				{
					var other = nonAllocColliders[j];

					if (collider.gameObject != other.gameObject && collider.GetBounds().IntersectsWith(other.GetBounds()))
					{
						other.AddContactsWith(collider, collision);
					}
				}

				if (collision.contancsCount > 0) collisions.Add(collider, collision);
			}
		}

		public abstract Rect GetBounds();

		public bool HasCollision(out Collision data)
		{
			if (collisions.ContainsKey(this))
			{
				data = collisions[this];
				return true;
			}
			else
			{
				data = emptyCollision;
				return false;
			}
		}

		protected static bool Intersection_Circle_Circle(CircleCollider a, CircleCollider b, Collision data)
		{
			float minDist2 = a.radius + b.radius;
			minDist2 *= minDist2;
			Vector2 dist = b.transform.TransformPoint(b.center) - a.transform.TransformPoint(a.center);
			Vector2 dist2 = dist * dist;

			if (minDist2 >= (dist2.x + dist2.y))
			{
				var contact = new ContactData();
				contact.hitNormal = dist.normalized;
				contact.hitPoint = b.transform.TransformPoint(contact.hitNormal * b.radius);
				data.AddContact(contact);
				return true;
			}
			else return false;
		}

		protected abstract bool AddContactsWith(Collider collider, Collision data);
	}
}
