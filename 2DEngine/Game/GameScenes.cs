using System.Collections.Generic;

namespace _2DEngine.Game
{
	public class IntroText : BackgroundTextRenderer
	{
		ITimedResolver loadGameResolver;

		[BehaviourEvent]
		void Start()
		{
			loadGameResolver = new TimedResolver(5f);
		}

		[BehaviourEvent]
		void Update()
		{
			if (loadGameResolver.resolving)
			{
				GameScenes.LoadScene(2);
			}
		}

		protected override Color32 GetColor()
		{
			return new Color32(Mathf.Sin(180f * loadGameResolver.readyProgress), 0f, 0f, 1f);
		}

		protected override string GetText()
		{
			return "Пять красноармейцев";
		}
	}

	public abstract class BackgroundTextRenderer : Renderer
	{
		protected override int queue => int.MaxValue;
		protected override bool isActive => true;

		SingleImage image;

		[BehaviourEvent]
		void Start()
		{
			image = new SingleImage(GetBackgroundTexture());
		}

		protected abstract string GetText();
		protected virtual Color32 GetColor()
		{
			return new Color32(1f, 1f, 1f, 1f);
		}
		protected virtual string GetBackgroundTexture()
		{
			return "./Data/Textures/Colors/Black.png";
		}


		protected override void Draw(IGraphics graphics)
		{
			var screenSize = Project.instance.mainPanel.GetScreenSize();
			image.Draw(graphics, new Rect(0f, 0f, screenSize.x, screenSize.y));
			graphics.DrawString(new Rect(0f, 0f, screenSize.x, screenSize.y), GetText(), GetColor());
		}
	}

	public class LoadingScreenText : BackgroundTextRenderer
	{
		ITimedResolver loadSceneResolver;
		public int sceneToLoad { get; set; }

		[BehaviourEvent]
		void Start()
		{
			loadSceneResolver = new TimedResolver(0.5f);
		}

		[BehaviourEvent]
		void Update()
		{
			if (loadSceneResolver.resolving) GameScenes.gameScenes[sceneToLoad].LoadScene();
		}

		protected override string GetText()
		{
			return "Загрузка...";
		}
	}

	public static class GameScenes
	{
		public static ReadOnlyArrayList<GameScene> gameScenes { get; private set; }
		static int sceneToLoad;


		public static void LoadScene(int index)
		{
			sceneToLoad = index;
			gameScenes[0].LoadScene();
		}

		static List<GameScene> scenesList = new List<GameScene>();
		static GameScenes()
		{
			gameScenes = scenesList;
			scenesList.Add(Scene_LoadingScreen());
			scenesList.Add(Scene_Intro());
			scenesList.Add(Scene_Game());
		}

		static GameScene Scene_Intro()
		{
			GameScene intro = new GameScene();
			intro.AddGameObjectInstancer(() => new GameObject("Intro text").AddComponent<IntroText>().gameObject);
			return intro;
		}
		static GameScene Scene_Game()
		{
			GameScene game = new GameScene();
			game.AddGameObjectInstancer(() => new GameObject("Player_SR").AddComponent<SoldierPlayerController>().gameObject);

			for (int i = 0; i < 5; i++)
			{
				int index = i;
				game.AddGameObjectInstancer(() =>
				{
					var bot = new GameObject("BOT_SR").AddComponent<Soldier_Protect_BOTController>();

					float angle = index * (360f / 5f);
					bot.transform.position = new Vector2(angle.Cos(), angle.Sin()) * 3f;

					bot.transform.forward = bot.transform.position;
					return bot.gameObject;
				});
			}

			/*game.AddGameObjectInstancer(() =>
			{
				var bot = new GameObject("Tank_NG").AddComponent<Tank_AI_Controller>();
				bot.transform.position = new Vector2(25f, 13f);
				bot.transform.forward = Vector2.left;
				return bot.gameObject;
			});*/
			/*game.AddGameObjectInstancer(() =>
			{
				var room = new GameObject("Room").AddComponent<RoomRenderer>();
				room.wallTexture = Project.instance.mainPanel.LoadAndBindTexture("./Data/Textures/Walls/Walls_1.png");
				room.floorTexture = Project.instance.mainPanel.LoadAndBindTexture("./Data/Textures/Floors/Floors_1.png");
				room.sizeX = 5;
				room.sizeY = 8;
				room.floorMapSizeX = 2;
				room.floorMapSizeY = 6;
				room.floorTextureOffset = 5;
				room.wallMapSizeX = 3;
				room.wallMapSizeY = 4;
				
				return room.gameObject;
			});*/

			/*for (int i = 0; i < 50; i++)
			{
				game.AddGameObjectInstancer(() =>
				{
					var tree = new GameObject($"Tree_{i}");

					tree.transform.position = new Vector2(Random.Range(-25f, 25f), Random.Range(-25f, 25f));

					var image = tree.AddComponent<ImageRenderer>();
					image.image = new SingleImage($"./Data/Textures/Trees/Tree ({Random.Range(1, 7)}).png");

					image.worldRect = Rect.FromCenterAndSize(tree.transform.position, new Vector2(2f, 2f));
					var collider = tree.AddComponent<CircleCollider>();
					collider.radius = 0.5f;
					collider.center = new Vector2(0f, -0.5f);
					return tree;
				});
			}*/


			game.AddGameObjectInstancer(() => new GameObject("Terrain").AddComponent<TerrainRenderer>().gameObject);
			return game;
		}
		static GameScene Scene_LoadingScreen()
		{
			GameScene loading = new GameScene();
			loading.AddGameObjectInstancer(() =>
			{
				var text = new GameObject("Loading").AddComponent<LoadingScreenText>();
				text.sceneToLoad = sceneToLoad;
				return text.gameObject;
			});
			return loading;
		}
	}
}
