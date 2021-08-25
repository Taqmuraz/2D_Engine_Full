using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace WinGL
{
	class Texture2D : IDisposable
    {
        public int id { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }
        public int bufferID { get; private set; }
        public float[] coordinate { get; private set; }

        public bool disposed { get; private set; }

        public Texture2D(string fileName)
        {
            Bitmap bmp;
            if (File.Exists(fileName))
            {
                bmp = (Bitmap)Image.FromFile(fileName);
            }
            else
            {
                bmp = new Bitmap(1, 1);
            }
            Init(bmp);
        }
        public Texture2D(Bitmap bitmap)
        {
            Init(bitmap);
        }

        public void Init (Bitmap bmp)
        {
            //bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            width = bmp.Width;
            height = bmp.Height;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (uint)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (uint)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (uint)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (uint)TextureMinFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            bmp.UnlockBits(data);

            bufferID = GL.GenBuffer();

            
        }

        public void SetMatrix(_2DEngine.Matrix4x4 matrix)
		{
            _2DEngine.Vector2 a = matrix.MultiplyPoint(new _2DEngine.Vector2(0f, 1f));
            _2DEngine.Vector2 b = matrix.MultiplyPoint(new _2DEngine.Vector2(1f, 1f));
            _2DEngine.Vector2 c = matrix.MultiplyPoint(new _2DEngine.Vector2(1f, 0f));
            _2DEngine.Vector2 d = matrix.MultiplyPoint(new _2DEngine.Vector2(0f, 0f));

            coordinate = new[]
            {
                a.x, a.y,
                b.x, b.y,
                c.x, c.y,
                d.x, d.y
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);
            GL.BufferData(BufferTarget.ArrayBuffer, coordinate.Length * sizeof(float), coordinate, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, id);
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, 0);
        }
        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Dispose()
        {
            disposed = true;
            GL.DeleteBuffer(bufferID);
            GL.DeleteTexture(id);
        }
    }
}