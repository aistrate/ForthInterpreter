using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.LexicalScan
{
    public class SignedIntegerTokenReader : TokenReader
    {
        protected override string TokenRegexPattern
        {
            get { return @"\A \s* (-?\d{1,10}) (\s|$)"; }
        }

        public override Token ReadToken(TextBuffer textBuffer)
        {
            string firstMatchGroup = GetFirstMatchGroup(textBuffer);

            if (firstMatchGroup != null)
            {
                int value;
                if (int.TryParse(firstMatchGroup, out value))
                    return new SignedIntegerToken(value);
                else
                    textBuffer.UndoRead();
            }
            
            return null;
        }
    }
}
