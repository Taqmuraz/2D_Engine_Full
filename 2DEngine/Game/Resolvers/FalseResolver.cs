namespace _2DEngine
{
	public sealed class FalseResolver : IResolver
	{
		private FalseResolver()
		{
		}

		public readonly static FalseResolver falseResolver = new FalseResolver();
		bool IResolver.resolving
		{
			get
			{
				return false;
			}
		}
	}
}