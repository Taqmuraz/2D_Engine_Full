namespace _2DEngine
{
	public sealed class TrueResolver : IResolver
	{
		private TrueResolver()
		{
		}

		public readonly static TrueResolver trueResolver = new TrueResolver();
		bool IResolver.resolving
		{
			get
			{
				return true;
			}
		}
	}
}