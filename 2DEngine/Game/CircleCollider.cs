namespace _2DEngine.Game
{
	public sealed class CircleCollider : Collider
	{
		public Vector2 center { get; set; }
		public float radius { get; set; }

		public override Rect GetBounds()
		{
			return Rect.FromCenterAndSize(transform.TransformPoint(center), new Vector2(radius * 2f, radius * 2f));
		}

		protected override bool AddContactsWith(Collider collider, Collision data)
		{
			if (collider is CircleCollider circle) return Intersection_Circle_Circle(this, circle, data);
			else throw new System.NotImplementedException();
		}
	}
}
