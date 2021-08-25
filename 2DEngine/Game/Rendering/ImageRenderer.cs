namespace _2DEngine.Game
{
	public sealed class ImageRenderer : Renderer
	{
		public SingleImage image { get; set; }
		public Rect worldRect { get; set; }
		protected override int queue => (int)Camera.WorldToScreen(worldRect.center).y;
		protected override bool isActive => true;

		protected override void Draw(IGraphics graphics)
		{
			if (image != null)
			{
				image.Draw(graphics, Camera.WorldToScreen(worldRect));
			}
		}
	}
}
