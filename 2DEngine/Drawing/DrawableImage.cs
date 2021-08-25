using System.Drawing;

namespace _2DEngine
{

	public abstract class DrawableImage
	{
		protected Texture image { get; private set; }

		public DrawableImage(string imageFile)
		{
			image = Project.instance.mainPanel.LoadAndBindTexture(imageFile);
		}

		public abstract void Draw(IGraphics graphics, Rect rect);
	}
}
