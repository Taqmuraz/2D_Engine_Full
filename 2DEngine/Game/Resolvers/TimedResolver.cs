using _2DEngine.Game;

namespace _2DEngine
{
	public class TimedResolver : ITimedResolver
	{
		private float lastInvokeTime;
		private float deltaTime;

		public TimedResolver(float deltaTime)
		{
			this.deltaTime = deltaTime;
			lastInvokeTime = Time.time;
		}

		public void Reset()
		{
			lastInvokeTime = Time.time;
			isLocked = false;
		}

		public void Reset(float newDeltaTime)
		{
			deltaTime = newDeltaTime;
			Reset();
		}

		public bool resolving
		{
			get
			{
				return !isLocked && (lastInvokeTime + deltaTime < Time.time);
			}
		}

		public float readyProgress
		{
			get
			{
				if (isLocked) return 0;
				else return (Time.time - lastInvokeTime) / deltaTime;
			}
		}

		public bool isLocked { get; private set; }

		public void SetLocked(bool locked)
		{
			isLocked = locked;
		}
	}
}
