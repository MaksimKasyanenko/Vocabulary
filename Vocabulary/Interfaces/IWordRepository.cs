using System;
using System.Collections.Generic;
using System.Text;

namespace Vocabulary.Abstract
{
    public interface IWordRepository
    {
        List<Word> GetWords();
        void SaveChanges();

    }
}
