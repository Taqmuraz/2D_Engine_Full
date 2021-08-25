namespace _2DEngine.Game
{
	public sealed class TerrainRenderer : Renderer
	{
		Texture terrainTexture;
		public int sizeX { get; set; } = 15;
		public int sizeY { get; set; } = 15;

		[BehaviourEvent]
		void Start()
		{
			terrainTexture = Project.instance.mainPanel.LoadAndBindTexture("./Data/Textures/Ground/Grass.png");
		}

		protected override int queue => int.MinValue;
		protected override bool isActive => true;

		protected override void Draw(IGraphics graphics)
		{
			int size = sizeX * sizeY;
			for (int i = 0; i < size; i++)
			{
				int x = i % sizeX - (sizeX >> 1);
				int y = i / sizeX - (sizeY >> 1);

				graphics.DrawImageTiled(terrainTexture, Camera.WorldToScreen(Rect.FromCenterAndSize(new Vector2(x * 5, y * 4), new Vector2(5, 4))));
			}
		}
	}
}
