using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using _2DEngine;
using OpenTK.Graphics;
using System.Linq;
using OpenTK.Input;

namespace WinGL
{

	class MainWindow : GameWindow, IGraphics, IMainPanel
    {
		public override void Dispose()
		{
			base.Dispose();
            TextureDrawer.Dispose();
		}
        private Matrix4 ortho;
        public MainWindow(int width, int height, string title)
        {
            Width = width;
            Height = height;
            WindowState = WindowState.Maximized;
            Title = title;
            GL.Enable(EnableCap.PointSmooth);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.PolygonSmooth);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.CullFace(CullFaceMode.Front);

            orderedHandlers = drawHandlers.OrderBy(h => h.queue);
            textDrawer = new TextDrawer();

            Icon = new System.Drawing.Icon("./Data/Icon.ico");
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            ortho = Matrix4.CreateOrthographicOffCenter(0, Width, Height, 0, -100, 100);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref ortho);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        List<IDrawHandler> drawHandlers = new List<IDrawHandler>();
        IEnumerable<IDrawHandler> orderedHandlers;
        TextDrawer textDrawer;

        struct Line
		{
            public _2DEngine.Vector2 a;
            public _2DEngine.Vector2 b;
            public Color32 color;

			public Line(_2DEngine.Vector2 a, _2DEngine.Vector2 b, Color32 color)
			{
				this.a = a;
				this.b = b;
				this.color = color;
			}
		}

		List<Line> lines = new List<Line>();

        void DrawFrame()
		{
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color4.Gray);
            GL.LoadIdentity();

            TextureDrawer.queue = 0;

            lock (_2DEngine.Game.GameThread.updateLock)
            {
                TextureDrawer.BeforeDraw();

                foreach (var handler in orderedHandlers)
                {
                    handler.Draw(this);
                }

                TextureDrawer.queue = 1;

                var textSize = textDrawer.GetFontSize();
                lock (log) for (int i = 0; i < log.Count; i++) textDrawer.DrawText(log[i], new _2DEngine.Vector2(50f, 50f + i * textSize.y), new Color32(1f, 1f, 1f, 1f));

                textDrawer.Draw();

                GL.Begin(PrimitiveType.Lines);
                for (int i = 0; i < lines.Count; i++)
                {
                    var line = lines[i];
                    GL.Color4(line.color.r, line.color.g, line.color.b, line.color.a);
                    GL.Vertex2(line.a.x, line.a.y);
                    GL.Vertex2(line.b.x, line.b.y);
                }
                GL.Color4(1f, 1f, 1f, 1f);
                lines.Clear();
                GL.End();

            }

