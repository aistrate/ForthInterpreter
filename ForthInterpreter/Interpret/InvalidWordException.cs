using System;
using System.Text.RegularExpressions;
using ForthInterpreter.LexicalScan;
using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.Interpret
{
    public class InvalidWordException : ApplicationException
    {
        public InvalidWordException(TextBuffer textBuffer)
            : this(textBuffer, true)
        {
        }

        public InvalidWordException(TextBuffer textBuffer, bool undoLastRead)
            : this(textBuffer, "Undefined word.", undoLastRead)
        {
        }

        public InvalidWordException(TextBuffer textBuffer, string errorDescription)
            : this(textBuffer, errorDescription, true)
        {
        }

        public InvalidWordException(TextBuffer textBuffer, string errorDescription, bool undoLastRead)
            : base(buildMessage(textBuffer, errorDescription, undoLastRead))
        {
        }

        private static string buildMessage(TextBuffer textBuffer, string errorDescription, bool undoLastRead)
        {
            if (undoLastRead)
                textBuffer.UndoRead();

            WordToken wordToken = TokenReader.ReadWordToken(textBuffer);

            int arrowsIndex = textBuffer.PreviousIndex + textBuffer.LastRead.Length - wordToken.Name.Length;
            string arrowsLine = Regex.Replace(textBuffer.Text.Substring(0, arrowsIndex), @"\S", " ", RegexOptions.Singleline);

            return string.Format("{0}\n{1}\n{2}", errorDescription, textBuffer.Text, arrowsLine + new string('^', wordToken.Name.Length));
        }
    }
}
