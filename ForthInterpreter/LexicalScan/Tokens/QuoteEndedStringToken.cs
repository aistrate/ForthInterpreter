
namespace ForthInterpreter.LexicalScan.Tokens
{
    public class QuoteEndedStringToken : Token
    {
        public QuoteEndedStringToken(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }
    }
}
