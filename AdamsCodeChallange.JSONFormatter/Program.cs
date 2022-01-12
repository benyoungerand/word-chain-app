using System;
using AdamsCodeChallange.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace AdamsCodeChallange.JSONFormatter
{
    class Program
    {
        private static readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _fileName = "rawData.txt";
        static void Main(string[] args)
        {
            string[] rawWords = System.IO.File.ReadAllLines($"{_baseDirectory}/{_fileName}");
            var words = rawWords.Select(rawWord => new Word { Name = rawWord }).ToList();
            words.ForEach(currentWord =>
            {
                Console.WriteLine($"Current Word: {currentWord.Name}");
                var dummyList = new List<Word>
                {
                    currentWord
                };
                var wordsExceptSelf = words.Except(dummyList);
                var wordsOfSameLength = words.Where(x => x.Name.Length == currentWord.Name.Length).ToList();
                wordsOfSameLength.ForEach(word =>
                {
                    var numberOfDeviations = 0;
                    var index = 0;
                    int? firstDeviationAtIndex = null;
                    foreach (var character in word.Name)
                    {
                        if (currentWord.Name[index] != word.Name[index])
                        {
                            numberOfDeviations++;
                            firstDeviationAtIndex = index;
                        }
                        if (numberOfDeviations > 1)
                        {
                            break;
                        }
                        index++;                        
                    }
                    if (numberOfDeviations == 1 && firstDeviationAtIndex != null)
                    {
                        currentWord.WordRelations.Add(new WordRelation { IndexToChange = firstDeviationAtIndex.Value, LetterToChangeTo = word.Name[firstDeviationAtIndex.Value], InitialWord = currentWord.Name, OutputWord = word.Name });
                    }
                });

            });
            string json = JsonSerializer.Serialize(words);
            File.WriteAllText($"{_baseDirectory}/processedData.json", json);
            Console.WriteLine("Processing complete");

        }
    }
}
