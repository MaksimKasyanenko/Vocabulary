using System;
using Vocabulary.Abstract;

namespace VocabularyConsole
{
    class ConsoleInterface : IUserInterface
    {
        public bool AskAlternative(string question, string[] options, out int choice)
        {
            choice=-1;
            Console.WriteLine(question);
            for(int i=1; i <= options.Length; i++)
            {
                Console.WriteLine($"{options[i-1]} ({i})");
            }
            Console.Write("Сделайте выбор >>");
            try
            {
                choice = int.Parse(Console.ReadLine());
                if (choice < 0 || choice >= options.Length) throw new FormatException();
            }
            catch (FormatException)
            {
                Console.WriteLine("Неверный ввод");
                return false;
            }
            return true;
        }

        public bool AskGeneric(string question)
        {
            Console.WriteLine(question + " (y - да)");
            return Console.ReadLine() == "y";
        }

        public string AskSpecial(string question)
        {
            Console.WriteLine(question);
            return Console.ReadLine();
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
        public string[] AskMultiply(string message, string[] questions)
        {
            Console.WriteLine(message);
            string[] res = new string[questions.Length];
            for(int i = 0; i < questions.Length; i++)
            {
                Console.WriteLine(questions[i]);
                res[i] = Console.ReadLine();
            }
            return res;
        }
    }
}
