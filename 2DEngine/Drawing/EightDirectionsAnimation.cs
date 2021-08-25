using System;

namespace _2DEngine
{

	public class EightDirectionsAnimation
	{
		int m_animationIndex;
		int lastIndex;

		public int animationIndex
		{
			get => m_animationIndex;
			set
			{
				m_animationIndex = value.Loop(animationsNumber);
				lastIndex = value;
				OnAnimationIndexChanged();
			}
		}

		protected int animationsNumber => animations.Length;

		protected readonly IAnimatedImage[] animations;

		public EightDirectionsAnimation(IAnimatedImage[] animations)
		{
			this.animations = animations;
		}

		public virtual void OnAnimationIndexChanged()
		{

		}

		public IAnimatedImage GetImage()
		{
			try
			{
				return animations[animationIndex];
			}
			catch
			{
				throw new Exception($"{lastIndex}/{animationsNumber}");
			}
		}
	}
}
