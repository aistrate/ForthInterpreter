using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.LexicalScan
{
    public class EmptyLineTokenReader : TokenReader
    {
        protected override string TokenRegexPattern
        {
            get { return @"\A (\s*) $"; }
        }

        public override Token ReadToken(TextBuffer textBuffer)
        {
            string firstMatchGroup = GetFirstMatchGroup(textBuffer);
            return (firstMatchGroup != null ? new EmptyLineToken() : null);
        }
    }
}
