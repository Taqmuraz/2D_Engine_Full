using System;
using System.Linq;

namespace _2DEngine
{
	public static partial class Extensions
	{
		public static string ExtractAnimationDirection(this string fileName, string folder, out int number)
		{
			number = ExtractNumber(fileName);
			return new string(fileName.Replace(folder, string.Empty).Replace($"_strip{number}", string.Empty).Replace(".png", string.Empty).SkipWhile(c => c != '_').Skip(1).SkipWhile(c => c != '_').Skip(1).ToArray());
		}
		public static int ExtractNumber(this string fileName)
		{
			int number;
			int.TryParse(new string(fileName.Reverse().SkipWhile(c => !char.IsDigit(c)).TakeWhile(c => char.IsDigit(c)).Reverse().ToArray()), out number);
			if (number <= 0) throw new ArgumentException($"Number of animations can't be {number}");
			return number;
		}

		public static int Loop(this int value, int range)
		{
			if (value < 0) value = ((value % range) + range) % range;
			else if (value >= range) value %= range;
			return value;
		}
	}
}
