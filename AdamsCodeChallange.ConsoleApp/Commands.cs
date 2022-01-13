using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdamsCodeChallange.ConsoleApp
{
    public static class Commands
    {
        // Ask Ben for these variables
        private static string _uri = "";
        private static string _user = "";
        private static string _password = "";



        public static async Task PromptAndCompare()
        {
            string word1, word2;
            PromptForWords(out word1, out word2);

            if (ValidateWords(word1, word2))
            {
                var driver = new Neo4JDriver(_uri, _user, _password);
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                var wordOneExists = await driver.WordExists(word1);
                var wordTwoExists = await driver.WordExists(word2);

                if (wordOneExists && wordTwoExists)
                {
                    await driver.CompareWords(word1, word2);
                }
                else if (!wordOneExists && wordTwoExists)
                {
                    Console.WriteLine($"{word1} is not in the dictionary, however {word2} is.");
                }
                else if (wordOneExists && !wordTwoExists)
                {
                    Console.WriteLine($"{word2} is not in the dictionary, however {word1} is.");
                }
                else
                {
                    Console.WriteLine($"Neither {word1} nor {word2} exists in the dictionary");
                }

                watch.Stop();
                Console.WriteLine($"Took {watch.ElapsedMilliseconds} miliseconds. ({watch.ElapsedMilliseconds / 1000L } seconds)");
            }
        }

        private static void PromptForWords(out string word1, out string word2)
        {
            Console.WriteLine("Please enter the first word to compare:");
            Console.WriteLine("------");
            word1 = Console.ReadLine().Trim().ToLower();
            Console.WriteLine();
            Console.WriteLine($"Please enter the second word to compare (must be of length: {word1.Length})");
            Console.WriteLine("-----");
            Console.WriteLine();
            word2 = Console.ReadLine().Trim().ToLower();
            Console.WriteLine();
        }

        public static async Task RepeatProgram()
        {
            var goAgain = false;
            do
            {
                ConsoleKey response;
                do
                {
                    Console.WriteLine("Do you wish to compare another two words? (y/n)");
                    response = Console.ReadKey(false).Key;
                    if (response != ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                    }
                } while (response != ConsoleKey.Y && response != ConsoleKey.N);

                if (response == ConsoleKey.Y)
                    goAgain = true;

                if (response == ConsoleKey.N)
                    goAgain = false;

                if (goAgain == true)
                {
                    await PromptAndCompare();
                }
            } while (goAgain == true);
        }


        private static bool ValidateWords(string word1, string word2)
        {
            if (word1 == word2)
            {
                Console.WriteLine("Both words entered are the same");
                return false;
            }

            if (word1.Length != word2.Length)
            {
                Console.WriteLine("Words differ in length, please enter words of the same length.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(word1))
            {
                Console.WriteLine("Word 1 is empty");
                return false;
            }
            if (string.IsNullOrWhiteSpace(word2))
            {
                Console.WriteLine("Word 2 is empty");
                return false;
            }

            static bool isAllEnglishLetters(string word) => Regex.IsMatch(word, "^[a-zA-Z]*$");

            if (!isAllEnglishLetters(word1))
            {
                Console.WriteLine("Word 1 is not only English Letters");
                return false;
            }
            if (!isAllEnglishLetters(word2))
            {
                Console.WriteLine("Word 2 is not only English letters");
                return true;
            }

            return true;
        }


    }
}
