namespace _2DEngine.Game
{
	public static class WaveSpawner
	{
		class WavesNumberDrawer : IDrawHandler
		{
			int IDrawHandler.queue => int.MaxValue;
			bool IDrawHandler.isActive => true;

			void IDrawHandler.Draw(IGraphics graphics)
			{
				Vector2 screenSize = Project.instance.mainPanel.GetScreenSize();
				graphics.DrawString(new Rect(50f, screenSize.y * 0.85f, 150f, 150f), "Волна " + waveIndex, Color32.red);
			}
		}

		static WaveSpawner()
		{
			Project.instance.mainPanel.AddDrawHandler(new WavesNumberDrawer());
		}

		public static void OnWaveDeath()
		{
			if (Soldier_Wave_BOTController.waveAlive <= 0)
			{
				SpawnWave();
			}
		}

		static int waveNumber = 30;
		static int waveIndex = 0;

		public static void Reset()
		{
			waveNumber = 30;
			waveIndex = 0;
		}

		public static void SpawnWave()
		{
			Soldier_Wave_BOTController.waveAlive = 0;
			waveIndex++;
			int num = waveNumber += 5;
			for (int i = 0; i < num; i++)
			{
				int p = i & 1;
				var bot = new GameObject("BOT_NG").AddComponent<Soldier_Wave_BOTController>();
				bot.transform.position = new Vector2(Random.Range(-100f, 100f), Random.Range(-100f, 100f));

				if (bot.transform.position == Vector2.zero)
				{
					float angle = Random.Range(0f, 360f);
					bot.transform.position = new Vector2(angle.Cos(), angle.Sin()) * Random.Range(25f, 100f);
				}
				else if (bot.transform.position.length < 25f) bot.transform.position = bot.transform.position.normalized * Random.Range(25f, 100f);

				bot.transform.forward = p == 0 ? Vector2.right : Vector2.left;
			}
		}
	}
}
