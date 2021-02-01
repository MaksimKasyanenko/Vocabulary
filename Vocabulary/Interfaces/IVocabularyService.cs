using System;
using System.Collections.Generic;
using System.Text;

namespace Vocabulary.Abstract
{
    public interface IVocabularyService
    {
        int WordsCount { get; }
        void AddWord(string word1, string word2);
        void DeleteWord(string word);
        IEnumerable<Word> GetWords();
        void Clear();
    }
}
