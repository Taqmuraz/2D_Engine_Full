namespace _2DEngine
{
	public static partial class Extensions
	{
		public static IResolver Concat(this IResolver a, IResolver b)
		{
			return new CombineResolver(a, b);
		}
	}
}