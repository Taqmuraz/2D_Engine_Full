using System.Collections.Generic;

namespace _2DEngine.Game
{
	public class RoomController : Component
	{
		public class RoomConnection
		{
			public RoomConnection(RoomController roomA, RoomController roomB)
			{
				this.roomA = roomA;
				this.roomB = roomB;
			}

			public RoomController roomA { get; }
			public RoomController roomB { get; }
		}

		public enum WallSide
		{
			Left,Right,Bottom,Top
		}


		public class RoomConnectionPoint
		{
			public RoomController room { get; }
			public WallSide side { get; }
			public int wallOffset { get; }
		}

		RoomRenderer renderer;
		List<RoomController> connections;
	}
}
