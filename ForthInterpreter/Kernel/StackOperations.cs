using ForthInterpreter.Interpret.Words;

namespace ForthInterpreter.Kernel
{
    public static class StackOperations
    {
        public static Word[] Primitives { get { return primitives; } }

        private static Word[] primitives = new Word[]
        {
            new Word("dup", env =>
            {
                env.DataStack.Push(env.DataStack.Peek());
            }),
            new Word("?dup", env =>
            {
                if (env.DataStack.Peek() != 0)
                    env.DataStack.Push(env.DataStack.Peek());
            }),
            new Word("drop", env =>
            {
                env.DataStack.Pop();
            }),
            new Word("swap", env =>
            {
                int n2 = env.DataStack.Pop(), n1 = env.DataStack.Pop();
                env.DataStack.Push(n2);
                env.DataStack.Push(n1);
            }),
            new Word("over", env =>
            {
                env.DataStack.Push(env.DataStack[1]);
            }),
            new Word("nip", env =>
            {
                int n2 = env.DataStack.Pop();
                env.DataStack.Pop();
                env.DataStack.Push(n2);
            }),
            new Word("tuck", env =>
            {
                env.DataStack.Push(env.DataStack[1]);
            }),
            new Word("rot", env =>
            {
                int n3 = env.DataStack.Pop(), n2 = env.DataStack.Pop(), 
                    n1 = env.DataStack.Pop();
                env.DataStack.Push(n2);
                env.DataStack.Push(n3);
                env.DataStack.Push(n1);
            }),
            new Word("-rot", env =>
            {
                int n3 = env.DataStack.Pop(), n2 = env.DataStack.Pop(), 
                    n1 = env.DataStack.Pop();
                env.DataStack.Push(n3);
                env.DataStack.Push(n1);
                env.DataStack.Push(n2);
            }),
            new Word("pick", env =>
            {
                int n = env.DataStack.Pop();
                env.DataStack.Push(env.DataStack[n]);
            }),
            new Word("2dup", env =>
            {
                env.DataStack.Push(env.DataStack[1]);
                env.DataStack.Push(env.DataStack[1]);
            }),
            new Word("2drop", env =>
            {
                env.DataStack.Pop();
                env.DataStack.Pop();
            }),
            new Word("2swap", env =>
            {
                int n4 = env.DataStack.Pop(), n3 = env.DataStack.Pop(), 
                    n2 = env.DataStack.Pop(), n1 = env.DataStack.Pop();
                env.DataStack.Push(n3);
                env.DataStack.Push(n4);
                env.DataStack.Push(n1);
                env.DataStack.Push(n2);
            }),
            new Word("2over", env =>
            {
                env.DataStack.Push(env.DataStack[3]);
                env.DataStack.Push(env.DataStack[3]);
            }),
            new Word("depth", env =>
            {
                env.DataStack.Push(env.DataStack.Count);
            }),
            
            
            // Return stack operations
            new Word(">r", env =>
            {
                env.ReturnStack.Push(env.DataStack.Pop());
            }),
            new Word("r@", env =>
            {
                env.DataStack.Push(env.ReturnStack.Peek());
            }),
            new Word("r>", env =>
            {
                env.DataStack.Push(env.ReturnStack.Pop());
            }),
            new Word("2>r", env =>
            {
                int n2 = env.DataStack.Pop(), n1 = env.DataStack.Pop();
                env.ReturnStack.Push(n1);
                env.ReturnStack.Push(n2);
            }),
            new Word("2r@", env =>
            {
                env.DataStack.Push(env.ReturnStack[1]);
                env.DataStack.Push(env.ReturnStack[0]);
            }),
            new Word("2r>", env =>
            {
                int n2 = env.ReturnStack.Pop(), n1 = env.ReturnStack.Pop();
                env.DataStack.Push(n1);
                env.DataStack.Push(n2);
            })
        };
    }
}
