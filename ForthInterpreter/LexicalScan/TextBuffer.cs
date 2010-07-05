using System;

namespace ForthInterpreter.LexicalScan
{
    public class TextBuffer
    {
        public TextBuffer(string text)
        {
            Text = text;
            Index = 0;
            PreviousIndex = 0;
            LastRead = "";
        }

        public string Text { get; private set; }
        public int Index { get; private set; }
        public int PreviousIndex { get; private set; }
        public string LastRead { get; private set; }

        public bool EndOfBuffer { get { return Index > Text.Length - 1; } }

        public string Remainder { get { return (!EndOfBuffer ? Text.Substring(Index) : ""); } }
        
        public void MoveForward(int offset)
        {
            if (offset > 0)
            {
                PreviousIndex = Index;
                Index = Math.Min(Index + offset, Text.Length);
                LastRead = Text.Substring(PreviousIndex, Index - PreviousIndex);
            }
        }

        public void UndoRead()
        {
            Index = PreviousIndex;
        }
    }
}
