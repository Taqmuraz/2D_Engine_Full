namespace _2DEngine
{
	public class AnimatedImage : IAnimatedImage
	{
		SingleImage[] images;

		int m_imageIndex;

		public AnimatedImage(SingleImage[] images)
		{
			this.images = images;
		}

		public int imageIndex
		{
			get => m_imageIndex;
			set
			{
				m_imageIndex = value.Loop(imagesNumber);
			}
		}
		public int imagesNumber => images.Length;

		public void Draw(IGraphics graphics, Rect rect)
		{
			try
			{
				images[imageIndex].Draw(graphics, rect);
			}
			catch
			{
				throw new System.Exception($"{imageIndex}/{imagesNumber}");
			}
		}
	}
}
