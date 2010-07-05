using System;
using ForthInterpreter.Interpret;
using ForthInterpreter.Interpret.Words;
using ForthInterpreter.IO;

namespace ForthInterpreter.Kernel
{
    public static class DevEnvironment
    {
        public static Word[] Primitives { get { return primitives; } }

        private static Word[] primitives = new Word[]
        {
            new Word("words", env =>
            {
                Console.WriteLine("({0})", env.Words.Count);
                
                ConsoleStringBuilder csb = new ConsoleStringBuilder(78);
                foreach (string name in env.Words.Keys)
                    csb.AppendFormat("{0}  ", name);
                
                Console.WriteLine(csb);
            }),
            new Word(".s", env =>
            {
                env.DataStack.Print(20);
            }),
            new Word(".rs", env =>
            {
                Console.Write("Return stack: ");
                env.ReturnStack.Print(20);
            }),
            new Word(".free", env =>
            {
                Console.WriteLine(" Free memory = {0:#,##0} bytes [{1:#,##0} kb]", 
                                  env.Memory.FreeMemory, env.Memory.FreeMemory / 1024);
            }),
            new Word("dump", env =>
            {
                int count = env.DataStack.Pop(), fromAddress = env.DataStack.Pop();
                env.Memory.PrintDump(fromAddress, count);
            }),
            new Word("see", env =>
            {
                Word word = Validate.ReadExistingWord(env);
                Console.Write("\n{0}{1}", word.SeeRootDescription, word.IsImmediate ? " immediate" : "");
            }),
            new Word("bye", env =>
            {
                Console.WriteLine();
                System.Environment.Exit(0);
            })
        };
    }
}
