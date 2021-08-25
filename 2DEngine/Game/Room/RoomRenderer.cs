namespace _2DEngine.Game
{
	public sealed class RoomRenderer : Renderer
	{
		public int sizeX { get; set; } = 5;
		public int sizeY { get; set; } = 5;
		public int wallMapSizeX { get; set; }
		public int wallMapSizeY { get; set; }
		public int floorMapSizeX { get; set; }
		public int floorMapSizeY { get; set; }
		public Vector2 position { get; set; }
		public int wallTextureOffset { get; set; }
		public int floorTextureOffset { get; set; }

		public Texture wallTexture { get; set; }
		public Texture floorTexture { get; set; }

		protected override int queue => int.MinValue;
		protected override bool isActive => true;

		protected override void Draw(IGraphics graphics)
		{
			Vector2 wallSize = new Vector2(1f, 1f);
			Matrix4x4 w2s = Camera.WorldToScreen();

			float wl = wallSize.length;
			Matrix4x4 floor = Matrix4x4.CreateWorldMatrix(new Vector3(-wl * sizeX, 0f, 0f), new Vector3(0f, wl * sizeY, 0f), Vector3.forward, position);
			floor *= Matrix4x4.CreateRotationMatrix_Z(45f);

			if (wallTexture == null) goto FLOOR;

			int wMapX = wallMapSizeX;
			int wMapY = wallMapSizeY;
			float tx = (1f / wMapX);
			float ty = (1f / wMapY);
			int wx = wallTextureOffset % wMapX;
			int wy = wallTextureOffset / wMapX;
			Rect uv_wall_left = new Rect(wx * tx, wy * ty, tx * 0.5f, ty);
			Rect uv_wall_right = new Rect((wx + 0.5f) * tx, wy * ty, tx * 0.5f, ty);

			Vector2 sd = floor.MultiplyVector(new Vector2(-0.5f, 0.5f));

			for (int x = 0; x < sizeX; x++)
			{
				Vector2 pos = position + sd - new Vector2(wallSize.x, wallSize.y) * x + new Vector2(-wallSize.x * 0.5f, wallSize.y * 0.5f);
				Rect screen = Rect.FromCenterAndSize(w2s.MultiplyPoint(pos), w2s.MultiplyVector(new Vector2(wallSize.x, wallSize.y * 3f)).Abs());
				graphics.DrawImage(wallTexture, screen, uv_wall_left);
			}
			for (int x = 0; x < sizeY; x++)
			{
				Vector2 pos = position + sd - new Vector2(-wallSize.x, wallSize.y) * x + new Vector2(wallSize.x * 0.5f, wallSize.y * 0.5f);
				Rect screen = Rect.FromCenterAndSize(w2s.MultiplyPoint(pos), w2s.MultiplyVector(new Vector2(wallSize.x, wallSize.y * 3f)).Abs());
				graphics.DrawImage(wallTexture, screen, uv_wall_right);
			}

			FLOOR:

			if (floorTexture == null) return;

			int fMapX = floorMapSizeX;
			int fMapY = floorMapSizeY;
			tx = 1f / fMapX;
			ty = 1f / fMapY;
			int fx = floorTextureOffset % fMapX;
			int fy = floorTextureOffset / fMapX;

			Rect floorUv = new Rect(fx * tx, fy * ty, tx, ty);
			Matrix4x4 uvMatrix = Matrix4x4.CreateWorldMatrix(new Vector3(floorUv.size.x, 0f, 0f), new Vector3(0f, floorUv.size.y, 0f), Vector3.forward, new Vector3(floorUv.min.x, floorUv.min.y, 0f));

			int c = sizeX * sizeY;
			for (int i = 0; i < c; i++)
			{
				float x = i % sizeX - sizeX * 0.5f;
				float y = i / sizeX - sizeY * 0.5f;
				Matrix4x4 f = Matrix4x4.CreateWorldMatrix(new Vector3(x * wl + wl * 0.5f, y * wl + wl * 0.5f, 0f), new Vector3(-wl, wl, 1f));
				f *= Matrix4x4.CreateRotationMatrix_Z(45f);
				graphics.DrawImage(floorTexture, f * w2s, uvMatrix);
			}
		}
	}
}
