using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Drawing.Imaging;

namespace _2DEngine.SystemDrawing
{
	internal class GraphicsBridge : IGraphics
	{
		List<Image> textures = new List<Image>();
		static ImageAttributes attributes;

		static GraphicsBridge()
		{
			attributes = new ImageAttributes();
			attributes.SetWrapMode(System.Drawing.Drawing2D.WrapMode.Tile);
		}

		bool CkeckInsideScreen(Rect rect)
		{
			return new Rect(Vector2.zero, Project.instance.mainPanel.GetScreenSize()).IntersectsWith(rect);
		}

		public int BindTexture(Image texture)
		{
			int index = textures.Count;
			textures.Add(texture);
			return index;
		}

		public Graphics activeGraphics { get; set; }

		public void DrawLine(Vector2 a, Vector2 b, Color32 color)
		{
			activeGraphics.DrawLine(new Pen(color), a, b);
		}

		public void DrawImage(Texture texture, Rect rect)
		{
			if (CkeckInsideScreen(rect)) activeGraphics.DrawImage(textures[texture.textureIndex], new RectangleF(rect.min.x, rect.min.y, rect.size.x, rect.size.y));
		}

		public void DrawImage(Texture texture, Rect rect, Rect textureRect)
		{
			if (CkeckInsideScreen(rect))
			{
				textureRect = _2DEngine.Game.Camera.WorldToScreen(textureRect);
				//textureRect = Rect.FromCenterAndSize(textureRect.center + rect.center, textureRect.size);

				TextureBrush tb = new TextureBrush(textures[texture.textureIndex]);
				tb.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0f, 0f, 1f, textureRect.min.x, textureRect.min.y);
				activeGraphics.FillRectangle(tb, rect);
			}
		}

		PointF[] nonAllocTriangle = new PointF[3];
		public void DrawImageTiled(Texture texture, Rect rect, Rect textureRect)
		{
			if (CkeckInsideScreen(rect))
			{
				nonAllocTriangle[2] = new PointF(rect.min.x, rect.max.y);
				nonAllocTriangle[1] = new PointF(rect.max.x, rect.min.y);
				nonAllocTriangle[0] = rect.min;
				activeGraphics.DrawImage(textures[texture.textureIndex], nonAllocTriangle, textureRect, GraphicsUnit.Pixel, attributes);
			}
		}

		public void DrawRect(Rect rect, Color32 color)
		{
			if (CkeckInsideScreen(rect))
			{
				activeGraphics.DrawRectangle(new Pen(color), (Rectangle)rect);
			}
		}

		public void DrawImageTiled(Texture texture, Rect rect)
		{
			if (CkeckInsideScreen(rect))
			{
				nonAllocTriangle[2] = new PointF(rect.min.x, rect.max.y);
				nonAllocTriangle[1] = new PointF(rect.max.x, rect.min.y);
				nonAllocTriangle[0] = rect.min;
				Rect textureRect = new Rect(Vector2.zero, new Vector2(texture.width, texture.height));
				activeGraphics.DrawImage(textures[texture.textureIndex], nonAllocTriangle, textureRect, GraphicsUnit.Pixel, attributes);
			}
		}

		public void DrawCircle(Rect rect, Color32 color)
		{
			activeGraphics.DrawEllipse(new Pen(color), rect);
		}

		public void DrawString(Vector2 position, string text, Color32 color)
		{
			activeGraphics.DrawString(text, SystemFonts.DefaultFont, new SolidBrush(color), position);
		}

		public void DrawImage(Texture texture, Matrix4x4 matrix)
		{
			Vector2 pos = (Vector2)matrix.column_3;
			Rect rect = new Rect(pos.x, pos.y, 0f, 0f);

			Vector2 a, b, c;

			rect = rect.Spread(c = matrix.MultiplyPoint(new Vector2(-0.5f, -0.5f)));
			rect = rect.Spread(b = matrix.MultiplyPoint(new Vector2(0.5f, -0.5f)));
			rect = rect.Spread(a = matrix.MultiplyPoint(new Vector2(-0.5f, 0.5f)));
			rect = rect.Spread(matrix.MultiplyPoint(new Vector2(0.5f, 0.5f)));

			if (CkeckInsideScreen(rect))
			{
				nonAllocTriangle[2] = a;
				nonAllocTriangle[1] = b;
				nonAllocTriangle[0] = c;
				activeGraphics.DrawImage(textures[texture.textureIndex], nonAllocTriangle);
			}
		}

		PointF[] nonAllocQuad = new PointF[4];

