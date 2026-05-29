using System;
using System.IO;
using System.Media;

namespace CybersecurityChatbot
{
    public class VoiceGreeting
    {
        public void Play()
        {

            try
            {
                // Try to locate greet.wav in the application's base directory
                string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greet.wav");


                // If not found, try the current working directory
                if (!File.Exists(audioPath))
                {
                    audioPath = Path.Combine(Environment.CurrentDirectory, "greet.wav");
                }

                // Play the audio if the file exists
                if (File.Exists(audioPath))
                {
                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.Load();
                        player.PlaySync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Optional: log error for debugging
                Console.WriteLine($"Audio playback error: {ex.Message}");
            }
        }
    }
}
