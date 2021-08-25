namespace _2DEngine.Game
{
	public sealed class Transform : Component
	{
		public Vector2 position { get; set; }
		public float rotation { get; set; }
		public Vector2 forward
		{
			get => new Vector2(Mathf.Cos(rotation), -Mathf.Sin(rotation));
			set
			{
				value = value.normalized;

				if (value.length == 0f) return;

				rotation = -Mathf.Round(value.ToAngle());
			}
		}
		public Vector2 right => new Vector2(Mathf.Cos(rotation + 90f), -Mathf.Sin(rotation + 90f));

		public Matrix4x4 localToWorld => Matrix4x4.CreateWorldMatrix(right, forward, Vector3.forward, position);

		public Vector2 TransformPoint(Vector2 point)
		{
			return position + forward * point.y + right * point.x;
		}
		public Vector2 TransformVector(Vector2 point)
		{
			return forward * point.y + right * point.x;
		}
	}
}
