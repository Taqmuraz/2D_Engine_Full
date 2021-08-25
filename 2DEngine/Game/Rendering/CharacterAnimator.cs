namespace _2DEngine.Game
{
	public sealed class CharacterAnimator : Renderer
	{
		SafeDictionary<string, CharacterAnimation> animations = new SafeDictionary<string, CharacterAnimation>();
		CharacterAnimation activeAnimation;

		public Rect characterRect { get; set; }
		public int characterDirection { get; set; }
		protected override int queue => (int)characterRect.center.y;
		protected override bool isActive => activeAnimation != null;
		public float animationSpeed { get; set; } = 1;
		float localTime;

		public const float FRAMES_PER_SECOND = 30f;

		public void AddAnimation(string name, CharacterAnimation animation)
		{
			animations[name] = animation;
		}

		public void Play(string name)
		{
			var last = activeAnimation;
			activeAnimation = animations[name];
			if (last != activeAnimation && activeAnimation != null)
			{
				Reset();
			}
		}

		void Reset()
		{
			activeAnimation.Reset();
			localTime = 0f;
		}

		[BehaviourEvent]
		void Update()
		{
			if (activeAnimation == null) return;
			activeAnimation.animationIndex = characterDirection;
			var image = activeAnimation.GetImage();

			localTime += Time.deltaTime * animationSpeed * FRAMES_PER_SECOND;
			if (!activeAnimation.loop) localTime = localTime.Clamp(0f, image.imagesNumber - 1f);
		}

		protected override void Draw(IGraphics graphics)
		{
			if (activeAnimation == null) return;

			activeAnimation.animationIndex = characterDirection;
			var image = activeAnimation.GetImage();
			image.Draw(graphics, characterRect);
			image.imageIndex = (int)localTime;
		}
	}
}
