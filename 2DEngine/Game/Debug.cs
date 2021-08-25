using System.Collections.Generic;

namespace _2DEngine.Game
{
	public static class Debug
	{
		struct Line
		{
			public Vector2 a;
			public Vector2 b;
			public Color32 color;

			public Line(Vector2 a, Vector2 b, Color32 color)
			{
				this.a = a;
				this.b = b;
				this.color = color;
			}
		}

		static List<Line> lines;

		static Debug()
		{
			Project.instance.mainPanel.AddDrawHandler(new DebugDrawer());
			lines = new List<Line>();
		}

		class DebugDrawer : IDrawHandler
		{
			public int queue => int.MaxValue;
			public bool isActive => true;

			public void Draw(IGraphics graphics)
			{
				for (int i = 0; i < lines.Count; i++)
				{
					var line = lines[i];
					graphics.DrawLine(Camera.WorldToScreen(line.a), Camera.WorldToScreen(line.b), line.color);
				}
				lines.Clear();
			}
		}

		public static void Log(object message)
		{
			Project.Log(message.ToString());
		}
		public static void DrawLine(Vector2 a, Vector2 b, Color32 color)
		{
			lines.Add(new Line(a, b, color));
		}
		public static void DrawRay(Vector2 origin, Vector2 direction, Color32 color)
		{
			lines.Add(new Line(origin, origin + direction, color));
		}
	}
}
