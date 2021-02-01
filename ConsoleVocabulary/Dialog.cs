using System;
using System.Collections.Generic;
using Vocabulary.Abstract;
using Vocabulary;

namespace VocabularyConsole
{
    class Dialog
    {
		ConsoleCommand _currentCommand;
		List<ConsoleCommand> _commands;
		IVocabularyService _voc;
		Trainer[] _trainers;
		IUserInterface _ui;
		public Dialog(IUserInterface ui, IVocabularyService voc, Trainer[] trainers)
        {
			if (voc == null) throw new ArgumentNullException("vocabulary service");
			_voc = voc;
			if (trainers == null) throw new ArgumentNullException("trainers");
			_trainers = trainers;
			if(ui==null) throw new ArgumentNullException("ui");
			_ui = ui;
		}
		public void Start()
        {
			_commands = RegisterCommands();
			_ui.ShowMessage("Введите команду(*h - помощь)");
			while (true)
			{
				EnterCommand();
			}
		}
		private void AddWord()
        {
			string[] input = _ui.AskMultiply("Введите слово и перевод",
				new string[] {
				$"Слово: ",
				$"Перевод: "
			});
			if (input == null || input.Length < 2) throw new Exception("AddWord input");
			_voc.AddWord(input[0], input[1]);
        }
		private void DeleteWord()
        {
			string word = _ui.AskSpecial("Введите слово для удаления");
			if (!_ui.AskGeneric($"Вы действительно хотите удалить слово {word}?")) return;
			_voc.DeleteWord(word);
			_ui.ShowMessage("Слово удалено из словаря");
		}
		private void ClearVocabulary()
        {
			if (_ui.AskGeneric("Вы уверены? Все слова из словаря будут удалены"))
			{
				_voc.Clear();
			}
		}
		public void ShowInfo()
		{
			_ui.ShowMessage($"Слов в словаре: {_voc.WordsCount}");
		}
		public void ShowAllWords()
		{
			int i = 0;
			foreach(Word w in _voc.GetWords())
            {
				i++;
				_ui.ShowMessage(w.ToString());
				if (i > 15 && !_ui.AskGeneric("Показать ещё 15 ?")) return;
				else i = 0;
            }
			_ui.ShowMessage("Слов больше нет");
		}
		private void ResetRates()
        {
			if (!_ui.AskGeneric($"Вы действительно хотите обнулить рейтинг слов?")) return;
			_voc.ResetRates();
			_ui.ShowMessage("Рейтинг слов обнулен");
		}
		private void EnterCommand()
		{
			Console.Write(">> ");
			string com = Console.ReadLine();
			_currentCommand = null;
			foreach (var c in _commands)
			{
				if (c.Text == com)
				{
					_currentCommand = c;
					break;
				}
			}
			if (_currentCommand != null)
			{
				_currentCommand.Execute();
			}
			else Console.WriteLine("Неизвестная команда");
		}
		private List<ConsoleCommand> RegisterCommands()
		{
			var res =  new List<ConsoleCommand>{
				new ConsoleCommand("*h", "Помощь", ()=>_commands.ForEach(x => Console.WriteLine(x.ToString()))),
				new ConsoleCommand("*i","Отобразить информацию о словаре",()=>ShowInfo()),
				new ConsoleCommand("*cc", "Очистить консоль", ()=>Console.Clear()),
				new ConsoleCommand("*cv","Очистить словарь", ()=>ClearVocabulary()),
				new ConsoleCommand("*sw","Показать слова",()=>ShowAllWords()),
				new ConsoleCommand("*aw","Добавить слово",()=>AddWord()),
				new ConsoleCommand("*dw","Удалить слово",()=>DeleteWord()),
				new ConsoleCommand("*rr","Обнулить рейтинг слов", ()=>ResetRates())
			};
			for(int i = 0; i < _trainers.Length; i++)
            {
				int num = i;
                if (_trainers[i] != null)
                {
					res.Add(new ConsoleCommand("*tr"+i, _trainers[i].Description, ()=> _trainers[num].Play(_voc)));
                }
            }
			return res;
		}
	}
}
