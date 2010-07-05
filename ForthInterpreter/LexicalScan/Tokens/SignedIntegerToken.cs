
namespace ForthInterpreter.LexicalScan.Tokens
{
    public class SignedIntegerToken : Token
    {
        public SignedIntegerToken(int value)
        {
            Value = value;
        }

        public int Value { get; private set; }
    }
}