            SwapBuffers();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            DrawFrame();
        }

		void IGraphics.DrawLine(_2DEngine.Vector2 a, _2DEngine.Vector2 b, Color32 color)
		{
            lines.Add(new Line(a, b, color));
		}

		void IGraphics.DrawImage(Texture texture, Rect rect)
		{
            TextureDrawer.Draw(texture.textureIndex, rect, new Rect(0f, 0f, 1f, 1f));
		}

		void IGraphics.DrawImageTiled(Texture texture, Rect rect, Rect textureRect)
		{
            TextureDrawer.Draw(texture.textureIndex, rect, textureRect);
        }

		void IGraphics.DrawImageTiled(Texture texture, Rect rect)
		{
            TextureDrawer.Draw(texture.textureIndex, rect, new Rect(0f, 0f, 1f, 1f));
        }

		void IGraphics.DrawImage(Texture texture, Rect rect, Rect textureRect)
		{
            TextureDrawer.Draw(texture.textureIndex, rect, textureRect);
        }

		void IGraphics.DrawRect(Rect rect, Color32 color)
		{
            lines.Add(new Line(rect.min, rect.downRight, color));
            lines.Add(new Line(rect.min, rect.upperLeft, color));
            lines.Add(new Line(rect.max, rect.downRight, color));
            lines.Add(new Line(rect.max, rect.upperLeft, color));
        }

		void IGraphics.DrawCircle(Rect rect, Color32 color)
		{

		}

		void IGraphics.DrawString(_2DEngine.Vector2 position, string text, Color32 color)
		{
            textDrawer.DrawText(text, position, color);
		}

		void IMainPanel.AddDrawHandler(IDrawHandler handler)
		{
            drawHandlers.Add(handler);
		}

        List<_2DEngine.IInputHandler> inputHandlers = new List<IInputHandler>();

		void IMainPanel.AddInputHandler(IInputHandler handler)
		{
            inputHandlers.Add(handler);
		}

        _2DEngine.KeyCode ToKeyCode(Key key)
		{
            if (key >= Key.A && key < Key.Z) return (KeyCode)((int)key - (int)Key.A + (int)KeyCode.A);
            else if (key >= Key.Keypad0 && key <= Key.Keypad9) return (KeyCode)((int)key - (int)Key.Keypad0 + (int)KeyCode.N0);
            else
            {
                switch (key)
                {
                    case Key.LShift:return KeyCode.ShiftKey;
                    case Key.RShift:return KeyCode.ShiftKey;
                    case Key.LControl: return KeyCode.ControlKey;
                    case Key.RControl: return KeyCode.ControlKey;
                }
            }
            return KeyCode.None;
        }

		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			base.OnKeyDown(e);
            foreach (var handler in inputHandlers) handler.OnKeyDown((Keys)(int)ToKeyCode(e.Key));
		}

		protected override void OnKeyUp(KeyboardKeyEventArgs e)
		{
            foreach (var handler in inputHandlers) handler.OnKeyUp((Keys)(int)ToKeyCode(e.Key));
        }

        int ToMouseButton(MouseButton btn)
		{
			switch (btn)
			{
				case MouseButton.Left: return 0;
				case MouseButton.Middle: return 2;
				case MouseButton.Right: return 1;
			}
            return 3;
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
            foreach (var handler in inputHandlers) handler.OnMouseDown(e.Position, ToMouseButton(e.Button));
        }
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
            foreach (var handler in inputHandlers) handler.OnMouseUp(e.Position, ToMouseButton(e.Button));
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            foreach (var handler in inputHandlers) handler.OnMouseMove(e.Position);
        }

		void IMainPanel.OnWindowKeyDown(KeyEventArgs e)
		{

		}

		void IMainPanel.OnWindowKeyUp(KeyEventArgs e)
		{

		}

		void IMainPanel.RemoveDrawHandler(IDrawHandler handler)
		{
            drawHandlers.Remove(handler);
		}

		void IMainPanel.RemoveInputHandler(IInputHandler handler)
		{
            inputHandlers.Remove(handler);
        }

		Texture IMainPanel.LoadAndBindTexture(string fileName)
		{
            return TextureDrawer.LoadTexture(fileName);
		}

        List<string> log = new List<string>();

		void IMainPanel.Log(string message)
		{
            lock (log)
            {
                log.Add(message);
                if (log.Count > 25) log.RemoveAt(0);
            }
		}

		_2DEngine.Vector2 IMainPanel.GetScreenSize()
		{
            return new _2DEngine.Vector2(Width, Height);
		}

		void IMainPanel.DrawCall()
		{
            //DrawFrame();
		}

		void IGraphics.DrawImage(Texture texture, Matrix4x4 matrix)
		{
            TextureDrawer.Draw(texture.textureIndex, matrix, new Rect(0f, 0f, 1f, 1f));
        }

		void IGraphics.DrawImage(Texture texture, Matrix4x4 matrix, Matrix4x4 uv)
		{
            TextureDrawer.Draw(texture.textureIndex, matrix, uv);
        }

		public void DrawString(Rect rect, string text, Color32 color)
		{
            var fSize = textDrawer.GetFontSize();
            var offset = new _2DEngine.Vector2(-fSize.x * text.Length * 0.5f, -fSize.y * 0.5f);
            textDrawer.DrawText(text, rect.center + offset, color);
		}
	}
}