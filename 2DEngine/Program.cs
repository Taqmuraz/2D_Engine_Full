using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using _2DEngine.SystemDrawing;

namespace _2DEngine
{
	static class Program
	{
		/// <summary>
		/// Главная точка входа для приложения.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			try
			{
				new SystemDrawingProject();
				Application.Run(new MainWindow());
			}
			catch (Exception ex)
			{
				new ErrorForm(ex).ShowDialog();
			}
			Game.GameThread.isPlaying = false;
		}
	}
	class ErrorForm : Form
	{
		public ErrorForm(Exception error)
		{
			Text = error.GetType().FullName;

			Label label = new Label();
			label.Parent = this;
			Size = new System.Drawing.Size(600, 400);
			label.SetBounds(0, 0, 600, 400);
			label.Text = error.ToString();
		}
	}
}
