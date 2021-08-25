using System.Windows.Forms;
using _2DEngine;

namespace WinGL
{
	class OTKProject : Project
    {
        IMainPanel panel;

		public OTKProject(IMainPanel panel)
		{
			this.panel = panel;
		}

		protected override IMainPanel OnCreateMainPanel(Form form)
		{
            return panel;
		}
	}
}