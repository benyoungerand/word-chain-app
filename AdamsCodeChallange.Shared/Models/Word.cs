using System;
using System.Collections.Generic;

namespace AdamsCodeChallange.Shared.Models
{
    public class Word
    {
        public Word()
        {
        }


        public string Name { get; set; }
        public List<WordRelation> WordRelations { get; set; } = new();
    }
}
