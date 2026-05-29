using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CybersecurityChatbot
{
    public partial class MainWindow : Window
    {
        // Chatbot engine and components
        private ChatbotEngine chatbot;
        private VoiceGreeting voiceGreeting;
        private string currentUserName;
        private bool waitingForName;

        // Response storage
        private Dictionary<string, List<string>> responses;
        private List<string> fallbackMessages;
        private Random random;
        private string currentTopic;
        private int followUpCount;

        public MainWindow()
        {
            InitializeComponent();
            InitializeChatbot();
        }

        private void InitializeChatbot()
        {
            chatbot = new ChatbotEngine();
            voiceGreeting = new VoiceGreeting();
            responses = new Dictionary<string, List<string>>();
            random = new Random();
            currentTopic = null;
            followUpCount = 0;
            waitingForName = true;

            InitializeResponses();

            // Play voice greeting on startup
            try { voiceGreeting.Play(); } catch (Exception) { }
        }

        private void InitializeResponses()
        {
            // Greeting responses
            responses["greeting"] = new List<string>
            {
                "I am functioning well. How are you doing today?",
                "All systems operational. Thank you for asking. How can I assist you with cybersecurity today?",
                "I am ready to help you stay safe online. What would you like to learn about?"
            };

            // Purpose responses
            responses["purpose"] = new List<string>
            {
                "My purpose is to educate South African citizens about cybersecurity threats and how to protect themselves online.",
                "I am designed to raise awareness about digital safety, including phishing, password security, and safe browsing habits.",
                "My mission is to help you navigate the digital world safely by providing practical cybersecurity guidance."
            };

            // What can I ask about
            responses["ask about"] = new List<string>
            {
                "You can ask me about password safety, phishing detection, safe browsing habits, scam awareness, privacy protection, two-factor authentication, malware prevention, and VPN usage.",
                "I can help with topics like creating strong passwords, identifying phishing emails, browsing safely, avoiding scams, protecting your privacy, setting up 2FA, preventing malware, and using VPNs."
            };

            // Password safety
            responses["password"] = new List<string>
            {
                "Create passwords that are at least 12 characters long, combining uppercase, lowercase, numbers, and symbols. Never reuse passwords across different accounts.",
                "Use a password manager to generate and store unique passwords. Enable two-factor authentication whenever available.",
                "Avoid using personal information like birthdates or pet names in your passwords. Consider using passphrases."
            };

            // Phishing
            responses["phishing"] = new List<string>
            {
                "Phishing attacks trick you into revealing sensitive information through fake emails or websites. Always verify the sender's email address.",
                "Never click links in unsolicited messages. Hover over links to see the actual destination before clicking.",
                "Legitimate companies never ask for your password or banking details via email. Contact the organization directly to verify."
            };

            // Safe browsing
            responses["safe browsing"] = new List<string>
            {
                "Use HTTPS websites and look for the padlock icon in your browser. Avoid public Wi-Fi for sensitive transactions.",
                "Keep your browser and extensions updated. Avoid downloading files from untrusted sources.",
                "Be cautious of pop-ups claiming your computer is infected. These are often scams."
            };

            // Scams
            responses["scam"] = new List<string>
            {
                "Scammers create urgency or fear to pressure you into quick decisions. Take time to verify unexpected requests.",
                "If something seems too good to be true, it probably is. Research before sharing personal information.",
                "Report scams to the South African Banking Risk Information Centre (SABRIC)."
            };

            // Privacy
            responses["privacy"] = new List<string>
            {
                "Review your privacy settings on social media platforms. Limit the personal information you share publicly.",
                "Use a Virtual Private Network (VPN) when accessing the internet from public locations.",
                "Regularly review which applications have access to your data and revoke permissions for unused apps."
            };

            // Two-factor authentication
            responses["2fa"] = new List<string>
            {
                "Two-factor authentication adds an extra layer of security by requiring a second verification method beyond your password.",
                "Enable 2FA on all accounts that support it. Use authenticator apps rather than SMS when possible."
            };

            // Malware
            responses["malware"] = new List<string>
            {
                "Keep your antivirus software updated and run regular system scans. Avoid downloading software from unofficial sources.",
                "Malware can be hidden in email attachments or fake software updates. Always verify the source before downloading."
            };

            // VPN
            responses["vpn"] = new List<string>
            {
                "A VPN encrypts your internet traffic, protecting your data from interception on public networks.",
                "Choose a reputable VPN provider that does not log your activity. Free VPNs may compromise your privacy."
            };

            // Fallback messages
            fallbackMessages = new List<string>
            {
                "I do not fully understand your question. Could you please rephrase or ask about password safety, phishing, safe browsing, scams, or privacy?",
                "My knowledge focuses on cybersecurity topics. Please ask me about password protection, phishing detection, or safe browsing practices.",
                "I am not certain how to respond to that. Would you like to learn about creating strong passwords or identifying phishing attempts?"
            };
        }

        // Proceed button - Navigate to username grid
        private void proceed(object sender, RoutedEventArgs e)
        {
            home_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
            usernames_input.Focus();
        }

        // Submit name button
        private void submit_name(object sender, RoutedEventArgs e)
        {
            string name = usernames_input.Text.Trim();

            // Validate name
            if (string.IsNullOrWhiteSpace(name))
            {
                error_message.Visibility = Visibility.Visible;
                error_message.Text = "Please enter a name to continue.";
                return;
            }

            if (name.Length < 2)
            {
                error_message.Visibility = Visibility.Visible;
                error_message.Text = "Please enter a name with at least 2 characters.";
                return;
            }

            if (name.Length > 30)
            {
                error_message.Visibility = Visibility.Visible;
                error_message.Text = "Please enter a name with fewer than 30 characters.";
                return;
            }

            // Hide error if visible
            error_message.Visibility = Visibility.Collapsed;

            // Save user name
            currentUserName = name;
            waitingForName = false;

            // Save user to file
            SaveUser(currentUserName);

            // Navigate to chat grid
            username_grid.Visibility = Visibility.Hidden;
            chat_grid.Visibility = Visibility.Visible;

            // Update user display
            user_display.Text = $"User: {currentUserName}";

            // Display welcome message
            AddBotMessage($"Hello {currentUserName}. Welcome to the Cybersecurity Awareness Bot.");

            // Check if returning user
            string interests = GetUserInterests(currentUserName);
            if (!string.IsNullOrEmpty(interests))
            {
                AddBotMessage($"Welcome back. I remember you are interested in {interests}.");
            }

            AddBotMessage("I am here to help you stay safe online.");
            AddBotMessage("");
            //AddBotMessage("You can ask me about:");
            //AddBotMessage("- Password safety");
            //AddBotMessage("- Phishing detection");
            //AddBotMessage("- Safe browsing");
            //AddBotMessage("- Scam awareness");
            //AddBotMessage("- Privacy protection");
            //AddBotMessage("- Two-factor authentication");
            //AddBotMessage("- Malware prevention");
            //AddBotMessage("- VPN usage");
            //AddBotMessage("");
            AddBotMessage("Type 'help' to see topics, or 'exit' to end the conversation.");

            question.Focus();
        }

        // Send button click
        private void send(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        // Enter key press in textbox
        private void question_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUserInput();
                e.Handled = true;
            }
        }

        private void ProcessUserInput()
        {
            string input = question.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                AddBotMessage("Please enter a message. I am here to help with cybersecurity questions.");
                question.Clear();
                return;
            }

            // Add user message to chat
            AddUserMessage(input);
            question.Clear();

            // Check for exit
            if (input.ToLower() == "exit" || input.ToLower() == "goodbye")
            {
                AddBotMessage($"Thank you for using the Cybersecurity Awareness Bot. Stay safe online, {currentUserName}!");
                return;
            }

            // Check for help
            if (input.ToLower() == "help" || input.ToLower() == "topics")
            {
                AddBotMessage("You can ask me about: passwords, phishing, safe browsing, scams, privacy, 2FA, malware, or VPNs.");
                return;
            }

            // Check for what can I ask
            if (input.ToLower().Contains("what can i ask") || input.ToLower().Contains("what questions"))
            {
                AddBotMessage(GetRandomResponse("ask about"));
                return;
            }

            // Detect sentiment
            string sentiment = DetectSentiment(input);

            // Check for follow-up
            if (IsFollowUpRequest(input) && !string.IsNullOrEmpty(currentTopic))
            {
                followUpCount++;
                string response = GetRandomResponse(currentTopic);
                AddBotMessage(AddEmpathyPrefix(response, sentiment));
                return;
            }

            // Check for interest statement
            if (input.ToLower().Contains("interested in"))
            {
                SaveUserInterest(currentUserName, input);
                string response = "Great. I have noted your interest. I will keep that in mind for our future conversations.";
                AddBotMessage(AddEmpathyPrefix(response, sentiment));
                return;
            }

            // Match topic
            string matchedTopic = MatchTopic(input.ToLower());

            if (matchedTopic != null)
            {
                currentTopic = matchedTopic;
                followUpCount = 0;
                string response = GetRandomResponse(matchedTopic);
                AddBotMessage(AddEmpathyPrefix(response, sentiment));
                return;
            }

            // Check for greeting
            if (input.ToLower().Contains("hello") || input.ToLower().Contains("hi") || input.ToLower().Contains("hey"))
            {
                AddBotMessage(GetRandomResponse("greeting"));
                return;
            }

            // Check for how are you
            if (input.ToLower().Contains("how are you"))
            {
                AddBotMessage(GetRandomResponse("greeting"));
                return;
            }

            // Check for purpose
            if (input.ToLower().Contains("purpose") || input.ToLower().Contains("what do you do"))
            {
                AddBotMessage(GetRandomResponse("purpose"));
                return;
            }

            // Default fallback
            string fallback = fallbackMessages[random.Next(fallbackMessages.Count)];
            AddBotMessage(AddEmpathyPrefix(fallback, sentiment));
        }

        private string MatchTopic(string input)
        {
            string[] topics = { "password", "phishing", "safe browsing", "scam", "privacy", "2fa", "malware", "vpn" };

            foreach (string topic in topics)
            {
                if (input.Contains(topic))
                {
                    return topic;
                }
            }
            return null;
        }

        private string GetRandomResponse(string topic)
        {
            if (responses.ContainsKey(topic) && responses[topic].Count > 0)
            {
                return responses[topic][random.Next(responses[topic].Count)];
            }
            return fallbackMessages[random.Next(fallbackMessages.Count)];
        }

        private bool IsFollowUpRequest(string input)
        {
            string lower = input.ToLower();
            return lower.Contains("tell me more") ||
                   lower.Contains("another tip") ||
                   lower.Contains("more information") ||
                   lower.Contains("explain more") ||
                   lower.Contains("continue") ||
                   lower.Contains("elaborate") ||
                   (lower.Contains("more") && followUpCount < 2);
        }

        private string DetectSentiment(string input)
        {
            string lower = input.ToLower();

            if (lower.Contains("worried") || lower.Contains("nervous") || lower.Contains("scared") || lower.Contains("anxious"))
                return "worried";

            if (lower.Contains("frustrated") || lower.Contains("annoying") || lower.Contains("difficult"))
                return "frustrated";

            if (lower.Contains("curious") || lower.Contains("interesting") || lower.Contains("tell me more"))
                return "curious";

            if (lower.Contains("confused") || lower.Contains("dont understand") || lower.Contains("not clear"))
                return "confused";

            return "neutral";
        }

        private string AddEmpathyPrefix(string response, string sentiment)
        {
            switch (sentiment)
            {
                case "worried":
                    return $"I understand your concern. {response}";
                case "frustrated":
                    return $"I appreciate your patience. Let me help. {response}";
                case "curious":
                    return $"That is an excellent question. {response}";
                case "confused":
                    return $"Let me clarify that for you. {response}";
                default:
                    return response;
            }
        }

        private void AddUserMessage(string message)
        {
            Border bubble = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(44, 123, 229)),
                CornerRadius = new CornerRadius(12, 12, 4, 12),
                Margin = new Thickness(50, 4, 10, 4),
                Padding = new Thickness(12, 8, 12, 8)
            };

            TextBlock text = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 13,
                FontFamily = new FontFamily("Segoe UI")
            };

            bubble.Child = text;
            chats.Items.Add(bubble);
            ScrollToBottom();
        }

        private void AddBotMessage(string message)
        {
            Border bubble = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(12, 12, 12, 4),
                Margin = new Thickness(10, 4, 50, 4),
                Padding = new Thickness(12, 8, 12, 8)
            };

            StackPanel content = new StackPanel();

            TextBlock nameText = new TextBlock
            {
                Text = "Cybersecurity Bot",
                Foreground = new SolidColorBrush(Color.FromRgb(30, 111, 92)),
                FontWeight = FontWeights.Bold,
                FontSize = 11,
                Margin = new Thickness(0, 0, 0, 4),
                FontFamily = new FontFamily("Segoe UI")
            };

            TextBlock messageText = new TextBlock
            {
                Text = message,
                Foreground = Brushes.Black,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 13,
                FontFamily = new FontFamily("Segoe UI")
            };

            content.Children.Add(nameText);
            content.Children.Add(messageText);
            bubble.Child = content;

            chats.Items.Add(bubble);
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            if (chats.Items.Count > 0)
            {
                chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
            }
        }

        private void SaveUser(string name)
        {
            string userFile = "users.txt";
            if (!File.Exists(userFile))
            {
                File.AppendAllText(userFile, name + "\n");
            }
            else
            {
                string[] users = File.ReadAllLines(userFile);
                bool exists = false;
                foreach (string u in users)
                {
                    if (u.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    File.AppendAllText(userFile, name + "\n");
                }
            }
        }

        private void SaveUserInterest(string userName, string input)
        {
            try
            {
                string interestFile = "user_interests.txt";
                string interest = ExtractInterest(input);

                if (!string.IsNullOrEmpty(interest))
                {
                    string line = $"{userName}|{interest}";

                    if (File.Exists(interestFile))
                    {
                        string[] lines = File.ReadAllLines(interestFile);
                        bool found = false;

                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i].StartsWith(userName + "|"))
                            {
                                lines[i] = line;
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            File.AppendAllText(interestFile, line + "\n");
                        }
                        else
                        {
                            File.WriteAllLines(interestFile, lines);
                        }
                    }
                    else
                    {
                        File.AppendAllText(interestFile, line + "\n");
                    }
                }
            }
            catch (Exception) { }
        }

        private string ExtractInterest(string input)
        {
            string lower = input.ToLower();
            if (lower.Contains("interested in"))
            {
                int index = lower.IndexOf("interested in") + 13;
                if (index < lower.Length)
                {
                    string interest = lower.Substring(index).Trim();
                    string[] words = interest.Split(' ');
                    if (words.Length > 0)
                    {
                        return words[0];
                    }
                }
            }
            return null;
        }

        private string GetUserInterests(string userName)
        {
            string interestFile = "user_interests.txt";
            if (!File.Exists(interestFile)) return null;

            string[] lines = File.ReadAllLines(interestFile);
            foreach (string line in lines)
            {
                if (line.StartsWith(userName + "|"))
                {
                    return line.Split('|')[1];
                }
            }
            return null;
        }
    }
}