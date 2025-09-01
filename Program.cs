using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;

namespace WordDefinitionApp
{
    class Program
    {
        private static OpenAIClient? _openAIClient;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to the Word Definition App!");
            Console.WriteLine("Enter a single word to get its definition powered by OpenAI.");
            Console.WriteLine();

            // Initialize OpenAI client
            string? apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("Error: OPENAI_API_KEY environment variable not set.");
                Console.WriteLine("Please set your OpenAI API key as an environment variable:");
                Console.WriteLine("export OPENAI_API_KEY=your_api_key_here");
                return;
            }

            _openAIClient = new OpenAIClient(apiKey);

            bool continueApp = true;

            while (continueApp)
            {
                Console.Write("Please enter a word: ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Error: Please enter a valid word.");
                    Console.WriteLine();
                    continue;
                }

                // Validate input - should be a single word with only letters
                if (!IsValidSingleWord(input))
                {
                    Console.WriteLine("Error: Please enter only a single word without numbers or special characters.");
                    Console.WriteLine();
                    continue;
                }

                // Look up the word definition using OpenAI
                string word = input.Trim();
                try
                {
                    Console.WriteLine($"\nLooking up definition for '{word}'...");
                    string definition = await GetWordDefinitionAsync(word);
                    Console.WriteLine($"Definition of '{word}': {definition}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nSorry, I couldn't get a definition for '{word}' at this time.");
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Please try again or try a different word.");
                }

                Console.WriteLine();

                // Ask if user wants to continue
                while (true)
                {
                    Console.Write("Would you like to look up another word? (y/n): ");
                    string? response = Console.ReadLine()?.Trim().ToLower();

                    if (response == "y" || response == "yes")
                    {
                        Console.WriteLine();
                        break;
                    }
                    else if (response == "n" || response == "no")
                    {
                        continueApp = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please enter 'y' for yes or 'n' for no.");
                    }
                }
            }

            Console.WriteLine("Thank you for using the Word Definition App!");
        }

        private static async Task<string> GetWordDefinitionAsync(string word)
        {
            if (_openAIClient == null)
                throw new InvalidOperationException("OpenAI client not initialized");

            var chatClient = _openAIClient.GetChatClient("gpt-3.5-turbo");
            
            var messages = new List<ChatMessage>
            {
                ChatMessage.CreateSystemMessage("You are a helpful dictionary assistant. Provide clear, concise definitions for words. Keep definitions to 1-2 sentences and focus on the most common meaning."),
                ChatMessage.CreateUserMessage($"Define the word '{word}' in simple terms.")
            };

            var completion = await chatClient.CompleteChatAsync(messages);
            
            if (completion?.Value?.Content?.Count > 0)
            {
                return completion.Value.Content[0].Text.Trim();
            }
            
            throw new Exception("No definition received from OpenAI");
        }

        private static bool IsValidSingleWord(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            string trimmed = input.Trim();

            // Check if it contains spaces (multiple words)
            if (trimmed.Contains(' '))
                return false;

            // Check if it contains numbers
            if (Regex.IsMatch(trimmed, @"\d"))
                return false;

            // Check if it contains only letters (allowing hyphens and apostrophes for compound words)
            if (!Regex.IsMatch(trimmed, @"^[a-zA-Z'-]+$"))
                return false;

            return true;
        }
    }
}
