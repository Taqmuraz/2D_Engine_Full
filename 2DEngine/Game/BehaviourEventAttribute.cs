using System;

namespace _2DEngine.Game
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public sealed class BehaviourEventAttribute : Attribute
	{
	}
}