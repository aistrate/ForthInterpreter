using System;
using ForthInterpreter.DataTypes;
using ForthInterpreter.Interpret.Words;
using ForthInterpreter.LexicalScan;
using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.Kernel
{
    public static class StringOperations
    {
        public static Word[] Primitives { get { return primitives; } }

        private static Word[] primitives = new Word[]
        {
            new Word(".\"", true, false,
            env =>
            {
                QuoteEndedStringToken quoteEndedStringToken = TokenReader.ReadQuoteEndedStringToken(env.TextBuffer);
                if (env.IsCompileMode)
                {
                    CharString charString = env.Memory.AllocateAndStoreCharString(quoteEndedStringToken.Text);
                    (new PrintStringLiteralWord(charString, env)).Compile(env);
                }
                else
                    Console.Write(quoteEndedStringToken.Text);
            }),
            new Word("s\"", true, false,
            env =>
            {
                QuoteEndedStringToken quoteEndedStringToken = TokenReader.ReadQuoteEndedStringToken(env.TextBuffer);
                CharString charString = env.Memory.AllocateAndStoreCharString(quoteEndedStringToken.Text);
                (new CharStringLiteralWord(charString, env)).Interpret(env);
            }),
            new Word("c\"", true, false,
            env =>
            {
                QuoteEndedStringToken quoteEndedStringToken = TokenReader.ReadQuoteEndedStringToken(env.TextBuffer);
                CountedString countedString = env.Memory.AllocateAndStoreCountedString(quoteEndedStringToken.Text);
                (new CountedStringLiteralWord(countedString, env)).Interpret(env);
            }),
            new Word("count", env =>
            {
                CharString charString = env.Memory.ToCharString(new CountedString(env.DataStack.Pop()));
                env.DataStack.Push(charString.CharAddress);
                env.DataStack.Push(charString.Length);
            }),
            new Word("type", env =>
            {
                int length = env.DataStack.Pop(), charAddress = env.DataStack.Pop();
                string text = env.Memory.FetchCharString(new CharString(charAddress, length));
                Console.Write(text);
            }),
            new Word("accept", env =>
            {
                int maxLength = env.DataStack.Pop(), bufferAddress = env.DataStack.Pop();
                string inputText = Console.ReadLine();
                env.Memory.StoreCharString(inputText, bufferAddress);
                env.DataStack.Push(inputText.Length);
            })
        };
    }
}
