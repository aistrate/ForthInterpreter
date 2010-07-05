using System;
using ForthInterpreter.DataTypes;

namespace ForthInterpreter.Interpret.Words
{
    public class CharStringLiteralWord : StringLiteralWord
    {
        public CharStringLiteralWord(CharString charString, Environment env)
            : base("s\"", charString.CharAddress, 
                   e => { e.DataStack.Push(charString.CharAddress); e.DataStack.Push(charString.Length); })
        {
            Value = env.Memory.FetchCharString(charString);
        }
    }


    public class CountedStringLiteralWord : StringLiteralWord
    {
        public CountedStringLiteralWord(CountedString countedString, Environment env)
            : base("c\"", countedString.CounterAddress, 
                   e => e.DataStack.Push(countedString.CounterAddress))
        {
            Value = env.Memory.FetchCountedString(countedString);
        }
    }


    public class PrintStringLiteralWord : StringLiteralWord
    {
        public PrintStringLiteralWord(CharString charString, Environment env)
            : base(".\"", charString.CharAddress, 
                   e => Console.Write(e.Memory.FetchCharString(charString)))
        {
            Value = env.Memory.FetchCharString(charString);
        }
    }


    public abstract class StringLiteralWord : InstanceWord
    {
        public StringLiteralWord(string definingWordName, int allocatedAddress, Action<Environment> primitiveExecuteAction)
            : base("", definingWordName, allocatedAddress, primitiveExecuteAction)
        {
        }

        public string Value { get; protected set; }

        public override string SeeNodeDescription { get { return string.Format("{0} {1}\"", DefiningWordName, Value); } }
    }
}
