using System;
using ForthInterpreter.Interpret;
using ForthInterpreter.Interpret.Words;
using ForthInterpreter.LexicalScan;
using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.Kernel
{
    public static class Comments
    {
        public static Word[] Primitives { get { return primitives; } }

        private static Word[] primitives = new Word[]
        {
            new Word(@"\", true, false,
            env =>
            {
                TokenReader.ReadLineCommentToken(env.TextBuffer);
            }),
            new Word("(", true, false,
            env =>
            {
                ParanEndedStringToken paranEndedStringToken = TokenReader.ReadParanEndedStringToken(env.TextBuffer);
                env.IsMultilineCommentMode = !paranEndedStringToken.IsEndingInParan;
            }),
            new Word(")", true, false,
            env =>
            {
                throw new InvalidWordException(env.TextBuffer, "Closed parenthesis found outside comment.");
            }),
            new Word(".(", true, false,
            env =>
            {
                Console.Write(TokenReader.ReadParanEndedStringToken(env.TextBuffer).Text);
            })
        };
    }
}
