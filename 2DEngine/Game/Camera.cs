namespace _2DEngine.Game
{
	public static class Camera
	{
		public static Vector2 cameraPosition { get; set; }
		public static Vector2 cameraSize { get; set; } = new Vector2(5, 5);

		public static Vector2 ScreenToWorld(Vector2 screen)
		{
			var screenSize = Project.instance.mainPanel.GetScreenSize();
			var cameraSize = Camera.cameraSize;
			cameraSize = new Vector2(cameraSize.x * screenSize.x / screenSize.y, cameraSize.y);
			screen -= screenSize * 0.5f;
			screen = screen / screenSize * cameraSize;
			screen.y = -screen.y;
			screen += cameraPosition;
			return screen;
		}

		public static Vector2 WorldToScreen(Vector2 world)
		{
			world -= cameraPosition;

			var screenSize = Project.instance.mainPanel.GetScreenSize();

			var cameraSize = Camera.cameraSize;
			cameraSize = new Vector2(cameraSize.x * screenSize.x / screenSize.y, cameraSize.y);

			world.y = -world.y;

			var screenSpace = world / cameraSize * screenSize;
			screenSpace = screenSpace + screenSize * 0.5f;

			return screenSpace;
		}

		public static Rect ScreenToWorld(Rect screen)
		{
			var screenSize = Project.instance.mainPanel.GetScreenSize();
			var cameraSize = Camera.cameraSize;
			cameraSize = new Vector2(cameraSize.x * screenSize.x / screenSize.y, cameraSize.y);

			screen = Rect.FromCenterAndSize(screen.center - screenSize * 0.5f, screen.size);
			screen = new Rect(screen.min / screenSize * cameraSize, screen.max / screenSize * cameraSize);
			screen.min.y = -screen.min.y;
			screen.max.y = -screen.max.y;
			Mathf.Swap(ref screen.min.y, ref screen.max.y);
			screen.min += cameraPosition;
			screen.max += cameraPosition;
			return screen;
		}

		public static Rect WorldToScreen(Rect world)
		{
			world.min -= cameraPosition;
			world.max -= cameraPosition;

			var screenSize = Project.instance.mainPanel.GetScreenSize();

			var cameraSize = Camera.cameraSize;
			cameraSize = new Vector2(cameraSize.x * screenSize.x / screenSize.y, cameraSize.y);

			world.min.y = -world.min.y;
			world.max.y = -world.max.y;
			Mathf.Swap(ref world.min.y, ref world.max.y);

			var screenSpace = new Rect(world.min / cameraSize * screenSize, world.max / cameraSize * screenSize);
			screenSpace = Rect.FromCenterAndSize(screenSpace.center + screenSize * 0.5f, screenSpace.size);

			return screenSpace;
		}
	}
}
