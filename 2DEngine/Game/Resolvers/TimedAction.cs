namespace _2DEngine
{
	public sealed class TimedAction : TimedResolver
	{
		private readonly System.Action action;

		public TimedAction(System.Action action, float deltaTime) : base(deltaTime)
		{
			this.action = action;
		}

		public bool TryInvoke()
		{
			if (resolving)
			{
				Reset();
				if (action != null) action();
				return true;
			}
			return false;
		}
	}
}