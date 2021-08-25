
namespace _2DEngine
{
	public interface ITimedResolver : IResolver
	{
		void Reset();
		void Reset(float time);
		void SetLocked(bool locked);
		bool isLocked { get; }
		float readyProgress { get; }
	}
}
