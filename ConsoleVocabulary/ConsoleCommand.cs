using System;

namespace VocabularyConsole
{
	sealed class ConsoleCommand
	{
		public string Text { get; }
		public string Description { get; }
		private readonly Action action;
		public ConsoleCommand(string text, string description, Action action)
		{
			Text = text;
			Description = description;
			this.action = action;
		}
		public void Execute()
		{
			this.action.Invoke();
		}
		public override string ToString()
		{
			return $"{Text} - {Description}";
		}
	}
}
