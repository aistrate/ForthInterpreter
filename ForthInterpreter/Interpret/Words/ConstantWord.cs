using System;

namespace ForthInterpreter.Interpret.Words
{
    public class ConstantWord : InstanceWord
    {
        public ConstantWord(string name, int allocatedAddress, Environment env)
            : base(name, "constant", allocatedAddress,
                   e => e.DataStack.Push(e.Memory.FetchCell(allocatedAddress)))
        {
            GetValue = () =>
                {
                    Execute(env);
                    return env.DataStack.Pop();
                };
        }

        public Func<int> GetValue { get; private set; }

        public override string SeeRootDescription { get { return string.Format("{0} Constant {1}", GetValue(), Name); } }
    }
}
