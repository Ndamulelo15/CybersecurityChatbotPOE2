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
                string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greet.wav");

                if (!File.Exists(audioPath))
                {
                    audioPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "greet.wav");
                }

                if (File.Exists(audioPath))
                {
                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.PlaySync();
                    }
                }
            }
            catch (Exception)
            {
                // Silent fail - application continues normally
            }
        }
    }
}