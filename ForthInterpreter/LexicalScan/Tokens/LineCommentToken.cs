
namespace ForthInterpreter.LexicalScan.Tokens
{
    public class LineCommentToken : Token
    {
        public LineCommentToken(string comment)
        {
            Comment = comment;
        }

        public string Comment { get; private set; }
    }
}
