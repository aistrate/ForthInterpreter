using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.LexicalScan
{
    class LineCommentTokenReader : TokenReader
    {
        protected override string TokenRegexPattern
        {
            get { return @"\A \s? ([^\n]*) $"; }
        }

        public override Token ReadToken(TextBuffer textBuffer)
        {
            return new LineCommentToken(GetFirstMatchGroup(textBuffer));
        }
    }
}
