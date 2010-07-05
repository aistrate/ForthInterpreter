
namespace ForthInterpreter.LexicalScan.Tokens
{
    public class ParanEndedStringToken : Token
    {
        public ParanEndedStringToken(string text, bool isEndingInParan)
        {
            Text = text;
            IsEndingInParan = isEndingInParan;
        }

        public string Text { get; private set; }
        public bool IsEndingInParan { get; private set; }
    }
}
