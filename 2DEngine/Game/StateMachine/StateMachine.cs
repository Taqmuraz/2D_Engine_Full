using System.Collections.Generic;

namespace _2DEngine.Game
{
	public abstract class StateMachine
	{
		SafeDictionary<string, State> states = new SafeDictionary<string, State>();
		List<State> statesList = new List<State>();

		public State activeState { get; private set; }

		public StateMachine(IEnumerable<State> states)
		{
			foreach (var state in states)
			{
				statesList.Add(state);
				this.states[state.GetName()] = state;
			}
		}

		public abstract string GetInitialStateName();

		public void Initialize()
		{
			MoveToState(GetInitialStateName());
		}

		bool movingToState;
		public void MoveToState(string name)
		{
			if (movingToState) Debug.LogError("Already moving to state");

			if (!states.ContainsKey(name))
			{
				Debug.LogError("There are no state with name " + name);
				return;
			}
			movingToState = true;
			if (activeState != null) activeState.CallEvent("OnExit");
			activeState = states[name];
			activeState.CallEvent("OnEnter");
			movingToState = false;
		}
	}

}