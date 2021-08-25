using System.Windows.Forms;

namespace _2DEngine
{
	public interface IMainPanel
	{
		void AddDrawHandler(IDrawHandler handler);
		void AddInputHandler(IInputHandler handler);
		void OnWindowKeyDown(KeyEventArgs e);
		void OnWindowKeyUp(KeyEventArgs e);
		void RemoveDrawHandler(IDrawHandler handler);
		void RemoveInputHandler(IInputHandler handler);

		Texture LoadAndBindTexture (string fileName);

		void Log (string message);

		Vector2 GetScreenSize ();

		void DrawCall();
	}
}