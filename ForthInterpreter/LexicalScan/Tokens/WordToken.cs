
namespace ForthInterpreter.LexicalScan.Tokens
{
    public class WordToken : Token
    {
        public WordToken(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
