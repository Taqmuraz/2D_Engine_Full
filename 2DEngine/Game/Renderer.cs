namespace _2DEngine.Game
{
	public abstract class Renderer : Component, IDrawHandler
	{
		protected abstract int queue { get; }
		protected abstract bool isActive { get; }

		[BehaviourEvent]
		void Start()
		{
			Project.instance.mainPanel.AddDrawHandler(this);
		}
		[BehaviourEvent]
		void OnDestroy()
		{
			Project.instance.mainPanel.RemoveDrawHandler(this);
		}

		int IDrawHandler.queue => queue;
		bool IDrawHandler.isActive => isActive;

		void IDrawHandler.Draw(IGraphics graphics)
		{
			Draw(graphics);
		}

		protected abstract void Draw(IGraphics graphics);
	}
}
