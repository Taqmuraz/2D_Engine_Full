using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using _2DEngine;
using System;

namespace WinGL
{
	static class TextureDrawer
    {
        static int vertexBufferId;
        static readonly float[] vertexData = new float[12];


        static List<Texture2D> textures = new List<Texture2D>();
        static Dictionary<string, Texture> cache = new Dictionary<string, Texture>();

		public static int queue { get; internal set; }

		static TextureDrawer()
		{
            vertexBufferId = GL.GenBuffer();
        }

        public static Texture LoadTexture(System.Drawing.Bitmap image)
		{
            Texture t;
            string hash = image.GetHashCode().ToString();

            if (cache.ContainsKey(hash))
            {
                t = cache[hash];
            }
            else
            {
                int index = textures.Count + texturesToLoad.Count;
                texturesToLoad.Add(image);
                cache.Add(hash, t = new Texture(index, image.Width, image.Height));
            }

            return t;
        }

		public static void UnloadTexture(int texture)
		{
            textures[texture].Dispose();
		}

        static List<System.Drawing.Bitmap> texturesToLoad = new List<System.Drawing.Bitmap>();

		public static Texture LoadTexture(string fileName)
		{
            Texture t;
            if (cache.ContainsKey(fileName))
            {
                t = cache[fileName];
            }
            else
            {
                int index = textures.Count + texturesToLoad.Count;
                System.Drawing.Bitmap image;
                texturesToLoad.Add(image = new System.Drawing.Bitmap(fileName));
                cache.Add(fileName, t = new Texture(index, image.Width, image.Height));
            }

            return t;
        }

        public static void BeforeDraw()
		{
            foreach (var texture in texturesToLoad)
            {
                textures.Add(new Texture2D(texture));
            }
            texturesToLoad.Clear();
		}

        public static void SetMatrix(Matrix4x4 matrix)
        {
            float depth = -100 + queue;

            Vector3 a = matrix.MultiplyPoint(new Vector3(0.5f, 0.5f, 0f));
            Vector3 b = matrix.MultiplyPoint(new Vector3(0.5f, -0.5f, 0f));
            Vector3 c = matrix.MultiplyPoint(new Vector3(-0.5f, -0.5f, 0f));
            Vector3 d = matrix.MultiplyPoint(new Vector3(-0.5f, 0.5f, 0f));

            //(Project.instance.mainPanel as IGraphics).DrawLine((Vector2)a, (Vector2)b, new Color32(1f, 0f, 0f, 1f));
            //(Project.instance.mainPanel as IGraphics).DrawLine((Vector2)b, (Vector2)c, new Color32(1f, 0f, 0f, 1f));
            //(Project.instance.mainPanel as IGraphics).DrawLine((Vector2)c, (Vector2)d, new Color32(1f, 0f, 0f, 1f));
            //(Project.instance.mainPanel as IGraphics).DrawLine((Vector2)d, (Vector2)a, new Color32(1f, 0f, 0f, 1f));

            vertexData[0] = d.x;
            vertexData[1] = d.y;
            vertexData[2] = depth;
            vertexData[3] = a.x;
            vertexData[4] = a.y;
            vertexData[5] = depth;
            vertexData[6] = b.x;
            vertexData[7] = b.y;
            vertexData[8] = depth;
            vertexData[9] = c.x;
            vertexData[10] = c.y;
            vertexData[11] = depth;

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public static void Draw(int textureIndex, Rect rect, Rect uv)
        {
            if (!rect.IntersectsWith(new Rect(Vector2.zero, Project.instance.mainPanel.GetScreenSize()))) return;

            var texture = textures[textureIndex];
            if (texture.disposed) return;

            SetMatrix(rect.ToMatrixTransform());

            DrawTexture(texture, uv.ToMatrixUV());
        }

        public static void Draw(int textureIndex, Matrix4x4 matrix, Rect uv)
        {
            Vector2 pos = (Vector2)matrix.column_3;
            Vector2 max = (Vector2)(matrix.MultiplyPoint(new Vector3(1f, 1f, 1f)));
            var rect = new Rect(Vector2.Min(pos, max), Vector2.Max(pos, max));

            if (!rect.IntersectsWith(new Rect(Vector2.zero, Project.instance.mainPanel.GetScreenSize()))) return;

            var texture = textures[textureIndex];
            if (texture.disposed) return;

            var m = uv.ToMatrixUV();

            SetMatrix(matrix);

            DrawTexture(texture, m);
        }

        public static void Draw(int textureIndex, Matrix4x4 matrix, Matrix4x4 uv)
        {
            Vector2 pos = (Vector2)matrix.column_3;
            Rect rect = new Rect(pos.x, pos.y, 0f, 0f);

            rect = rect.Spread(matrix.MultiplyPoint(new Vector2(-0.5f, -0.5f)));
            rect = rect.Spread(matrix.MultiplyPoint(new Vector2(0.5f, -0.5f)));
            rect = rect.Spread(matrix.MultiplyPoint(new Vector2(-0.5f, 0.5f)));
            rect = rect.Spread(matrix.MultiplyPoint(new Vector2(0.5f, 0.5f)));

            //(Project.instance.mainPanel as IGraphics).DrawRect(rect, new Color32(1f, 0f, 1f, 1f));

            if (!rect.IntersectsWith(new Rect(Vector2.zero, Project.instance.mainPanel.GetScreenSize()))) return;

            var texture = textures[textureIndex];
            if (texture.disposed) return;

            SetMatrix(matrix);

            DrawTexture(texture, uv);
        }

        static void DrawTexture(Texture2D texture, Matrix4x4 uv)
		{
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);

            texture.SetMatrix(uv);
            texture.Bind();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);

            GL.VertexPointer(3, VertexPointerType.Float, 0, 0);
            //GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, sizeof(float) * 12);

            GL.DrawArrays(PrimitiveType.Quads, 0, vertexData.Length);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            texture.Unbind();
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
        }

        public static void Dispose()
        {
            foreach (var t in textures) t.Dispose();
            GL.DeleteBuffer(vertexBufferId);
        }
    }
}