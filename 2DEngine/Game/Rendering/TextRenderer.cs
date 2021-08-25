namespace _2DEngine.Game
{
	public sealed class TextRenderer : Renderer
	{
		protected override int queue => int.MaxValue;
		protected override bool isActive => true;

		public string text { get; set; } = "New Text";
		public Color32 color { get; set; } = new Color32(1f, 1f, 1f, 1f);
		public Vector2 screenSpacePosition { get;  set; }

		protected override void Draw(IGraphics graphics)
		{
			graphics.DrawString(screenSpacePosition, text, color);
		}
	}
}
