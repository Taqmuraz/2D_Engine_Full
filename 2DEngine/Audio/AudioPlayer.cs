using System.Media;
using WMPLib;

namespace _2DEngine
{
	public class AudioPlayer
	{
		string soundName;
		float volume;
		IAudioPlayer player;

		public AudioPlayer(string soundName, float volume)
		{
			this.soundName = soundName;
			this.volume = volume;

			//player = CreatePlayer();
		}

		IAudioPlayer CreatePlayer()
		{
			var lower = soundName.ToLower();
			IAudioPlayer player;
			if (lower.EndsWith(".wav")) player = new WAVPlayer(soundName);
			else if (lower.EndsWith(".mp3")) player = new MP3Player(soundName);
			else throw new System.ArgumentException("This format can't be played");

			player.volume = volume * 0.5f;

			return player;
		}

		public void Play()
		{
			//player.Play();
		}
	}
	interface IAudioPlayer
	{
		void Play();
		float volume { get; set; }
	}
	class MP3Player : IAudioPlayer
	{
		//WindowsMediaPlayer player;
		//string file;
		public MP3Player(string soundName)
		{
			/*file = "./Data/Sounds/" + soundName;
			player = new WindowsMediaPlayer();
			player.EndOfStream += r => player.close();
			try
			{
				player.URL = file;
				player.controls.stop();
			}
			catch
			{
			}*/
		}

		public void Play()
		{
			
		}

		public float volume
		{
			get => 0f;// player.settings.volume;
			set => value = 0f;// player.settings.volume = (int)(value * 100f);
		}
	}
	class WAVPlayer : IAudioPlayer
	{
		SoundPlayer player;
		public WAVPlayer(string soundName)
		{
			player = new SoundPlayer("./Data/Sounds/" + soundName);
		}
		public void Play()
		{
			player.Play();
		}
		public float volume
		{
			get => 0f;
			set => value = 0f;
		}
	}
}