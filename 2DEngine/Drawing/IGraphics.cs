namespace _2DEngine
{
	public interface IGraphics
	{
		void DrawLine (Vector2 a, Vector2 b, Color32 color);
		void DrawImage(Texture texture, Rect rect);
		void DrawImageTiled(Texture texture, Rect rect, Rect textureRect);
		void DrawImageTiled(Texture texture, Rect rect);
		void DrawImage(Texture texture, Rect rect, Rect textureRect);
		void DrawImage(Texture texture, Matrix4x4 matrix);
		void DrawImage(Texture texture, Matrix4x4 matrix, Matrix4x4 uv);
		void DrawRect(Rect rect, Color32 color);
		void DrawCircle(Rect rect, Color32 color);
		void DrawString(Vector2 position, string text, Color32 color);
		void DrawString(Rect rect, string text, Color32 color);
	}
}
