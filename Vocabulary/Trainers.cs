using System;
using System.Collections.Generic;
using System.Linq;
using Vocabulary.Abstract;

namespace Vocabulary
{
    public abstract class Trainer
    {
        protected IUserInterface _ui;
        protected readonly IWordRepository _repository;
        protected Random random = new Random();
        public abstract string Description { get; }
        protected Trainer(IUserInterface ui, IWordRepository _repository)
        {
            if (ui == null) throw new ArgumentNullException("ui");
            this._ui = ui;
            if(_repository == null) throw new ArgumentNullException("repository");
            this._repository = _repository;
        }
        public abstract void Play(IVocabularyService voc);
        public virtual bool IOAndVerifyAnswer(Word word, bool expectedSecond)
        {
            string answer = _ui.AskSpecial(expectedSecond ? word.FirstTranslate : word.SecondTranslate);
            if (expectedSecond ? word.IsSecondTranslate(answer) : word.IsFirstTranslate(answer))
            {
                _ui.ShowMessage(" Правильно!");
                return true;
            }
            else
            {
                _ui.ShowMessage($" Ошибка! Правильный ответ: {(expectedSecond ? word.SecondTranslate : word.FirstTranslate)}");
                return false;
            }
        }
    }
    public class TranslateAll : Trainer
    {
        public override string Description { get; } = "Перевод всех слов";
        private int errors = 0;
        private int correct = 0;
        public TranslateAll(IUserInterface ui, IWordRepository repository) : base(ui,repository) { }
        public override void Play(IVocabularyService voc)
        {
            while (true)
            {
                _ui.ShowMessage("Переводите слово: ");
                List<Word> words = voc.GetWords().ToList();
                if(words.Count < 1)
                {
                    _ui.ShowMessage("Нет слов для заучивания");
                    break;
                }
                Word word = words[random.Next(0, words.Count)];
                int firOrSec = random.Next(0, 2);
                if (IOAndVerifyAnswer(word, firOrSec > 0)) correct++;
                else errors++;
                _ui.ShowMessage($"Правильных ответов: {correct}, ошибок: {errors}");
                if (_ui.AskGeneric("Закончить?")) break;
            }
        }
    }
    public class TranslateWithRating : Trainer
    {
        public override string Description => "Перевод с рейтингом";

        public TranslateWithRating(IUserInterface ui,IWordRepository repository ) : base(ui, repository) { }

        public override void Play(IVocabularyService voc)
        {
            var queryWords = (from w in _repository.GetWords()
                        where (w.Rating.Rate / w.Rating.MaxRate * 100) < 90
                         select w);
            while (true)
            {
                List<Word> words = queryWords.ToList();
                _ui.ShowMessage("Переводите слово: ");
                if (words.Count < 1)
                {
                    _ui.ShowMessage("Нет слов для заучивания");
                    break;
                }
                Word word = words[random.Next(0, words.Count)];
                if (IOAndVerifyAnswer(word, random.Next(0, 2) > 0))
                {
                    word.Rating.Rate += 20;
                }
                else {
                    word.Rating.Rate -= 30;
                }
                _repository.SaveChanges();
                _ui.ShowMessage($"Рейтинг слова {word}: {word.Rating.Rate}");
                if (_ui.AskGeneric("Закончить?")) break;
            }
        }
    }
}
