using System.Windows.Forms;

namespace _2DEngine
{
	public abstract class Project
	{
		public static Project instance { get; private set; }

		public IMainPanel mainPanel;

		protected Project()
		{
			instance = this;
		}

		public static void Log(string message)
		{
			if (instance != null && instance.mainPanel != null) instance.mainPanel.Log(message);
		}

		public IMainPanel CreateMainPanel(Form form)
		{
			mainPanel = OnCreateMainPanel(form);
			Game.GameThread.Start();
			return mainPanel;
		}

		protected abstract IMainPanel OnCreateMainPanel(Form form);
	}
}
