namespace _2DEngine.Game
{
	public class Bullet : Renderer
	{
		const float BULLET_SPEED = 50f;

		protected override int queue => (int)Camera.WorldToScreen(transform.position).y;
		protected override bool isActive => true;

		public float endDistance { get; set; }
		public IDamagable target { get; set; }
		public float damage { get; set; }
		public Vector2 offset { get; set; }
		public float length { get; set; } = 0.5f;

		[BehaviourEvent]
		void Update()
		{
			float dist = Time.deltaTime * BULLET_SPEED;
			transform.position += transform.forward * dist;
			endDistance -= dist;

			if (endDistance <= dist)
			{
				if (target != null) target.BulletDamage(damage);
				gameObject.Destroy();
			}
		}

		protected override void Draw(IGraphics graphics)
		{
			var matrix = Camera.WorldToScreen();

			Vector2 pos = transform.position + new Vector2(0f, offset.y) + transform.right * offset.x;

			graphics.DrawLine(matrix.MultiplyPoint(pos), matrix.MultiplyPoint(pos + transform.forward * length), new Color32(1f, 1f, 0f, 1f));
		}
	}
}
