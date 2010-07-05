using ForthInterpreter.Interpret.Words;
using ForthInterpreter.LexicalScan;

namespace ForthInterpreter.Interpret
{
    public class Environment
    {
        public Environment()
        {
            Words = new WordDictionary();
            Memory = new Memory(100000);
            
            DataStack = new DataStack();
            ReturnStack = new DataStack("Return Stack");
            ControlFlowStack = new ControlFlowStack();
        }

        public TextBuffer TextBuffer { get; set; }

        public WordDictionary Words { get; private set; }
        public Memory Memory { get; private set; }

        public DataStack DataStack { get; private set; }
        public DataStack ReturnStack { get; private set; }
        public ControlFlowStack ControlFlowStack { get; private set; }

        public string ActiveExitWordName { get; set; }
        public bool IsExitMode { get { return ActiveExitWordName != null; } }

        public bool IsCompileMode { get; set; }
        public Word CompilingWord { get; set; }
        public Word LastCompiledWord { get; set; }
        
        public bool IsMultilineCommentMode { get; set; }

        public void Reset()
        {
            // NOT reset: TextBuffer, Words, Memory and LastCompiledWord
            DataStack.Clear();
            ReturnStack.Clear();
            ControlFlowStack.Clear();

            ActiveExitWordName = null;
            IsCompileMode = false;
            CompilingWord = null;
            IsMultilineCommentMode = false;
        }
    }
}
