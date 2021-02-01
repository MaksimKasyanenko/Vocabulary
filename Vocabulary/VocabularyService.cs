using System;
using System.Collections.Generic;
using System.Linq;
using Vocabulary.Abstract;

namespace Vocabulary
{
    public sealed class VocabularyService : IVocabularyService
    {
		private readonly IWordRepository _repository;

		public VocabularyService(IWordRepository repository)
        {
			if (repository == null) throw new ArgumentNullException("repository");
			this._repository = repository;
        }
        public int WordsCount { get { return _repository.GetWords().Count; } }
        public void AddWord(string word1, string word2)
        {
			if (word1 == "" || word2 == "") throw new ArgumentException("word1 or word2 is empty");
            Word newWord = new Word(word1, word2);
            foreach(var w in _repository.GetWords())
            {
                if (newWord.IsMatch(w)) {
                    w.Append(newWord);
                    _repository.SaveChanges();
                    return;
                }
            }
			_repository.GetWords().Add(new Word(word1, word2));
			_repository.SaveChanges();
        }
		public void DeleteWord(string word)
        {
			Word w = _repository.GetWords().Find(w=>w.IsFirstTranslate(word)||w.IsSecondTranslate(word));
            if (w != null)
            {
				_repository.GetWords().Remove(w);
				_repository.SaveChanges();
            }
        }

        public IEnumerable<Word> GetWords()
        {
			return from w in _repository.GetWords()
				   select w.Clone();
        }
		public void Clear()
        {
			_repository.GetWords().Clear();
			_repository.SaveChanges();
		}

        public void ResetRates()
        {
            _repository.GetWords().ForEach(w=>w.Rating.Rate = 0);
        }
    }
}
