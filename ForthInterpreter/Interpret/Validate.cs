using System;
using ForthInterpreter.Interpret.Words;
using ForthInterpreter.LexicalScan;

namespace ForthInterpreter.Interpret
{
    public static class Validate
    {
        public static string ReadMandatoryWordName(Environment env)
        {
            if (env.TextBuffer.EndOfBuffer)
                throw new ApplicationException("Attempt to use zero-length string as a name.");

            return TokenReader.ReadWordToken(env.TextBuffer).Name;
        }

        public static Word ReadExistingWord(Environment env)
        {
            string wordName = ReadMandatoryWordName(env);

            if (!env.Words.ContainsKey(wordName))
                throw new InvalidWordException(env.TextBuffer);

            return env.Words[wordName];
        }

        public static InstanceWord ReadExistingInstanceWord(Environment env, string definingWordName)
        {
            InstanceWord instanceWord = ReadExistingWord(env) as InstanceWord;
            
            if (instanceWord == null || instanceWord.DefiningWordName != definingWordName)
                throw new InvalidWordException(env.TextBuffer, "Invalid name argument.");

            return instanceWord;
        }

        //public static Word TryPopControlFlowStack(Environment env, bool countIsOne, string definingWordName, Type type)
        //{
        //    if (env.ControlFlowStack.TopWordIsValid(countIsOne, definingWordName, type))
        //        return env.ControlFlowStack.Pop();
        //    else
        //        return null;
        //}

        public static Word PopControlFlowStack(Environment env, bool countIsOne)
        {
            return PopControlFlowStack(env, countIsOne, null, null);
        }

        public static Word PopControlFlowStack(Environment env, bool countIsOne, string definingWordName, Type type)
        {
            checkControlFlowStack(env, countIsOne, definingWordName, type);
            return env.ControlFlowStack.Pop();
        }

        public static Word PeekControlFlowStack(Environment env, bool countIsOne, string definingWordName, Type type)
        {
            checkControlFlowStack(env, countIsOne, definingWordName, type);
            return env.ControlFlowStack.Peek();
        }

        private static void checkControlFlowStack(Environment env, bool countIsOne, string definingWordName, Type type)
        {
            if (!env.ControlFlowStack.TopWordIsValid(countIsOne, definingWordName, type))
                throw new InvalidWordException(env.TextBuffer, "Unstructured.");
        }
    }
}
