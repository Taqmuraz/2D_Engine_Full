using System.Collections.Generic;

namespace _2DEngine.Game
{
	public class Soldier_Protect_BOTController : SoldierBOTController
	{
		public static List<SoldierController> protectors { get; set; } = new List<SoldierController>();

		protected override IEnumerable<SoldierState> OverrideStates()
		{
			yield return new ProtectIdle();
			yield return new ProtectWeapon();
			yield return new ProtectDeath();
		}

		[BehaviourEvent]
		void Start()
		{
			protectors.Add(this);
		}
		[BehaviourEvent]
		void OnDestroy()
		{
			if (protectors.Count != 0) protectors.Clear();
			AI_Playing = true;
		}

		public class ProtectDeath : SoldierDeathState
		{
			public override string GetName() => "Death";

			[BehaviourEvent]
			void OnEnter()
			{
				protectors.Remove(soldier);
				if (protectors.Count == 0)
				{
					AI_Playing = false;
				}
			}
		}

		class ProtectIdle : BOT_IdleState
		{
			new Soldier_Protect_BOTController soldier => base.soldier as Soldier_Protect_BOTController;

			[BehaviourEvent]
			void Update()
			{
				soldier.stateMachine.MoveToState("Weapon");
			}
		}
		class ProtectWeapon : BOT_Idle_WeaponState
		{
			new Soldier_Protect_BOTController soldier => base.soldier as Soldier_Protect_BOTController;

			[BehaviourEvent]
			void Update()
			{
				if (soldier.weapon.patrones == 0) soldier.weapon.Reload();

				if (soldier.enemy != null)
				{
					Vector2 delta = (soldier.enemy.transform.position - soldier.transform.position);
					Vector2 deltaNorm = delta.normalized;
					soldier.moveVelocity = Vector2.zero;
					soldier.transform.forward = deltaNorm;
					soldier.weapon.Fire();
				}
				else
				{
					soldier.transform.forward = soldier.transform.position.normalized;
				}
			}
		}
	}
}
