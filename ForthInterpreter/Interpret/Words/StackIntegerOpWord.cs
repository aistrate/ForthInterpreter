using System;

namespace ForthInterpreter.Interpret.Words
{
    public class StackIntegerOpWord : Word
    {
        public StackIntegerOpWord(string name, Func<int, int> unaryOp)
            : base(name, env =>
            {
                int n1 = env.DataStack.Pop();
                env.DataStack.Push(unaryOp(n1));
            })
        {
        }

        public StackIntegerOpWord(string name, Func<int, int, int> binaryOp)
            : base(name, env =>
            {
                int n2 = env.DataStack.Pop(), n1 = env.DataStack.Pop();
                env.DataStack.Push(binaryOp(n1, n2));
            })
        {
        }

        public StackIntegerOpWord(string name, Func<int, int, int, int> ternaryOp)
            : base(name, env =>
            {
                int n3 = env.DataStack.Pop(), n2 = env.DataStack.Pop(), n1 = env.DataStack.Pop();
                env.DataStack.Push(ternaryOp(n1, n2, n3));
            })
        {
        }
    }
}
