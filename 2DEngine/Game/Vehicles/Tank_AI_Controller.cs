namespace _2DEngine.Game
{
	public class Tank_AI_Controller : TankController
	{
		System.Nullable<SoldierSide> m_side;
		public SoldierSide side
		{
			get
			{
				if (m_side.HasValue) return m_side.Value;
				else return (m_side = name.EndsWith("SR") ? SoldierSide.SR : SoldierSide.NG).Value;
			}
		}

		protected override string GetModelName()
		{
			return side == SoldierSide.SR ? "T34" : "Pantera";
		}

		SoldierController enemy;

		[BehaviourEvent]
		void Update()
		{
			bool friendlyNearby = true;

			SoldierController.soldiersQuadTree.TreeRecursiveCheck(n =>
			{
				friendlyNearby = false;

				foreach (var soldier in n.GetLocatables())
				{
					if ((soldier.transform.position - transform.position).length < SoldierBOTController.ViewDistance * soldier.visibility)
					{
						if (soldier.soldierSide == side)
						{
							friendlyNearby = true;
						}
						else if (enemy != null)
						{
							enemy = soldier;
						}
					}
					if (enemy != null && friendlyNearby == true) break;
				}

			}, n => n.GetBounds().Contains(transform.position));

			if (friendlyNearby)
			{
				float targetAngle = transform.rotation;
				if (enemy != null)
				{
					targetAngle = (enemy.transform.position - transform.position).ToAngle();

					if (Mathf.Abs(targetAngle - transform.rotation) < 10f)
					{
						Fire();
					}
				}
				towerTransform.rotation = Mathf.Lerp(towerTransform.rotation, targetAngle, Time.deltaTime * 5f);
			}
			else
			{
				var soldier = new GameObject($"Soldier_{side}").AddComponent<Soldier_Protect_BOTController>();
				soldier.transform.position = transform.position + transform.right * 2.5f;
				soldier.transform.forward = transform.right;
				Destroy();
			}
		}
	}
}
