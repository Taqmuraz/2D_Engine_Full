using System.Collections.Generic;

namespace _2DEngine.Game
{
	public class Soldier_Wave_BOTController : SoldierBOTController
	{
		public static int waveAlive { get; set; }

		[BehaviourEvent]
		void Start()
		{
			waveAlive++;
		}

		class WaveDeath : SoldierDeathState
		{
			protected override string CreateName() => "Death";
			ITimedResolver destroyResolver;

			[BehaviourEvent]
			void OnEnter()
			{
				waveAlive--;
				WaveSpawner.OnWaveDeath();
				destroyResolver = new TimedResolver(10f);
			}
			[BehaviourEvent]
			void Update()
			{
				if (destroyResolver.resolving) soldier.gameObject.Destroy();
			}
		}

		protected override IEnumerable<SoldierState> OverrideStates()
		{
			yield return new WaveIdle();
			yield return new WaveWeapon();
			yield return new WaveDeath();
		}

		class WaveIdle : BOT_IdleState
		{
			protected new Soldier_Wave_BOTController soldier => base.soldier as Soldier_Wave_BOTController;

			[BehaviourEvent]
			void Update()
			{
				if (!AI_Playing)
				{
					soldier.moveVelocity = Vector2.zero;
					return;
				}

				if (soldier.enemy != null)
				{
					stateMachine.MoveToState("Weapon");
				}
				else
				{
					int count = Soldier_Protect_BOTController.protectors.Count;
					if (count != 0)
					{
						Vector2 pos = Vector2.zero;
						foreach (var protector in Soldier_Protect_BOTController.protectors)
						{
							pos += protector.transform.position;
						}
						Vector2 delta = pos / count - soldier.transform.position;
						soldier.moveVelocity = delta.normalized * moveSpeed;
						soldier.transform.forward = delta;
					}
				}
			}
		}
		class WaveWeapon : BOT_Idle_WeaponState
		{
			protected new Soldier_Wave_BOTController soldier => base.soldier as Soldier_Wave_BOTController;

			[BehaviourEvent]
			void Update()
			{
				if (!AI_Playing)
				{
					soldier.moveVelocity = Vector2.zero;
					return;
				}

				if (soldier.enemy == null)
				{
					stateMachine.MoveToState("Idle");
				}
				else
				{
					Vector2 delta = (soldier.enemy.transform.position - soldier.transform.position);
					Vector2 deltaNorm = delta.normalized;
					if (delta.length < ViewDistance * 0.5f)
					{
						soldier.moveVelocity = Vector2.zero;
						soldier.transform.forward = deltaNorm;
						soldier.weapon.Fire();
					}
					else
					{
						soldier.moveVelocity = deltaNorm * moveSpeed;
						soldier.transform.forward = deltaNorm;
					}
					if (soldier.weapon.patrones == 0) soldier.weapon.Reload();
				}
			}
		}
	}
}
