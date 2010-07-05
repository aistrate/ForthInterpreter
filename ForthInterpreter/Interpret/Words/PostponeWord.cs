
namespace ForthInterpreter.Interpret.Words
{
    public class PostponeWord : InstanceWord
    {
        public PostponeWord(Word postponedWord)
            : base("postpone")
        {
            PostponedWordName = postponedWord.Name;

            if (postponedWord.IsImmediate)
                ExecuteWords.Add(postponedWord);
            else
                ExecuteWords.Add(new Word("", postponedWord.CompileOrExecute));
        }

        public string PostponedWordName { get; private set; }

        public override string SeeNodeDescription { get { return string.Format("postpone {0}", PostponedWordName); } }
    }
}
