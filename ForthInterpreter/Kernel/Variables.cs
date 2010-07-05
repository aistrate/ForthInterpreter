using ForthInterpreter.Interpret;
using ForthInterpreter.Interpret.Words;

namespace ForthInterpreter.Kernel
{
    public static class Variables
    {
        public static Word[] Primitives { get { return primitives; } }

        private static Word[] primitives = new Word[]
        {
            new Word("variable",
                env =>
                {
                    string wordName = Validate.ReadMandatoryWordName(env);
                    int variableAddress = env.Memory.AllocateCell();

                    env.LastCompiledWord = new VariableWord(wordName, variableAddress);
                    env.Words.AddOrUpdate(env.LastCompiledWord);
                }),
            new Word("value",
                env =>
                {
                    string wordName = Validate.ReadMandatoryWordName(env);
                    int initialValue = env.DataStack.Pop();
                    int variableAddress = env.Memory.AllocateAndStoreCell(initialValue);

                    env.LastCompiledWord = new ConstantWord(wordName, variableAddress, env);
                    env.Words.AddOrUpdate(env.LastCompiledWord);
                }),
            new Word("to",
                env =>
                {
                    InstanceWord instanceWord = Validate.ReadExistingInstanceWord(env, "constant");
                    int variableAddress = instanceWord.AllocatedAddress;
                    int value = env.DataStack.Pop();
                    
                    env.Memory.StoreCell(value, variableAddress);
                }),
            new Word("addr",
                env =>
                {
                    InstanceWord instanceWord = Validate.ReadExistingInstanceWord(env, "constant");
                    int variableAddress = instanceWord.AllocatedAddress;
                    
                    env.DataStack.Push(variableAddress);
                }),
            new Word("create",
                env =>
                {
                    string wordName = Validate.ReadMandatoryWordName(env);
                    int variableAddress = env.Memory.FirstFreeAddress;

                    env.LastCompiledWord = new VariableWord(wordName, variableAddress);
                    env.Words.AddOrUpdate(env.LastCompiledWord);
                })
        };
    }
}
