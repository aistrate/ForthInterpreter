using ForthInterpreter.DataTypes;

namespace ForthInterpreter.Interpret.Words
{
    public class LiteralWord : InstanceWord
    {
        public LiteralWord(char value)
            : this(CharType.ToCell(value))
        {
        }
        
        public LiteralWord(int value)
            : base("", "literal", -1, env => env.DataStack.Push(value))
        {
            Value = value;
        }

        public int Value { get; private set; }

        public override string SeeNodeDescription { get { return Value.ToString(); } }
    }
}
