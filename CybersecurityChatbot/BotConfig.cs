namespace CybersecurityChatbot
{
    public class BotConfig
    {
        // Automatic properties - required for Part 1
        public string BotName { get; set; } = "Bot";
        public string Version { get; } = "1.0";
        public int TypingDelayMs { get; set; } = 25;
        public bool EnableVoiceGreeting { get; set; } = true;

        // Read-only automatic property
        public string DataFolder { get; } = "UserData";
        public string WelcomeMessage { get; set; } = "Welcome to the Cybersecurity Awareness Bot";
    }
}