namespace _2DEngine.Game
{
	public sealed class TerrainRenderer : Renderer
	{
		Texture terrainTexture;
		public int sizeX { get; set; } = 500;
		public int sizeY { get; set; } = 500;
		public float tilingX { get; set; } = 200f;
		public float tilingY { get; set; } = 200f;

		[BehaviourEvent]
		void Start()
		{
			terrainTexture = Project.instance.mainPanel.LoadAndBindTexture("./Data/Textures/Ground/Grass.png");
		}

		protected override int queue => int.MinValue;
		protected override bool isActive => true;

		protected override void Draw(IGraphics graphics)
		{
			Matrix4x4 matrix = Matrix4x4.CreateScaleMatrix(new Vector3(sizeX, sizeY, 1f)) * Matrix4x4.CreateRotationMatrix_Y(180f) * Matrix4x4.CreateRotationMatrix_Z(45f) * Camera.WorldToScreen();
			Matrix4x4 uv = Matrix4x4.CreateScaleMatrix(new Vector3(tilingX, tilingY, 1f));
			graphics.DrawImage(terrainTexture, matrix, uv);
		}
	}
}
