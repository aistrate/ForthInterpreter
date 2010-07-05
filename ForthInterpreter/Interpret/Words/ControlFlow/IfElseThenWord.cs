using ForthInterpreter.DataTypes;

namespace ForthInterpreter.Interpret.Words.ControlFlow
{
    public class IfElseThenWord : ControlFlowWord
    {
        public IfElseThenWord(Environment env)
            : base("if", env)
        {
            TrueBranchWord = new ControlFlowWord("branch", env);
            FalseBranchWord = new ControlFlowWord("branch", env);

            PrimitiveExecuteAction = executeBranch;
        }

        public ControlFlowWord TrueBranchWord { get; private set; }
        public ControlFlowWord FalseBranchWord { get; private set; }

        private void executeBranch(Environment env)
        {
            if (BoolType.IsTrue(env.DataStack.Pop()))
                TrueBranchWord.Execute(env);
            else
                FalseBranchWord.Execute(env);
        }


        protected override string SeeNodeStartDelimiter { get { return "if"; } }
        protected override string SeeNodeMidDelimiter { get { return "else"; } }
        protected override string SeeNodeEndDelimiter { get { return "then"; } }

        protected override string SeeNodeFrontBodyDescription { get { return TrueBranchWord.SeeNodeDescription; } }
        protected override string SeeNodeRearBodyDescription { get { return FalseBranchWord.SeeNodeDescription; } }
    }
}
