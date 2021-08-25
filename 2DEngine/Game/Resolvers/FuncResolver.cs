namespace _2DEngine
{
	public sealed class FuncResolver : IResolver
	{
		private System.Func<bool> func;
		public FuncResolver(System.Func<bool> func)
		{
			this.func = func;
		}

		bool IResolver.resolving { get { return func(); } }
	}
}
