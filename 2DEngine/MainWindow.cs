using System.ComponentModel;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DEngine
{
	public partial class MainWindow : Form
	{
		IMainPanel panel;

		public MainWindow()
		{
			InitializeComponent();

			panel = Project.instance.CreateMainPanel(this);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			panel.OnWindowKeyDown(e);
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			panel.OnWindowKeyUp(e);
		}
	}
}