		public void DrawImage(Texture texture, Matrix4x4 matrix, Matrix4x4 uv)
		{
			Vector2 pos = (Vector2)matrix.column_3;
			Rect rect = new Rect(pos.x, pos.y, 0f, 0f);

			Vector2 a, b, c, d;

			rect = rect.Spread(c = matrix.MultiplyPoint(new Vector2(-0.5f, -0.5f)));
			rect = rect.Spread(b = matrix.MultiplyPoint(new Vector2(0.5f, -0.5f)));
			rect = rect.Spread(a = matrix.MultiplyPoint(new Vector2(-0.5f, 0.5f)));
			rect = rect.Spread(d = matrix.MultiplyPoint(new Vector2(0.5f, 0.5f)));

			activeGraphics.DrawPolygon(Pens.Magenta, new PointF[4] { a, d, b, c });

			if (CkeckInsideScreen(rect))
			{
				uv = uv.GetInversed() * Matrix4x4.CreateScaleMatrix((matrix).MultiplySize(Vector3.one));

				try
				{
					nonAllocQuad[3] = a;
					nonAllocQuad[2] = d;
					nonAllocQuad[1] = b;
					nonAllocQuad[0] = c;
					TextureBrush tb = new TextureBrush(textures[texture.textureIndex]);
					var trans = tb.Transform;
					trans.Multiply(new System.Drawing.Drawing2D.Matrix(uv.column_0.x, uv.column_0.y, uv.column_1.x, uv.column_1.y, uv.column_3.x, uv.column_3.y));
					tb.Transform = trans;
					activeGraphics.FillPolygon(tb, nonAllocQuad);
				}
				catch
				{
					throw new System.Exception(matrix.GetDeterminant().ToString());
				}
			}
		}

		public void DrawString(Rect rect, string text, Color32 color)
		{
			var lastTransform = activeGraphics.Transform;
			activeGraphics.Transform = new System.Drawing.Drawing2D.Matrix(rect.size.x, 0f, 0f, rect.size.y, rect.min.x, rect.min.y);
			activeGraphics.DrawString(text, System.Drawing.SystemFonts.DefaultFont, new SolidBrush(color), new PointF());
			activeGraphics.Transform = lastTransform;
		}
	}

	public class DrawPanel : Panel, IMainPanel
	{
		GraphicsBridge graphicsBridge = new GraphicsBridge();

		Dictionary<string, Texture> texturesCache = new Dictionary<string, Texture>();

		List<IDrawHandler> drawHandlers;
		List<IInputHandler> inputHandlers = new List<IInputHandler>();
		IEnumerable<IDrawHandler> orderedDrawHandlers;

		public DrawPanel()
		{
			drawHandlers = new List<IDrawHandler>();
			orderedDrawHandlers = drawHandlers.OrderBy(d => d.queue);

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

			/*refreshTimer = new Timer();
			refreshTimer.Tick += (s, e) => Refresh();
			refreshTimer.Interval = 33;
			refreshTimer.Start();*/
		}

		public void AddDrawHandler(IDrawHandler handler)
		{
			drawHandlers.Add(handler);
		}
		public void RemoveDrawHandler(IDrawHandler handler)
		{
			drawHandlers.Remove(handler);
		}

		public void AddInputHandler(IInputHandler handler)
		{
			inputHandlers.Add(handler);
		}
		public void RemoveInputHandler(IInputHandler handler)
		{
			inputHandlers.Remove(handler);
		}

		List<string> log = new List<string>();
		const int MAX_MESSAGES = 25;

		public void Log(string message)
		{
			lock (log)
			{
				log.Add(message);
				while (log.Count >= MAX_MESSAGES) log.RemoveAt(0);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var g = graphicsBridge.activeGraphics = e.Graphics;
			g.Clear(Color.Gray);
			//g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

			lock (drawHandlers)
			{
				foreach (var handler in orderedDrawHandlers)
				{
					if (handler.isActive) handler.Draw(graphicsBridge);
				}
			}

			lock (log)
			{
				for (int i = 0; i < log.Count; i++)
				{
					string message = log[i];
					var font = SystemFonts.DefaultFont;
					g.DrawString(message, font, Brushes.GreenYellow, new PointF(25f, 25f + i * (font.Size + font.Height)));
				}
			}
		}

		public void OnWindowKeyDown(KeyEventArgs e)
		{
			lock (inputHandlers) for (int i = 0; i < inputHandlers.Count; i++) inputHandlers[i].OnKeyDown(e.KeyCode);
		}
		public void OnWindowKeyUp(KeyEventArgs e)
		{
			lock (inputHandlers) for (int i = 0; i < inputHandlers.Count; i++) inputHandlers[i].OnKeyUp(e.KeyCode);
		}


		int ToMouseButton(MouseButtons btn)
		{
			switch (btn)
			{
				case MouseButtons.Left: return 0;
				case MouseButtons.Middle: return 2;
				case MouseButtons.Right: return 1;
			}
			return 3;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			lock (inputHandlers) for (int i = 0; i < inputHandlers.Count; i++) inputHandlers[i].OnMouseDown(e.Location, ToMouseButton(e.Button));
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			lock (inputHandlers) for (int i = 0; i < inputHandlers.Count; i++) inputHandlers[i].OnMouseMove(e.Location);
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			lock (inputHandlers) for (int i = 0; i < inputHandlers.Count; i++) inputHandlers[i].OnMouseUp(e.Location, ToMouseButton(e.Button));
		}

		public Texture LoadAndBindTexture(string fileName)
		{
			if (string.IsNullOrEmpty(fileName)) throw new System.Exception("File name is empty");

			if (texturesCache.ContainsKey(fileName))
			{
				return texturesCache[fileName];
			}
			else
			{
				var image = new Bitmap(fileName);
				var texture = new Texture(graphicsBridge.BindTexture(image), image.Width, image.Height);
				texturesCache.Add(fileName, texture);
				return texture;
			}
		}

		public Vector2 GetScreenSize()
		{
			return new Vector2(Width, Height);
		}

		public void DrawCall()
		{
			Refresh();
		}
	}
}
