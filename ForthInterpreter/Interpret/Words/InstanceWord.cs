using System;

namespace ForthInterpreter.Interpret.Words
{
    public class InstanceWord : Word
    {
        public InstanceWord(string definingWordName)
            : this("", definingWordName, -1, null)
        {
        }

        public InstanceWord(string name, string definingWordName, int allocatedAddress, Action<Environment> primitiveExecuteAction)
            : base(name, primitiveExecuteAction)
        {
            DefiningWordName = definingWordName;
            AllocatedAddress = allocatedAddress;
        }

        public override string[] RecognizedExitWordNames { get { return new string[] { }; } }

        public string DefiningWordName { get; private set; }
        public int AllocatedAddress { get; protected set; }
    }
}
