namespace Vocabulary.Abstract
{
   public interface IUserInterface
{
        void ShowMessage(string message);
        bool AskGeneric(string question);
        string[] AskMultiply(string message, string[] questions);
        bool AskAlternative(string question, string[] options, out int choice);
        string AskSpecial(string question);
}
}
