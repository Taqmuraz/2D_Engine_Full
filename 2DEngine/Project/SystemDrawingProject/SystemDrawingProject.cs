using System.Windows.Forms;

namespace _2DEngine.SystemDrawing
{
	public sealed class SystemDrawingProject : Project
	{
		protected override IMainPanel OnCreateMainPanel(Form form)
		{
			var panel = new DrawPanel();
			panel.Parent = form;

			OnResize(panel, form);
			form.SizeChanged += (s, e) => OnResize(panel, form);

			return panel;
		}
		void OnResize(DrawPanel panel, Form form)
		{
			panel.SetBounds(0, 0, form.Width, form.Height);
		}
	}
}
