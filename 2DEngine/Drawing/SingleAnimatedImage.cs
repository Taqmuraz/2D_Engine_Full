namespace _2DEngine
{
	public class SingleAnimatedImage : IAnimatedImage
	{
		SingleImage image;

		public SingleAnimatedImage(SingleImage image)
		{
			this.image = image;
		}

		public int imageIndex { get => 0; set { return; } }
		public int imagesNumber => 1;

		public void Draw(IGraphics graphics, Rect rect)
		{
			image.Draw(graphics, rect);
		}
	}
}
