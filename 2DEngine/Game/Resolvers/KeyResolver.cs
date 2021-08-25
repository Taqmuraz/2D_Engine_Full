namespace _2DEngine
{
	public class KeyResolver : IResolver
	{
		private KeyCode keyCode;

		public KeyResolver(KeyCode keyCode)
		{
			this.keyCode = keyCode;
		}

		bool IResolver.resolving
		{
			get { return Input.GetKey(keyCode); }
		}
	}
}