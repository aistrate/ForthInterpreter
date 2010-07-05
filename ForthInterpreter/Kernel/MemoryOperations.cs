using ForthInterpreter.Interpret.Words;

namespace ForthInterpreter.Kernel
{
    public static class MemoryOperations
    {
        public static Word[] Primitives { get { return primitives; } }

        private static Word[] primitives = new Word[]
        {
            new Word("@", env =>
            {
                int fromAddress = env.DataStack.Pop();
                env.DataStack.Push(env.Memory.FetchCell(fromAddress));
            }),
            new Word("!", env =>
            {
                int toAddress = env.DataStack.Pop(), value = env.DataStack.Pop();
                env.Memory.StoreCell(value, toAddress);
            }),
            new Word("+!", env =>
            {
                int address = env.DataStack.Pop(), valueToAdd = env.DataStack.Pop();
                int value = env.Memory.FetchCell(address);
                env.Memory.StoreCell(value + valueToAdd, address);
            }),
            new Word("c@", env =>
            {
                int fromAddress = env.DataStack.Pop();
                env.DataStack.Push((int)env.Memory.FetchByte(fromAddress));
            }),
            new Word("c!", env =>
            {
                int toAddress = env.DataStack.Pop(), value = env.DataStack.Pop();
                env.Memory.StoreByte((byte)value, toAddress);
            }),
            new Word("here", env =>
            {
                env.DataStack.Push(env.Memory.FirstFreeAddress);
            }),
            new Word("allot", env =>
            {
                int n = env.DataStack.Pop();
                env.Memory.AllocateBytes(n);
            }),
            new Word("fill", env =>
            {
                int value = env.DataStack.Pop(), count = env.DataStack.Pop(), toAddress = env.DataStack.Pop();
                env.Memory.FillBytes(toAddress, count, (byte)value);
            }),
            new Word("cell", env =>
            {
                env.DataStack.Push(sizeof(int));
            }),
            new Word("cells", env =>
            {
                int n = env.DataStack.Pop();
                env.DataStack.Push(n * sizeof(int));
            }),
            new Word("chars", env =>
            {
                int n = env.DataStack.Pop();
                env.DataStack.Push(n);
            })
        };
    }
}
