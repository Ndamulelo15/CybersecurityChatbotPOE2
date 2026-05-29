# CybersecurityChatbot


## Overview

The **Cybersecurity ChatBot** is a C# WPF desktop application designed to educate users about cybersecurity threats and online safety practices. The chatbot provides guidance on topics such as phishing, password safety, malware prevention, privacy protection, VPN usage, and safe browsing habits.

The application includes:

* Interactive chatbot conversations
* Sentiment-aware responses
* User interest tracking
* Voice greeting functionality
* Follow-up conversation support
* Persistent user storage

---

# Features

## 1. Cybersecurity Guidance

The chatbot can answer questions about:

* Password safety
* Phishing attacks
* Safe browsing
* Scam awareness
* Privacy protection
* Two-factor authentication (2FA)
* Malware prevention
* VPN usage

---

## 2. Sentiment Detection

The bot detects user emotions and adjusts responses accordingly.

### Supported sentiments:

* Worried
* Frustrated
* Curious
* Confused
* Neutral

### Example:

User:

```text
I am worried about phishing scams
```

Bot:

```text
I understand your concern. Phishing attacks trick you into revealing sensitive information through fake emails or websites.
```

---

## 3. Follow-Up Conversations

The chatbot remembers the current topic and allows users to request more information using phrases like:

* Tell me more
* Another tip
* Explain more
* Continue
* Elaborate

---

## 4. User Interest Tracking

The system stores user interests in a text file:

```text
user_interests.txt
```

When a returning user logs in, the bot remembers their previous interests.

Example:

```text
Welcome back. I remember you are interested in phishing.
```

---

## 5. Voice Greeting

A WAV audio file (`greet.wav`) is played when the application starts.

---

# Technologies Used

* C#
* WPF (Windows Presentation Foundation)
* .NET
* XAML
* File Handling
* Delegates
* Dictionaries and Lists

---

# Project Structure

```text
CybersecurityChatbot/
│
├── BotConfig.cs
├── ChatbotEngine.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── VoiceGreeting.cs
├── greet.wav
├── users.txt
├── user_interests.txt
└── README.md
```

---

# Classes Overview

## BotConfig

Stores chatbot configuration settings.

### Properties:

* `BotName`
* `Version`
* `TypingDelayMs`
* `EnableVoiceGreeting`
* `DataFolder`
* `WelcomeMessage`

---

## ChatbotEngine

Handles:

* Response generation
* Sentiment detection
* Topic matching
* User interest storage
* Follow-up logic

### Delegate Used:

```csharp
public delegate string SentimentResponseDelegate(string response, string sentiment);
```

---

## MainWindow

Controls:

* User interface
* Chat display
* Message handling
* Navigation between screens

---

## VoiceGreeting

Responsible for playing the startup greeting audio.

---

# How the Chatbot Works

## Step 1: User Login

The user enters their name.

## Step 2: User Validation

The application checks:

* Name is not empty
* Minimum length is 2 characters
* Maximum length is 30 characters

## Step 3: Conversation Begins

The chatbot welcomes the user and displays available commands.

## Step 4: Input Processing

The chatbot:

1. Detects sentiment
2. Matches topic keywords
3. Generates a response
4. Applies empathy if necessary

---

# Supported Commands

| Command | Description            |
| ------- | ---------------------- |
| help    | Shows available topics |
| topics  | Shows available topics |
| exit    | Ends the conversation  |
| goodbye | Ends the conversation  |

---

# Supported Topics

| Topic         | Example                              |
| ------------- | ------------------------------------ |
| Passwords     | "How do I create a strong password?" |
| Phishing      | "What is phishing?"                  |
| Safe Browsing | "How can I browse safely?"           |
| Scams         | "How do online scams work?"          |
| Privacy       | "How can I protect my privacy?"      |
| 2FA           | "What is two-factor authentication?" |
| Malware       | "How can I avoid malware?"           |
| VPN           | "What is a VPN?"                     |

---

# File Storage

## users.txt

Stores usernames of returning users.

Example:

```text
John
Sarah
Michael
```

---

## user_interests.txt

Stores user interests.

Example:

```text
John|phishing
Sarah|privacy
```

---

# Error Handling

The application uses `try-catch` blocks to prevent crashes during:

* Audio playback
* File operations
* User data handling

Silent failure is used to ensure the application continues running smoothly.

---

# Installation Instructions

## Requirements

* Visual Studio
* .NET Framework / .NET SDK
* Windows OS

---

## Steps

1. Open the solution in Visual Studio
2. Build the project
3. Ensure `greet.wav` is in the project directory
4. Run the application

---

# Example Conversation

```text
User: What is phishing?

Bot: Phishing attacks trick you into revealing sensitive information through fake emails or websites.

User: Tell me more

Bot: Never click links in unsolicited messages. Hover over links to see the actual destination before clicking.
```

---

# Future Improvements

Possible future enhancements include:

* Database integration
* AI-powered responses
* Dark mode support
* More cybersecurity topics
* Speech-to-text support
* Multi-language support
* Improved NLP capabilities

---

# Author

Cybersecurity Awareness Bot Project

---

# License

This project is for educational purposes.

