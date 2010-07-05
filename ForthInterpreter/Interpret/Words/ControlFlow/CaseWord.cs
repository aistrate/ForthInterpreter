
namespace ForthInterpreter.Interpret.Words.ControlFlow
{
    public class CaseWord : ControlFlowWord
    {
        public CaseWord(Environment env)
            : base("case", env)
        {
            CaseBodyWord = new ControlFlowWord("case-body", env);

            PrimitiveExecuteAction = executeCase;
        }

        public override string[] RecognizedExitWordNames { get { return new[] { "leave-case" }; } }

        public ControlFlowWord CaseBodyWord { get; private set; }

        private void executeCase(Environment env)
        {
            CaseBodyWord.Execute(env);

            if (env.IsExitMode) return;

            env.DataStack.Pop();
        }


        protected override string SeeNodeStartDelimiter { get { return "case"; } }
        protected override string SeeNodeEndDelimiter { get { return "endcase"; } }

        protected override string SeeNodeFrontBodyDescription { get { return CaseBodyWord.SeeNodeDescription; } }
    }
}
