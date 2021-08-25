namespace _2DEngine.Game
{

	public static class Camera
	{
		public static Vector2 cameraPosition { get; set; }
		public static Matrix4x4 worldCurvature { get; private set; }


		static Camera()
		{
			worldCurvature = Matrix4x4.CreateScaleMatrix(new Vector3(1f, 1f / Mathf.Cos(45f), 1f));
		}

		static Vector2 m_size = new Vector2(15, 15);
		public static Vector2 cameraSizeWithAspect
		{
			get
			{
				return new Vector2(m_size.x * screenSize.x / screenSize.y, m_size.y);
			}
			set => m_size = value;
		}
		static Vector2 screenSize => Project.instance.mainPanel.GetScreenSize();

		public static Vector2 ScreenToWorld(Vector2 screen)
		{
			return (Vector2)ScreenToWorld().MultiplyPoint(screen);
		}

		public static Vector2 WorldToScreen(Vector2 world)
		{
			return (Vector2)WorldToScreen().MultiplyPoint(world);
		}

		public static Rect ScreenToWorld(Rect screen)
		{
			return screen.MultiplyMatrix(ScreenToWorld());
		}

		public static Rect WorldToScreen(Rect world)
		{
			return world.MultiplyMatrix(WorldToScreen());
		}

		public static Matrix4x4 CameraToScreen()
		{
			return FromPositionAndScale(screenSize * 0.5f, new Vector2(screenSize.x, -screenSize.y));
		}
		public static Matrix4x4 CameraToWorld()
		{
			return worldCurvature * FromPositionAndScale(cameraPosition, new Vector2(cameraSizeWithAspect.x, cameraSizeWithAspect.y));
		}
		public static Matrix4x4 WorldToCamera()
		{
			return CameraToWorld().GetInversed();
		}
		public static Matrix4x4 ScreenToCamera()
		{
			return CameraToScreen().GetInversed();
		}

		public static Matrix4x4 WorldToScreen()
		{
			return WorldToCamera() * CameraToScreen();
		}
		public static Matrix4x4 ScreenToWorld()
		{
			return WorldToScreen().GetInversed();
		}

		static Matrix4x4 FromPositionAndScale(Vector2 position, Vector2 scale)
		{
			return Matrix4x4.CreateWorldMatrix(new Vector3(scale.x, 0f, 0f), new Vector3(0f, scale.y, 0f), Vector3.forward, position);
		}
	}
}
