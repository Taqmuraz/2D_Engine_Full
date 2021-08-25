using System;
using System.Linq;
using System.Collections.Generic;

namespace _2DEngine
{
	public class CharacterAnimation : EightDirectionsAnimation
	{
		public bool loop { get; private set; }

		public CharacterAnimation(string rootFolder, bool loop = true) : base(GetAnimations(rootFolder))
		{
			this.loop = loop;
		}

		static IAnimatedImage[] GetAnimations(string rootFolder)
		{
			var files = System.IO.Directory.GetFiles(rootFolder);
			List<SingleImage>[] images = new List<SingleImage>[8];

			string indices = string.Empty;

			for (int i = 0; i < files.Length; i++)
			{
				var file = files[i];

				string indexString = new string(file.Reverse().Skip(4).Take(5).Reverse().ToArray());
				int index;
				int.TryParse(indexString, out index);
				indices += "\n" + indexString;
				index /= 10000;

				if (images[index] == null) images[index] = new List<SingleImage>();
				images[index].Add(new SingleImage(file));
			}
			IAnimatedImage[] animatedImages = new IAnimatedImage[8];

			for (int i = 0; i < animatedImages.Length; i++)
			{
				var array = images[i].ToArray();
				try { animatedImages[i] = array.Length == 1 ? new SingleAnimatedImage(array[0]) : (IAnimatedImage)new AnimatedImage(array); }
				catch { throw new Exception(indices); }
			}

			return animatedImages;
		}

		public void Reset()
		{
			for (int i = 0; i < animations.Length; i++)
			{
				animations[i].imageIndex = 0;
			}
		}
	}
}
