namespace _2DEngine.Game
{
	public abstract class CharacterController : Component
	{
		public float animationSpeed
		{
			get => animator.animationSpeed;
			set => animator.animationSpeed = value;
		}

		protected CharacterAnimator animator { get; private set; }

		protected virtual void OnPreLoadAnimations()
		{

		}
		
		[BehaviourEvent]
		void Start()
		{
			animator = gameObject.AddComponent<CharacterAnimator>();

			OnPreLoadAnimations();

			string animationsFolder = GetAnimationsPath();
			string[] animations = System.IO.Directory.GetDirectories(animationsFolder);
			for (int i = 0; i < animations.Length; i++)
			{
				var name = new System.IO.DirectoryInfo(animations[i]).Name;
				animator.AddAnimation(name, CreateAnimation(name, animations[i]));
			}
			animator.Play(GetInitAnimation());
		}

		[BehaviourEvent]
		void OnPreRender()
		{
			animator.characterDirection = Mathf.Round(transform.rotation / 45f) + GetDirectionOffset();
			animator.characterRect = Camera.WorldToScreen(CreateWorldRect()).MultiplyMatrixSize(Camera.worldCurvature);
		}

		protected abstract int GetDirectionOffset();

		protected abstract string GetAnimationsPath();
		protected virtual CharacterAnimation CreateAnimation(string animationName, string folderName)
		{
			return new CharacterAnimation(folderName, false);
		}
		protected abstract Rect CreateWorldRect();

		protected abstract string GetInitAnimation();
	}
}
