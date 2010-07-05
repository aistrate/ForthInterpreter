using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForthInterpreter.Interpret.Words.ControlFlow
{
    public class OfWord : ControlFlowWord
    {
        public OfWord(Environment env)
            : base("of", env)
        {
            OfBodyWord = new ControlFlowWord("of-body", env);

            PrimitiveExecuteAction = executeOf;
        }

        public ControlFlowWord OfBodyWord { get; private set; }

        private void executeOf(Environment env)
        {
            int ofParam = env.DataStack.Pop(), caseParam = env.DataStack.Peek();

            if (ofParam == caseParam)
            {
                env.DataStack.Pop();
                OfBodyWord.Execute(env);

                if (env.IsExitMode) return;

                env.ActiveExitWordName = "leave-case";
            }
        }


        protected override string SeeNodeStartDelimiter { get { return "of"; } }
        protected override string SeeNodeEndDelimiter { get { return "endof"; } }

        protected override string SeeNodeFrontBodyDescription { get { return OfBodyWord.SeeNodeDescription; } }
    }
}
