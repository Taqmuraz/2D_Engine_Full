namespace _2DEngine.Game
{
	public static class TexturesLoader
	{
		public static void LoadTextures(string root)
		{
			string[] files = System.IO.Directory.GetFiles(root);

			for (int i = 0; i < files.Length; i++)
			{
				Project.instance.mainPanel.LoadAndBindTexture(files[i]);
			}

			string[] folders = System.IO.Directory.GetDirectories(root);

			for (int i = 0; i < folders.Length; i++)
			{
				LoadTextures(folders[i]);
			}
		}
	}
}
