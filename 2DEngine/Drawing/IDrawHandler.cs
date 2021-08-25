namespace _2DEngine
{
	public interface IDrawHandler
	{
		int queue { get; }
		bool isActive { get; }
		void Draw(IGraphics graphics);
	}
}
