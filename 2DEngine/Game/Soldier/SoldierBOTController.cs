using System.Collections;

namespace _2DEngine.Game
{

	public abstract class SoldierBOTController : SoldierController
	{
		public static bool AI_Playing { get; set; } = true;

		protected abstract class BOT_IdleState : SoldierIdleState
		{
			protected override string CreateName() => "Idle";
		}
		protected abstract class BOT_Idle_WeaponState : SoldierWeaponState
		{
			protected override string CreateName() => "Weapon";
		}

		public const float ViewDistance = 10f;

		protected SoldierController enemy { get; private set; }
		IEnumerator routine;

		[BehaviourEvent]
		void Start()
		{
			routine = AI_Routine();
		}

		[BehaviourEvent]
		void Update()
		{
			routine.MoveNext();
		}

		IEnumerator AI_Routine()
		{
			yield return null;

			START:
			while (enemy == null)
			{
				if (Rect.FromCenterAndSize(Camera.cameraPosition, Camera.cameraSizeWithAspect).Contains(transform.position))
				{
					soldiersQuadTree.TreeRecursiveCheck(node =>
					{
						float min = ViewDistance;
						var soldiers = node.GetLocatables();
						foreach (var soldier in soldiers)
						{
							float dist;
							if (soldier.soldierSide != soldierSide && (dist = (soldier.transform.position - transform.position).length) < min)
							{
								if (!Collider.Raycast(new Ray(transform.position, soldier.transform.position - transform.position), out RaycastHit hit) || (hit.collider != null && hit.collider.gameObject == soldier.gameObject))
								{
									min = dist;
									enemy = soldier;
								}
							}
						}
					});
				}
				yield return null;
			}
			while (enemy != null)
			{
				if (!enemy.isAlive || (enemy.transform.position - transform.position).length > ViewDistance) enemy = null;
				yield return null;
			}
			goto START;
		}
	}
}
