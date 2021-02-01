using System;
using System.Collections.Generic;
using Vocabulary.Abstract;
using Vocabulary;
using VocabularyRepository;
using System.IO;

namespace VocabularyConsole
{
    class Program
    {
		public static void Main()
		{
			string path = Path.Combine(Directory.GetCurrentDirectory(), "default.vcb");
			IWordRepository repo = new FileRepository(path);
			IUserInterface ui = new ConsoleInterface();
			Trainer[] tr = new Trainer[] { 
			  new TranslateAll(ui,repo),
			  new TranslateWithRating(ui,repo)
			};
			new Dialog(ui, new VocabularyService(repo), tr).Start();
		}
	}
}
