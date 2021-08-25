using System;
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
		struct Box
		{
			public Rect rect;
			public Color32 color;

			public Box(Rect rect, Color32 color)
			{
				this.rect = rect;
				this.color = color;
			}
		}
		struct Text
		{
			public Vector2 point;
			public Color32 color;
			public string text;

			public Text(Vector2 point, string text, Color32 color)
			{
				this.point = point;
				this.color = color;
				this.text = text;
			}
		}

		static List<Line> lines;

		public static void LogError(Exception ex)
		{
			new ErrorForm(ex).ShowDialog();
		}
		public static void LogError(string error)
		{
			new ErrorForm(new Exception(error)).ShowDialog();
		}

		static List<Box> boxes;
		static List<Text> texts;

		static Debug()
		{
			Project.instance.mainPanel.AddDrawHandler(new DebugDrawer());
			lines = new List<Line>();
			boxes = new List<Box>();
			texts = new List<Text>();
		}

		class DebugDrawer : IDrawHandler
		{
			int IDrawHandler.queue => int.MaxValue;
			bool IDrawHandler.isActive => true;

			void IDrawHandler.Draw(IGraphics graphics)
			{
				for (int i = 0; i < lines.Count; i++)
				{
					var line = lines[i];
					graphics.DrawLine(Camera.WorldToScreen(line.a), Camera.WorldToScreen(line.b), line.color);
				}
				for (int i = 0; i < boxes.Count; i++)
				{
					var box = boxes[i];
					graphics.DrawRect(Camera.WorldToScreen(box.rect), box.color);
				}
				for (int i = 0; i < texts.Count; i++)
				{
					var text = texts[i];
					graphics.DrawString(Camera.WorldToScreen(text.point), text.text, text.color);
				}
				lines.Clear();
				boxes.Clear();
				texts.Clear();
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
		public static void DrawBox(Rect rect, Color32 color)
		{
			boxes.Add(new Box(rect, color));
		}
		public static void DrawBox(Matrix4x4 matrix, Color32 color)
		{
			Vector2 a = (Vector2)matrix.MultiplyPoint(new Vector2(-0.5f, 0.5f));
			Vector2 b = (Vector2)matrix.MultiplyPoint(new Vector2(0.5f, 0.5f));
			Vector2 c = (Vector2)matrix.MultiplyPoint(new Vector2(0.5f, -0.5f));
			Vector2 d = (Vector2)matrix.MultiplyPoint(new Vector2(-0.5f, -0.5f));
			DrawLine(a, b, color);
			DrawLine(b, c, color);
			DrawLine(c, d, color);
			DrawLine(d, a, color);
		}
		public static void DrawRay(Vector2 origin, Vector2 direction, Color32 color)
		{
			lines.Add(new Line(origin, origin + direction, color));
		}

		public static void DrawText(Vector2 point, string text, Color32 color)
		{
			texts.Add(new Text(point, text, color));
		}
	}
}
