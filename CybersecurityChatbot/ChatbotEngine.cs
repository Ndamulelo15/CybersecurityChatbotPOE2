using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CybersecurityChatbot
{
    // Delegate for sentiment-based response modification - Part 2 requirement
    public delegate string SentimentResponseDelegate(string response, string sentiment);

    public class ChatbotEngine
    {
        private Dictionary<string, List<string>> responses;
        private List<string> fallbackMessages;
        private Random random;
        private string currentTopic;
        private int followUpCount;
        private string currentUser;
        private BotConfig config;

        // Delegate instance - Part 2 requirement
        public SentimentResponseDelegate AddEmpathy;

        public ChatbotEngine()
        {
            config = new BotConfig();
            random = new Random();
            currentTopic = null;
            followUpCount = 0;
            InitializeResponses();

            // Assign delegate method
            AddEmpathy = AddEmpatheticPrefix;
        }

        private void InitializeResponses()
        {
            responses = new Dictionary<string, List<string>>();

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

        public string ProcessInput(string input, string userName, out string detectedSentiment)
        {
            detectedSentiment = DetectSentiment(input);
            string lowerInput = input.ToLower();

            // Check for exit
            if (lowerInput == "exit" || lowerInput == "goodbye")
            {
                return $"Thank you for using {config.BotName}. Remember to stay vigilant online. Goodbye.";
            }

            // Check for help
            if (lowerInput == "help" || lowerInput == "topics")
            {
                return "You can ask me about: passwords, phishing, safe browsing, scams, privacy, 2FA, malware, or VPNs.";
            }

            // Check for "what can I ask about"
            if (lowerInput.Contains("what can i ask") || lowerInput.Contains("what questions"))
            {
                return GetRandomResponse("ask about");
            }

            // Check for follow-up request
            if (IsFollowUpRequest(lowerInput) && !string.IsNullOrEmpty(currentTopic))
            {
                followUpCount++;
                string response = GetRandomResponse(currentTopic);
                return ApplyDelegate(response, detectedSentiment);
            }

            // Check for interest statement
            if (lowerInput.Contains("interested in"))
            {
                SaveUserInterest(userName, input);
                string response = "Great. I have noted your interest. I will keep that in mind for our future conversations.";
                return ApplyDelegate(response, detectedSentiment);
            }

            // Check for specific topics
            string matchedTopic = MatchTopic(lowerInput);

            if (matchedTopic != null)
            {
                currentTopic = matchedTopic;
                followUpCount = 0;
                string response = GetRandomResponse(matchedTopic);
                return ApplyDelegate(response, detectedSentiment);
            }

            // Check for greeting
            if (lowerInput.Contains("hello") || lowerInput.Contains("hi") || lowerInput.Contains("hey"))
            {
                return GetRandomResponse("greeting");
            }

            // Default fallback
            return ApplyDelegate(fallbackMessages[random.Next(fallbackMessages.Count)], detectedSentiment);
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

            if (input.Contains("purpose") || input.Contains("what do you do"))
                return "purpose";

            if (input.Contains("how are you"))
                return "greeting";

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
            return input.Contains("tell me more") ||
                   input.Contains("another tip") ||
                   input.Contains("more information") ||
                   input.Contains("explain more") ||
                   input.Contains("continue") ||
                   input.Contains("elaborate") ||
                   (input.Contains("more") && followUpCount < 2);
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

        private string AddEmpatheticPrefix(string response, string sentiment)
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

        private string ApplyDelegate(string response, string sentiment)
        {
            if (AddEmpathy != null && sentiment != "neutral")
            {
                return AddEmpathy(response, sentiment);
            }
            return response;
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
            catch (Exception)
            {
                // Silent fail
            }
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

        public string GetUserInterests(string userName)
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

        public void SetCurrentUser(string userName)
        {
            currentUser = userName;
        }

        public void ResetContext()
        {
            currentTopic = null;
            followUpCount = 0;
        }

        public bool IsReturningUser(string name)
        {
            string userFile = "users.txt";
            if (!File.Exists(userFile)) return false;

            string[] users = File.ReadAllLines(userFile);
            return users.Any(u => u.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void SaveUser(string name)
        {
            string userFile = "users.txt";
            if (!IsReturningUser(name))
            {
                File.AppendAllText(userFile, name + "\n");
            }
        }

        public string GetBotName()
        {
            return config.BotName;
        }
    }
}