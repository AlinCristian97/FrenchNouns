using System;

namespace FrenchNouns
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Choisissez une option :");
                Console.WriteLine("1) Mot aléatoire");
                Console.WriteLine("2) Mot aléatoire par lettre");
                Console.WriteLine("3) Entrer un mot");
                Console.WriteLine("Q) Quitter");

                var choice = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(choice))
                {
                    Console.WriteLine("Entrée invalide.\n");
                    continue;
                }

                if (string.Equals(choice, "Q", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                if (choice == "1")
                {
                    if (NounRepository.TryGetRandom(out var word))
                    {
                        DisplayWordWithMetadata(word);
                    }
                    else
                    {
                        Console.WriteLine("Aucun mot disponible.\n");
                    }

                    continue;
                }

                if (choice == "2")
                {
                    Console.Write("Entrez une lettre (a - z) : ");
                    var letterInput = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(letterInput) || !char.IsLetter(letterInput[0]))
                    {
                        Console.WriteLine("Lettre invalide.\n");
                        continue;
                    }

                    var letter = letterInput[0];
                    if (NounRepository.TryGetRandomByLetter(letter, out var wordByLetter))
                    {
                        DisplayWordWithMetadata(wordByLetter);
                    }
                    else
                    {
                        Console.WriteLine($"Aucun mot trouvé pour la lettre '{char.ToLowerInvariant(letter)}'.\n");
                    }

                    continue;
                }

                if (choice == "3")
                {
                    Console.Write("Entrez le mot : ");
                    var userInput = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(userInput))
                    {
                        Console.WriteLine("Entrée invalide.\n");
                        continue;
                    }

                    // normalize to canonical "article + space + noun" when possible
                    var canonical = ResolveToCanonical(userInput);

                    // check existence in repository (case-insensitive)
                    var exists = false;
                    foreach (var entry in NounRepository.All)
                    {
                        if (string.Equals(entry, canonical, StringComparison.OrdinalIgnoreCase))
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                    {
                        Console.WriteLine("Le mot n'existe pas ou n'a pas été implémenté.\n");
                        continue;
                    }

                    DisplayWordWithMetadata(canonical);

                    continue;
                }

                Console.WriteLine("Choix invalide.\n");
            }
        }

        private static void DisplayWordWithMetadata(string word, int exampleCount = Constants.NumberOfRandomExampleSentences)
        {
            Console.WriteLine(Constants.LongDivider);
            PrintColoredWord(word);
            Console.WriteLine();
            Console.WriteLine(Constants.ShortDivider);
            PrintDescription(word);
            Console.WriteLine(Constants.ShortDivider);
            PrintExampleSentences(word, exampleCount);
            Console.WriteLine(Constants.LongDivider);
        }

        private static void PrintColoredWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                Console.Write(word);
                return;
            }

            var parts = word.Split(Constants.Space, 2, StringSplitOptions.None);
            var article = parts.Length > 0 ? parts[0] : string.Empty;
            var rest = parts.Length > 1 ? parts[1] : string.Empty;

            var previous = Console.ForegroundColor;

            // Article: blue for "un", red for "une"
            if (!string.IsNullOrEmpty(article))
            {
                Console.Write(Constants.GuillemetOuvrant + Constants.Space);

                if (string.Equals(article, Constants.Un, StringComparison.Ordinal))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else if (string.Equals(article, Constants.Une, StringComparison.Ordinal))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = previous;
                }

                Console.Write(article);
            }

            // Word itself in yellow
            if (!string.IsNullOrEmpty(rest))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;

                if (!string.IsNullOrEmpty(article))
                {
                    Console.Write(Constants.Space + rest);
                }
                else
                {
                    Console.Write(rest);
                }
                
                Console.ResetColor();
                Console.Write(Constants.Space + Constants.GuillemetFermant);
            }

            // Restore console color
            Console.ForegroundColor = previous;
        }

        private static void PrintExampleSentences(string word, int count = 3)
        {
            if (string.IsNullOrWhiteSpace(word) || count <= 0)
                return;

            var sentences = NounRepository.GetSentencesForWord(word, count);
            if (sentences == null || sentences.Count == 0)
                return;

            var previous = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            foreach (var s in sentences)
            {
                Console.WriteLine($"{Constants.GuillemetOuvrant}{Constants.Space}{s}{Constants.Space}{Constants.GuillemetFermant}");
            }
            Console.ForegroundColor = previous;
        }

        private static void PrintDescription(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return;

            var description = NounRepository.GetDescriptionForWord(word);
            if (string.IsNullOrWhiteSpace(description))
                return;

            var previous = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(description);
            Console.ForegroundColor = previous;
        }

        private static string ResolveToCanonical(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input ?? string.Empty;

            var normalized = input.Trim();
            var parts = normalized.Split(Constants.Space, 2, StringSplitOptions.RemoveEmptyEntries);
            var nounPart = parts.Length > 1 ? parts[1] : parts[0];

            // search repository for a matching noun (case-insensitive)
            foreach (var entry in NounRepository.All)
            {
                var eParts = entry.Split(Constants.Space, 2, StringSplitOptions.RemoveEmptyEntries);
                var entryNoun = eParts.Length > 1 ? eParts[1] : eParts[0];
                if (string.Equals(entryNoun, nounPart, StringComparison.OrdinalIgnoreCase))
                    return entry; // return canonical "article + space + noun" from repository
            }

            // if user supplied an article (e.g., "un arbre") normalize article + noun casing/spacing
            if (parts.Length > 1)
            {
                var article = parts[0].ToLowerInvariant();
                var noun = nounPart.ToLowerInvariant();
                if (article == Constants.Un || article == Constants.Une)
                    return article + Constants.Space + noun;
            }

            // fallback: return trimmed input unchanged
            return normalized;
        }
    }
}