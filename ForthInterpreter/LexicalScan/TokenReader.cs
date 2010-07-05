using System.Text.RegularExpressions;
using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.LexicalScan
{
    public abstract class TokenReader
    {
        public TokenReader()
        {
            TokenRegex = new Regex(TokenRegexPattern,
                                   RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline |
                                   RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        }

        protected Regex TokenRegex { get; private set; }

        protected abstract string TokenRegexPattern { get; }

        protected Match GetMatch(TextBuffer textBuffer)
        {
            Match match = TokenRegex.Match(textBuffer.Remainder);

            if (match.Success)
            {
                textBuffer.MoveForward(match.Value.Length);
                return match;
            }
            else
                return null;
        }

        protected string GetFirstMatchGroup(TextBuffer textBuffer)
        {
            Match match = GetMatch(textBuffer);
            return (match != null ? match.Groups[1].Value : null);
        }

        public abstract Token ReadToken(TextBuffer textBuffer);

        
        public static WordToken ReadWordToken(TextBuffer textBuffer)
        {
            return (WordToken)wordTokenReader.ReadToken(textBuffer);
        }

        public static SignedIntegerToken ReadSignedIntegerToken(TextBuffer textBuffer)
        {
            return (SignedIntegerToken)signedIntegerTokenReader.ReadToken(textBuffer);
        }

        public static QuoteEndedStringToken ReadQuoteEndedStringToken(TextBuffer textBuffer)
        {
            return (QuoteEndedStringToken)quoteEndedStringTokenReader.ReadToken(textBuffer);
        }

        public static ParanEndedStringToken ReadParanEndedStringToken(TextBuffer textBuffer)
        {
            return (ParanEndedStringToken)paranEndedStringTokenReader.ReadToken(textBuffer);
        }

        public static LineCommentToken ReadLineCommentToken(TextBuffer textBuffer)
        {
            return (LineCommentToken)lineCommentTokenReader.ReadToken(textBuffer);
        }

        public static EmptyLineToken ReadEmptyLineToken(TextBuffer textBuffer)
        {
            return (EmptyLineToken)emptyLineTokenReader.ReadToken(textBuffer);
        }

        private static WordTokenReader wordTokenReader = new WordTokenReader();
        private static SignedIntegerTokenReader signedIntegerTokenReader = new SignedIntegerTokenReader();
        private static QuoteEndedStringTokenReader quoteEndedStringTokenReader = new QuoteEndedStringTokenReader();
        private static ParanEndedStringTokenReader paranEndedStringTokenReader = new ParanEndedStringTokenReader();
        private static LineCommentTokenReader lineCommentTokenReader = new LineCommentTokenReader();
        private static EmptyLineTokenReader emptyLineTokenReader = new EmptyLineTokenReader();
    }
}
