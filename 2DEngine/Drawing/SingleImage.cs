using System.Drawing;

namespace _2DEngine
{
	public class SingleImage : DrawableImage
	{
		public SingleImage(string imageFile) : base(imageFile)
		{
		}

		public override void Draw(IGraphics graphics, Rect rect)
		{
			graphics.DrawImage(image, rect);
		}
	}
}
