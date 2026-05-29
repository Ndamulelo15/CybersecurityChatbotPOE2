# CybersecurityChatbot

 WPF cybersecurity awareness chatbot built in C# (.NET). It chats with the user about common cybersecurity topics (passwords, phishing, malware, safe browsing, social engineering, 2FA, scams, privacy, etc.) and greets the user with a voice greeting (greeting.wav).

✨ Features
🎵 Voice greeting: plays greeting.wav (configured in the project)
💬 Interactive chat UI: messages displayed in the WPF window
🧠 Keyword-based responses: the chatbot uses predefined topic matching
🔒 Cybersecurity tips: practical awareness guidance
📋 Project Structure
CyberSecurityChatbot-master/
├── MainWindow.xaml            # Chat UI
├── MainWindow.xaml.cs         # UI logic / message list
├── Chatbot.cs                 # Chat logic: GetResponse()
├── ChatMessage.cs            # Model for chat items
├── greeting.wav              # Voice greeting audio file
├── CyberSecurityChatbot.csproj  # WPF project file
├── CyberSecurityChatbot.slnx    # Solution file
├── .github/workflows/dotnet.yml # GitHub Actions workflow
└── TODO.md                    # Task tracking
🔄 CI/CD Pipeline (GitHub Actions)
Workflow file: .github/workflows/dotnet.yml

Triggers: push / pull_request to main and master
Runner: windows-latest
SDK: 10.0.x
Jobs: restore → build (Release) → publish → upload artifact
🛠️ Prerequisites
.NET SDK 10.0+
Windows (WPF)
🚀 Quick Start & Workflow Test
Local (Windows)
From CyberSecurityChatbot-master/:

dotnet restore
dotnet build -c Release
dotnet run
greeting.wav is already configured to be copied to the output by the .csproj.

GitHub Actions
Push commits to main or master
Check the repo Actions tab for build/publish results
💻 Usage (WPF UI)
Launch the app
Enter your name
Ask questions like:
tell me about passwords
how to avoid phishing?
what is malware
help (topic list)
🧪 Testing
This repo has no automated test projects yet.

Manual validation:

dotnet run
📸 Screenshots
(Add WPF UI screenshots here)

🔮 Roadmap
 Improve response logic (more topics / better matching)
 Better UX (load/focus/input polish)
 NLP enhancements (e.g., ML.NET)
 Multi-language support
🤝 Contributing
Fork the repo
Create feature branch (git checkout -b feature/AmazingFeature)
Commit changes (git commit -m 'Add some AmazingFeature')
Push (git push origin feature/AmazingFeature)
Open a Pull Request
📄 License
MIT License - see LICENSE file.

Stay safe online! 🔒
