namespace _2DEngine
{
	public class Texture
	{
		public Texture(int textureIndex, int width, int height)
		{
			this.textureIndex = textureIndex;
			this.width = width;
			this.height = height;
		}

		public int textureIndex { get; private set; }
		public int width { get; private set; }
		public int height { get; private set; }
	}
}
