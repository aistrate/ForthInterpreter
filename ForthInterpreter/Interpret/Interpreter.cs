using ForthInterpreter.Interpret.Words;
using ForthInterpreter.IO;
using ForthInterpreter.Kernel;
using ForthInterpreter.LexicalScan;
using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.Interpret
{
    public class Interpreter
    {
        public Interpreter()
        {
            Environment = new Environment();

            Environment.Words.AddRange(StackOperations.Primitives);
            Environment.Words.AddRange(MathOperations.Primitives);
            Environment.Words.AddRange(MemoryOperations.Primitives);
            Environment.Words.AddRange(StringOperations.Primitives);
            Environment.Words.AddRange(IOOperations.Primitives);
            Environment.Words.AddRange(Variables.Primitives);
            Environment.Words.AddRange(Compiling.Primitives);
            Environment.Words.AddRange(ControlFlow.Primitives);
            Environment.Words.AddRange(Comments.Primitives);
            Environment.Words.AddRange(DevEnvironment.Primitives);
            
            Interpret(KernelSourceCode);
            Environment.LastCompiledWord = null;
        }

        public Environment Environment { get; private set; }

        public void InterpretLine(string line)
        {
            try
            {
                Environment.TextBuffer = new TextBuffer(line);
                
                while (!Environment.TextBuffer.EndOfBuffer)
                {
                    if (isInsideMultilineComment(Environment) || TokenReader.ReadEmptyLineToken(Environment.TextBuffer) != null)
                        continue;

                    WordToken wordToken = TokenReader.ReadWordToken(Environment.TextBuffer);

                    if (Environment.Words.ContainsKey(wordToken.Name))
                        Environment.Words[wordToken.Name].Interpret(Environment);
                    else
                    {
                        Environment.TextBuffer.UndoRead();

                        SignedIntegerToken signedIntegerToken = TokenReader.ReadSignedIntegerToken(Environment.TextBuffer);
                        if (signedIntegerToken != null)
                            (new LiteralWord(signedIntegerToken.Value)).Interpret(Environment);
                        else
                            throw new InvalidWordException(Environment.TextBuffer, false);
                    }
                }

                if (Environment.ActiveExitWordName == "abort")
                    throw new InvalidWordException(Environment.TextBuffer, "Aborted.");
                Environment.ActiveExitWordName = null;
            }
            catch
            {
                Environment.Reset();
                throw;
            }
        }

        private bool isInsideMultilineComment(Environment env)
        {
            if (env.IsMultilineCommentMode)
            {
                ParanEndedStringToken paranEndedStringToken = TokenReader.ReadParanEndedStringToken(env.TextBuffer);
                env.IsMultilineCommentMode = !paranEndedStringToken.IsEndingInParan;
                return true;
            }

            return false;
        }

        public void Interpret(string text)
        {
            foreach (string line in FileLoader.GetTextLines(text))
                InterpretLine(line);
        }

        public static string KernelSourceCode
        {
            get { return FileLoader.ReadResource("ForthInterpreter.Kernel.Kernel.fth"); }
        }
    }
}
