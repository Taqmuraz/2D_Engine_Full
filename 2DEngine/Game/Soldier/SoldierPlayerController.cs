using System.Collections.Generic;
using System.Linq;

namespace _2DEngine.Game
{
	public class SoldierPlayerController : SoldierController
	{
		TextRenderer weaponText;

		static SoldierPlayerController()
		{
			Debug.Log("W,A,S,D - перемещение");
			Debug.Log("ПКМ - прицелиться");
			Debug.Log("ЛКМ - стрелять");
			Debug.Log("Q - достать/убрать оружие");
			Debug.Log("Left Control - сесть/встать");
			Debug.Log("U - перезагрузить уровень");
		}

		[BehaviourEvent]
		void Start()
		{
			weaponText = gameObject.AddComponent<TextRenderer>();
			Vector2 screenSize = Project.instance.mainPanel.GetScreenSize();
			weaponText.screenSpacePosition = new Vector2(50f, screenSize.y * 0.8f);

			SoldierBOTController.AI_Playing = true;
			Soldier_Protect_BOTController.protectors.Add(this);

			WaveSpawner.Reset();
			WaveSpawner.SpawnWave();
		}
		[BehaviourEvent]
		void Update()
		{
			int count = Soldier_Protect_BOTController.protectors.Count;
			if (count != 0)
			{
				Vector2 pos = Vector2.zero;
				foreach (var protector in Soldier_Protect_BOTController.protectors)
				{
					pos += protector.transform.position;
				}
				Camera.cameraPosition = Vector2.Lerp(Camera.cameraPosition, pos / count, Time.deltaTime * 10f);
			}

			if (Input.GetKeyDown(KeyCode.U)) GameScenes.LoadScene(2);
		}

		[BehaviourEvent]
		void LateUpdate()
		{
			weaponText.text = weapon.IsReloading() ? "Перезарядка..." : $"Патроны : {weapon.patrones}/{weapon.patronesCapacity}";

			//float c = Camera.cameraSizeWithAspect.y;
			//if (Input.GetKeyDown(KeyCode.L)) c *= 2f;
			//if (Input.GetKeyDown(KeyCode.P)) c *= 0.5f;
			//Camera.cameraSizeWithAspect = new Vector2(c, c);
		}

		protected override IEnumerable<SoldierState> OverrideStates()
		{
			SoldierMoveStateBase[] states = new SoldierMoveStateBase[4];
			yield return states[0] = new PlayerIdleState();
			yield return states[1] = new PlayerWeaponState();
			yield return states[2] = new PlayerCrouchState();
			yield return states[3] = new PlayerCrouchWeaponState();
			yield return new Soldier_Protect_BOTController.ProtectDeath();

			PlayerStateHandler[] handlers = new PlayerStateHandler[4];

			handlers[0] = new PlayerIdleHandler();
			handlers[1] = new PlayerWeaponHandler();
			handlers[2] = new PlayerCrouchHandler();
			handlers[3] = new PlayerCrouchWeaponHandler();

			for (int i = 0; i < handlers.Length; i++)
			{
				handlers[i].Initialize(states[i], this);
				states[i].stateHandler = handlers[i];
			}

		}
		void MoveControl(SoldierMoveStateBase state)
		{
			Vector2 wasd = Input.GetWASD().normalized;
			transform.forward = wasd;
			moveVelocity = wasd * state.moveSpeed;
			//Camera.cameraPosition = Vector2.Lerp(Camera.cameraPosition, transform.position + wasd, Time.deltaTime * 10f);
		}

		sealed class PlayerIdleState : SoldierIdleState
		{
			protected override string CreateName() => "Idle";
		}
		sealed class PlayerWeaponState : SoldierWeaponState
		{
			protected override string CreateName() => "Weapon";
		}
		sealed class PlayerCrouchState : SoldierCrouchState
		{
			protected override string CreateName() => "Crouch";
		}
		sealed class PlayerCrouchWeaponState : SoldierCrouchWeaponState
		{
			protected override string CreateName() => "CrouchWeapon";
		}

		public class MoveStateHandler<TSoldier> : BehaviourEventsHandler, IInitializable<SoldierMoveStateBase, TSoldier> where TSoldier : SoldierController
		{
			public void Initialize(SoldierMoveStateBase state, TSoldier soldier)
			{
				this.state = state;
				this.soldier = soldier;
			}

			protected SoldierMoveStateBase state { get; private set; }
			protected TSoldier soldier { get; private set; }
		}
		class PlayerStateHandler : MoveStateHandler<SoldierPlayerController>
		{
			protected StateMachine stateMachine => soldier.stateMachine;
		}
		abstract class PlayerIdleHandlerBase : PlayerStateHandler
		{
			[BehaviourEvent]
			void Update()
			{
				soldier.MoveControl(state);
			}
		}
		class PlayerIdleHandler : PlayerIdleHandlerBase
		{
			[BehaviourEvent]
			void Update()
			{
				if (Input.GetKeyDown(KeyCode.Q))
				{
					stateMachine.MoveToState("Weapon");
				}
				if (Input.GetKeyDown(KeyCode.ControlKey))
				{
					stateMachine.MoveToState("Crouch");
				}
			}
		}
		class PlayerCrouchHandler : PlayerIdleHandlerBase
		{
			[BehaviourEvent]
			void Update()
			{
				if (Input.GetKeyDown(KeyCode.Q))
				{
					stateMachine.MoveToState("CrouchWeapon");
				}
				if (Input.GetKeyDown(KeyCode.ControlKey))
				{
					stateMachine.MoveToState("Idle");
				}
			}
		}
		class PlayerCrouchWeaponHandler : PlayerWeaponHandlerBase
		{
			[BehaviourEvent]
			void Update()
			{
				if (Input.GetKeyDown(KeyCode.Q))
				{
					stateMachine.MoveToState("Crouch");
				}
				if (Input.GetKeyDown(KeyCode.ControlKey))
				{
					stateMachine.MoveToState("Weapon");
				}
			}
		}
		class PlayerWeaponHandler : PlayerWeaponHandlerBase
		{
			[BehaviourEvent]
			void Update()
			{
				if (Input.GetKeyDown(KeyCode.Q))
				{
					stateMachine.MoveToState("Idle");
				}
				if (Input.GetKeyDown(KeyCode.ControlKey))
				{
					stateMachine.MoveToState("CrouchWeapon");
				}
			}
		}
		abstract class PlayerWeaponHandlerBase : PlayerStateHandler
		{
			[BehaviourEvent]
			void Update()
			{
				if (soldier.weapon.patronesCapacity > soldier.weapon.patrones && Input.GetKeyDown(KeyCode.R))
				{
					soldier.weapon.Reload();
				}
				if (Input.GetKey(KeyCode.MouseL))
				{
					soldier.weapon.Fire();
				}
				if (Input.GetKey(KeyCode.MouseR))
				{
					Vector2 dir = (Camera.ScreenToWorld().MultiplyPoint(Input.mousePosition) - soldier.transform.position);
					soldier.transform.forward = dir.normalized;
					soldier.moveVelocity = Vector2.zero;

					//Debug.DrawRay(soldier.transform.position, dir * 1000f, Color32.blue);
					//Collider.Raycast(new Ray(soldier.transform.position, dir), out RaycastHit hit);

					//Camera.cameraPosition = Vector2.Lerp(Camera.cameraPosition, soldier.transform.position, Time.deltaTime * 10f);
				}
				else
				{
					soldier.MoveControl(state);
				}
			}
		}
	}
}
