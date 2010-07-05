
namespace ForthInterpreter.Interpret.Words
{
    public class DefinedChildWord : InstanceWord
    {
        public DefinedChildWord(string name, string definingWordName, int allocatedAddress)
            : base(name, definingWordName, allocatedAddress, null)
        {
        }


        public override string SeeRootDescription
        {
            get { return string.Format("create {0}   \\ {1}\nDOES>{2};", Name, DefiningWordName, CleanFormat(SeeEnumerateChildNodes)); }
        }
    }
}
