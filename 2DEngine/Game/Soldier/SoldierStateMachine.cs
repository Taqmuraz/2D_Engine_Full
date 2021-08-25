using System.Collections.Generic;

namespace _2DEngine.Game
{
	public partial class SoldierController
	{
		class SoldierStateMachine : StateMachine
		{
			public SoldierStateMachine(IEnumerable<State> states) : base(states)
			{
			}

			public override string GetInitialStateName()
			{
				return "Idle";
			}
		}
	}
}
