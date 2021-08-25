namespace _2DEngine
{

	public class CombineResolver : IResolver
	{
		private IResolver a, b;

		public CombineResolver(IResolver a, IResolver b)
		{
			this.a = a;
			this.b = b;
		}

		bool IResolver.resolving { get { return a.resolving && b.resolving; } }
	}
}