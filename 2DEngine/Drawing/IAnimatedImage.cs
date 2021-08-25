using System.Drawing;

namespace _2DEngine
{
	public interface IAnimatedImage
	{
		int imageIndex { get; set; }
		int imagesNumber { get; }

		void Draw(IGraphics graphics, Rect rect);
	}
}