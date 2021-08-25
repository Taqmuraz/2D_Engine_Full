using System.Drawing;

namespace _2DEngine
{
	public class SlicedHorizontalImage : DrawableImage, IAnimatedImage
	{
		int m_imageIndex;
		public int imageIndex
		{
			get => m_imageIndex;
			set
			{
				m_imageIndex = value.Loop(imagesNumber);
			}
		}
		public int imagesNumber { get; private set; }

		public SlicedHorizontalImage(string imageFile, int animationsNumber) : base(imageFile)
		{
			this.imagesNumber = animationsNumber;
		}

		public override void Draw(IGraphics graphics, Rect rect)
		{
			float step = image.width / imagesNumber;

			graphics.DrawImage(image, rect, new Rect(step * imageIndex, 0f, step, image.height));
		}
	}
}
