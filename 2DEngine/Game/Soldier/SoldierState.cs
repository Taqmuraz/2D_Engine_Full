namespace _2DEngine.Game
{
	public partial class SoldierController
	{
		public abstract class SoldierState : State, IInitializable<SoldierController>
		{
			public BehaviourEventsHandler stateHandler { get; set; }
			public virtual float visibility => 1f;
			public virtual float animationSpeed => 1f;
			public virtual Vector2 weaponOffset => new Vector2();
			protected StateMachine stateMachine { get; private set; }
			protected SoldierController soldier { get; private set; }
			string name;

			public SoldierState()
			{
				name = CreateName();
			}

			protected virtual string CreateName()
			{
				return GetType().Name.Replace("Soldier", string.Empty).Replace("State", string.Empty);
			}

			protected override void OnEventCall(string name)
			{
				base.OnEventCall(name);
				if (stateHandler != null) stateHandler.CallEvent(name);
			}

			void IInitializable<SoldierController>.Initialize(SoldierController soldier)
			{
				this.soldier = soldier;
				stateMachine = soldier.stateMachine;
			}

			[BehaviourEvent]
			void Update()
			{
				soldier.animator.Play(GetAnimationName());
			}

			protected virtual string GetAnimationName()
			{
				return name;
			}

			public override string GetName()
			{
				return name;
			}
		}

		public abstract class SoldierMoveStateBase : SoldierState
		{
			public abstract float moveSpeed { get; }
		}

		protected class SoldierIdleState : SoldierMoveStateBase
		{
			public override float moveSpeed => 2.5f;
			public override float animationSpeed => 1f;

			protected sealed override string GetAnimationName()
			{
				return soldier.moveVelocity.length > Mathf.Epsilon ? "Walk" : "Idle";
			}
		}
		protected class SoldierWeaponState : SoldierMoveStateBase
		{
			public override float moveSpeed => 3.5f;
			public override float animationSpeed => 1.5f;
			public override Vector2 weaponOffset => new Vector2(0.125f, 0.8f);

			protected sealed override string GetAnimationName()
			{
				return soldier.moveVelocity.length > Mathf.Epsilon ? "Walk_Weapon" : "Idle_Weapon";
			}
		}
		public class SoldierDeathState : SoldierState
		{
			string animationName;
			[BehaviourEvent]
			void OnEnter()
			{
				foreach (var cld in soldier.colliders) cld.Destroy();
				animationName = "Death_" + Random.Range(0, 3).ToString();

				soldier.moveVelocity = Vector2.zero;
			}
			protected override string GetAnimationName()
			{
				return animationName;
			}
		}
		protected class SoldierCrouchState : SoldierMoveStateBase
		{
			public override float moveSpeed => 1.5f;
			public override float animationSpeed => 1f;
			public override float visibility => 0.5f;

			protected sealed override string GetAnimationName()
			{
				return soldier.moveVelocity.length > Mathf.Epsilon ? "Crouch_Walk" : "Crouch_Idle";
			}
		}
		protected class SoldierCrouchWeaponState : SoldierMoveStateBase
		{
			public override float moveSpeed => 1.5f;
			public override float animationSpeed => 1f;
			public override Vector2 weaponOffset => new Vector2(0.125f, 0.4f);
			public override float visibility => 0.5f;

			protected sealed override string GetAnimationName()
			{
				return soldier.moveVelocity.length > Mathf.Epsilon ? "Crouch_Walk_Weapon" : "Crouch_Idle_Weapon";
			}
		}
	}
}
