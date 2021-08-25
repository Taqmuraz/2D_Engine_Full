using System;
using System.Collections.Generic;
using System.Linq;

namespace _2DEngine
{
	public class CharacterSlicedAnimation : EightDirectionsAnimation
	{
		public CharacterSlicedAnimation(string folder) : base(GetCharacterAnimations(folder))
		{
		}

		static List<string> postfixOrder = new List<string>
		{
			"Down",
			"DownL",
			"Left",
			"UpL",
			"Up",
			"UpR",
			"Right",
			"DownR",
		};

		static SlicedHorizontalImage[] GetCharacterAnimations(string folder)
		{
			string[] files = System.IO.Directory.GetFiles(folder);

			if (files.Length == 0) throw new ArgumentException($"There are no files in folder {folder}");

			SlicedHorizontalImage[] images = new SlicedHorizontalImage[files.Length];

			bool byIndex = files.Any(f => postfixOrder.Any(p => f.Contains(p)));

			for (int i = 0; i < images.Length; i++)
			{

				int number;
				var dir = files[i].ExtractAnimationDirection(folder, out number);
				images[byIndex ? postfixOrder.IndexOf(dir) : i] = new SlicedHorizontalImage(files[i], number);
			}
			return images;
		}
	}
}
