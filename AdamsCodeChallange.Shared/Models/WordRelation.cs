using System;
namespace AdamsCodeChallange.Shared.Models
{
    public class WordRelation
    {
        public WordRelation()
        {
        }
        public string InitialWord { get; set; }
        public int IndexToChange { get; set; }
        public char LetterToChangeTo { get; set; }
        public string OutputWord { get; set; }
    }
}
