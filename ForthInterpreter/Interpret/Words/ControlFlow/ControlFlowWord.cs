using System;

namespace ForthInterpreter.Interpret.Words.ControlFlow
{
    public class ControlFlowWord : InstanceWord
    {
        public ControlFlowWord(string definingWordName, Environment env)
            : this("", definingWordName, -1, null, env)
        {
        }

        public ControlFlowWord(string name, string definingWordName, int allocatedAddress, 
                               Action<Environment> primitiveExecuteAction, Environment env)
            : base(name, definingWordName, allocatedAddress, primitiveExecuteAction)
        {
            SeeNodeIndentLevel = env.ControlFlowStack.Count / 2;
        }

        
        protected int SeeNodeIndentLevel { get; private set; }

        protected virtual string SeeNodeStartDelimiter { get { return ""; } }
        protected virtual string SeeNodeMidDelimiter { get { return ""; } }
        protected virtual string SeeNodeEndDelimiter { get { return ""; } }

        protected virtual string SeeNodeFrontBodyDescription { get { return ""; } }
        protected virtual string SeeNodeRearBodyDescription { get { return ""; } }

        public override string SeeNodeDescription
        {
            get
            {
                if (SeeNodeStartDelimiter == "")
                    return ExecuteWords.Count > 0 ? "\t" + SeeEnumerateChildNodes : "";
                else
                    return seeNewLine(SeeNodeStartDelimiter, SeeNodeFrontBodyDescription, true) +
                           seeNewLine(SeeNodeMidDelimiter, SeeNodeRearBodyDescription, false) +
                           seeNewLine(SeeNodeEndDelimiter) +
                           seeNewLine();
            }
        }

        private string seeNewLine(string delimiter, string bodyDescription, bool mandatory)
        {
            return bodyDescription.Trim() != "" || mandatory ? seeNewLine(delimiter) + bodyDescription : "";
        }

        private string seeNewLine(string delimiter)
        {
            return delimiter != "" ? seeNewLine() + delimiter.ToUpper() : "";
        }

        private string seeNewLine()
        {
            return "\n" + new string('\t', SeeNodeIndentLevel);
        }
    }
}
