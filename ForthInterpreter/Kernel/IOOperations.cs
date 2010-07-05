using System;
using ForthInterpreter.DataTypes;
using ForthInterpreter.Interpret.Words;
using ForthInterpreter.LexicalScan;
using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.Kernel
{
    public static class IOOperations
    {
        public static Word[] Primitives { get { return primitives; } }

        private static Word[] primitives = new Word[]
        {
            new Word("key", env =>
            {
                char ch = Console.ReadKey(true).KeyChar;
                env.DataStack.Push(CharType.ToCell(ch));
            }),
            new Word("key?", env =>
            {
                bool avail = Console.KeyAvailable;
                env.DataStack.Push(BoolType.Flag(avail));
            }),
            new Word("emit", env =>
            {
                Console.Write(CharType.ToChar(env.DataStack.Pop()));
            }),
            new Word("char", env =>
            {
                WordToken wordToken = TokenReader.ReadWordToken(env.TextBuffer);
                char ch = (wordToken != null ? wordToken.Name.ToCharArray(0, 1)[0] : ' ');
                env.DataStack.Push(CharType.ToCell(ch));
            }),
            new Word("[char]", true, true,
            env =>
            {
                WordToken wordToken = TokenReader.ReadWordToken(env.TextBuffer);
                char ch = (wordToken != null ? wordToken.Name.ToCharArray(0, 1)[0] : ' ');
                (new LiteralWord(ch)).CompileOrExecute(env);
            }),
            new Word(".", env =>
            {
                Console.Write("{0} ", env.DataStack.Pop());
            }),
            new Word("cr", env =>
            {
                Console.WriteLine();
            }),
            new Word("spaces", env =>
            {
                int n1 = env.DataStack.Pop();
                if (n1 > 0)
                    Console.Write(new string(' ', n1));
            }),
            new Word("space", env =>
            {
                Console.Write(' ');
            }),
            new Word("bl", env =>
            {
                env.DataStack.Push(CharType.ToCell(' '));
            }),
            new Word("page", env =>
            {
                Console.Clear();
            })
        };
    }
}
