namespace _2DEngine.Game
{
	public sealed class CursorRenderer : Renderer
	{
		protected override void Draw(IGraphics graphics)
		{
			var rect = Rect.FromCenterAndSize(Input.mousePosition, new Vector2(25f, 25f));
			graphics.DrawCircle(rect, new Color32(1f, 1f, 0f, 1f));
		}

		protected override int queue => int.MaxValue;
		protected override bool isActive => true;
	}
}
