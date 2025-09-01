using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WordDefinitionApp
{
    class Program
    {
        // Simple dictionary with common word definitions
        private static readonly Dictionary<string, string> WordDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"apple", "A round fruit with red, green, or yellow skin and white flesh"},
            {"book", "A set of written or printed pages bound together"},
            {"cat", "A small domesticated carnivorous mammal with soft fur"},
            {"dog", "A domesticated carnivorous mammal that typically has a long snout"},
            {"house", "A building for human habitation"},
            {"water", "A colorless, transparent, odorless liquid that forms seas, lakes, rivers, and rain"},
            {"tree", "A woody perennial plant, typically having a main trunk"},
            {"car", "A road vehicle, typically with four wheels, powered by an internal combustion engine"},
            {"computer", "An electronic device for storing and processing data"},
            {"phone", "A device used for transmitting speech over a distance"},
            {"love", "An intense feeling of deep affection"},
            {"happy", "Feeling or showing pleasure or contentment"},
            {"sad", "Feeling or showing sorrow; unhappy"},
            {"run", "Move at a speed faster than a walk"},
            {"walk", "Move at a regular pace by lifting and setting down each foot"},
            {"sun", "The star around which the earth orbits"},
            {"moon", "The natural satellite of the earth"},
            {"ocean", "A very large expanse of sea"},
            {"mountain", "A large natural elevation of the earth's surface"},
            {"river", "A large natural stream of water flowing in a channel"}
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Word Definition App!");
            Console.WriteLine("Enter a single word to get its definition.");
            Console.WriteLine();

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

                // Look up the word definition
                string word = input.Trim().ToLower();
                if (WordDictionary.TryGetValue(word, out string? definition))
                {
                    Console.WriteLine($"\nDefinition of '{word}': {definition}");
                }
                else
                {
                    Console.WriteLine($"\nSorry, I don't have a definition for '{word}' in my dictionary.");
                    Console.WriteLine("Try words like: apple, book, cat, dog, house, water, etc.");
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
