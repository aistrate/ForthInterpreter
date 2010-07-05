using System;
using ForthInterpreter.Interpret;

namespace ForthInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Interpreter interpreter = new Interpreter();

                Console.WriteLine("Forth .NET Interpreter, Copyright (C) 2008 Adrian Istrate");
                Console.WriteLine("Type 'bye' to exit\n");

                while (true)
                {
                    int oldCursorTop = Console.CursorTop;

                    string line = Console.ReadLine();

                    Console.CursorTop = oldCursorTop;
                    Console.CursorLeft = 0;
                    Console.Write(line + " ");

                    try
                    {
                        interpreter.InterpretLine(line);

                        interpreter.Environment.IsMultilineCommentMode = false;
                        
                        if (interpreter.Environment.IsCompileMode)
                            Console.WriteLine(" compiled");
                        else
                            Console.WriteLine(" ok{0}", (interpreter.Environment.DataStack.Count > 0 ?
                                                            "-" + interpreter.Environment.DataStack.Count.ToString() : ""));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\nERROR: {0}\n", ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("INIT ERROR: {0}", ex.Message);
            }
        }
    }
}
