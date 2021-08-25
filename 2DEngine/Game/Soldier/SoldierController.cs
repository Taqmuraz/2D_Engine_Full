using System.Collections.Generic;
using System.Linq;

namespace _2DEngine.Game
{
	public enum SoldierSide
	{
		SR,
		NG
	}
	public partial class SoldierController : PhysicsCharacterController, IDamagable, ILocatable
	{
		static AudioPlayer[] bulletHit = { new AudioPlayer("BloodHit_0.mp3", 0.25f), new AudioPlayer("BloodHit_1.mp3", 0.25f), new AudioPlayer("BloodHit_2.mp3", 0.25f) };

		public float visibility => (stateMachine.activeState as SoldierState).visibility;
		public Vector2 weaponOffset => (stateMachine.activeState as SoldierState).weaponOffset;
		protected WeaponController weapon { get; private set; }
		protected StateMachine stateMachine { get; private set; }
		public SoldierSide soldierSide { get; private set; }

		public static QuadTree<SoldierController> soldiersQuadTree { get; private set; }
		bool quadTreeUsed;

		public bool isAlive => health > 0f;

		[BehaviourEvent]
		void EarlyUpdate()
		{
			if (isAlive)
			{
				if (soldiersQuadTree == null) CreateTree();
				soldiersQuadTree.AddLocatable(this);
				quadTreeUsed = true;
			}
		}

		void CreateTree()
		{
			soldiersQuadTree = new QuadTree<SoldierController>(Rect.FromCenterAndSize(Camera.cameraPosition, Camera.cameraSizeWithAspect), 1, 0, 0);
		}

		[BehaviourEvent]
		void LateUpdate()
		{
			if (isAlive && quadTreeUsed)
			{
				CreateTree();
				quadTreeUsed = false;
			}
		}

		float healthMax = 100f;
		float m_health = 100f;
		protected float health
		{
			get
			{
				return m_health;
			}
			private set
			{
				healthMax = Mathf.Max(healthMax, value);
				m_health = value;
				healthbarRenderer.normalizedHealth = health / healthMax;
			}
		}
		HealthbarRenderer healthbarRenderer;

		protected virtual WeaponController CreateWeapon()
		{
			return soldierSide == SoldierSide.SR ? new WeaponController(this, "PPSH", 71, 1000f / 60f, 1, 25f, 5f) : new WeaponController(this, "SMAISSER", 32, 10f, 0.5f, 15f, 2.5f);
		}

		protected override int GetDirectionOffset()
		{
			return -1;
		}

		protected override string GetInitAnimation()
		{
			return "Idle";
		}

		protected override void OnPreLoadAnimations()
		{
			soldierSide = name.EndsWith("SR") ? SoldierSide.SR : SoldierSide.NG;
		}

		protected override string GetAnimationsPath()
		{
			return "./Data/Textures/" + soldierSide.ToString();
		}
		protected override CharacterAnimation CreateAnimation(string animationName, string folderName)
		{
			return new CharacterAnimation(folderName, !animationName.StartsWith("Death"));
		}

		protected override Rect CreateWorldRect()
		{
			return Rect.FromCenterAndSize(transform.position + new Vector2(0f, 0.3f), new Vector2(3f, 3f));
		}

		protected override IEnumerable<Collider> CreateColliders()
		{
			var collider = gameObject.AddComponent<CircleCollider>();
			collider.radius = 0.5f;
			yield return collider;
		}

		IEnumerable<SoldierState> CreateStates()
		{
			yield return new SoldierIdleState();
			yield return new SoldierWeaponState();
			yield return new SoldierDeathState();
		}

		protected virtual IEnumerable<SoldierState> OverrideStates()
		{
			yield break;
		}

		[BehaviourEvent]
		void Start()
		{
			weapon = CreateWeapon();
			healthbarRenderer = gameObject.AddComponent<HealthbarRenderer>();

			SafeDictionary<string, SoldierState> states = new SafeDictionary<string, SoldierState>();

			foreach (var state in CreateStates().Concat(OverrideStates())) states[state.GetName()] = state;
			stateMachine = new SoldierStateMachine(states.Select(s => s.Value));
			foreach (var state in states)
			{
				(state.Value as IInitializable<SoldierController>).Initialize(this);
			}
			stateMachine.Initialize();
		}

		[BehaviourEvent]
		void Update()
		{
			animationSpeed = (stateMachine.activeState as SoldierState).animationSpeed * 0.5f;
			if (isAlive) health = (health + Time.deltaTime * 10f).Clamp(0f, 100f);
		}

		protected override void OnEventCall(string name)
		{
			base.OnEventCall(name);
			if (stateMachine != null && stateMachine.activeState != null) stateMachine.activeState.CallEvent(name);
		}

		protected void ApplyDamage(float damage)
		{
			if (health > 0f)
			{
				health -= damage;
				if (health <= 0f)
				{
					stateMachine.MoveToState("Death");
				}
			}
		}
		void IDamagable.BulletDamage(float damage)
		{
			ApplyDamage(damage);
			bulletHit[Random.Range(0, bulletHit.Length)].Play();
		}

		bool ILocatable.IntersectsRect(Rect rect)
		{
			return rect.Contains(transform.position);
		}
	}
}
