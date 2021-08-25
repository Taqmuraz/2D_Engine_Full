using _2DEngine;
using System.Drawing;
using System.Collections.Generic;

namespace WinGL
{
	class TextDrawer
    {
        System.Drawing.Bitmap image;
        Texture texture;
        System.Drawing.Graphics graphics;
        Font font;
        System.Text.Encoding encoding;

        public TextDrawer ()
		{
            encoding = System.Text.Encoding.GetEncoding(1251);

            font = new Font(SystemFonts.DefaultFont.Name, 21, GraphicsUnit.Pixel);
            
            int letterWidth = (int)System.Math.Ceiling(font.Size) + 4;
            int letterHeight = font.Height + 4;

            image = new Bitmap(letterWidth * 16, letterHeight * 16);

            graphics = Graphics.FromImage(image);
            graphics.Clear(new Color32(0f, 0f, 0f, 0f));

            var bytes = new byte[1];

			for (int i = 0; i < 256; i++)
			{
                int x = i % 16;
                int y = i / 16;
                bytes[0] = (byte)i;
                graphics.DrawString(encoding.GetString(bytes), font, Brushes.White, x * letterWidth, y * letterHeight);
			}

            texture = TextureDrawer.LoadTexture(image);
        }

        public Vector2 GetFontSize()
		{
            Vector2 s = new Vector2(font.Size, font.Height) * _2DEngine.Game.Camera.CameraToScreen().MultiplySize(Vector3.one).y * 0.0015f;
            s.x *= 0.75f;
            return s;
        }

        public void Draw()
		{
            Vector2 fontSize = GetFontSize();

			foreach (var text in texts)
			{
                var ascii = encoding.GetBytes(text.text);

                OpenTK.Graphics.OpenGL.GL.Color4(text.color.r, text.color.g, text.color.b, (byte)0xFF);

				for (int i = 0; i < ascii.Length; i++)
				{
                    float d = 1f / 16f;
                    float x = (ascii[i] % 16) * d;
                    float y = (ascii[i] / 16) * d;
                    TextureDrawer.Draw(texture.textureIndex, new Rect(text.point.x + fontSize.x * i, text.point.y, fontSize.x, fontSize.y), new Rect(x, y, d, d));
                    //(Project.instance.mainPanel as IGraphics).DrawRect(new Rect(text.point.x + fontSize.x * i, text.point.y, fontSize.x, fontSize.y), Color32.red);
                }
            }
            //TextureDrawer.Draw(texture, new Rect(0f, 0f, font.Size * 16, font.Height * 16), new Rect(0f, 0f, 1f, 1f));

            OpenTK.Graphics.OpenGL.GL.Color4(1f, 1f, 1f, 1f);

            texts.Clear();
        }

        struct Text
        {
            public Vector2 point;
            public string text;
            public Color32 color;

			public Text(string text, Vector2 point, Color32 color)
			{
				this.text = text;
				this.point = point;
				this.color = color;
			}
		}

        List<Text> texts = new List<Text>();

        public void DrawText(string text, _2DEngine.Vector2 point, Color32 color)
		{
            texts.Add(new Text(text, point, color));
        }
	}
}