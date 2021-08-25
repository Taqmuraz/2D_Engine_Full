namespace _2DEngine.Game
{
	public sealed class HealthbarRenderer : Renderer
	{
		protected override int queue => int.MaxValue;
		protected override bool isActive => true;

		public float normalizedHealth { get; set; } = 1f;

		protected override void Draw(IGraphics graphics)
		{
			if (normalizedHealth <= 0f || normalizedHealth >= 1f) return;

			Matrix4x4 w2s = Camera.WorldToScreen();
			Vector2 a = w2s.MultiplyPoint(transform.position + new Vector2(-0.5f, -0.95f));
			Vector2 b = w2s.MultiplyPoint(transform.position + new Vector2(0.5f, -0.95f));
			Vector2 c = w2s.MultiplyPoint(transform.position + new Vector2(normalizedHealth.Clamp(0f, 1f) - 0.5f, -0.95f));
			graphics.DrawLine(a, b, Color32.red);
			graphics.DrawLine(a, c, Color32.green);
		}
	}
}
