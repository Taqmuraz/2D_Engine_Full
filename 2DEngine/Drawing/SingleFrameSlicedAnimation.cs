namespace _2DEngine
{
	public class SingleFrameSlicedAnimation : EightDirectionsAnimation
	{
		SlicedHorizontalImage image;

		public SingleFrameSlicedAnimation(string fileName) : base(GetImages(fileName))
		{
			this.image = GetImage() as SlicedHorizontalImage;
		}

		static SlicedHorizontalImage[] GetImages(string fileName)
		{
			var image = new SlicedHorizontalImage(fileName, fileName.ExtractNumber());
			var images = new SlicedHorizontalImage[8];
			for (int i = 0; i < 8; i++) images[i] = image;
			return images;
		}

		public override void OnAnimationIndexChanged()
		{
			image.imageIndex = animationIndex;
		}
	}
}
