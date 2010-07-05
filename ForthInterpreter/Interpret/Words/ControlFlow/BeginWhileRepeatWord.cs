using ForthInterpreter.DataTypes;

namespace ForthInterpreter.Interpret.Words.ControlFlow
{
    public class BeginWhileRepeatWord : BasicLoopWord
    {
        public BeginWhileRepeatWord(Environment env)
            : base("begin-while", env)
        {
            WhileTestWord = new ControlFlowWord("while", env);
        }

        public ControlFlowWord WhileTestWord { get; private set; }

        protected override bool BeforeEachCycleAction(Environment env)
        {
            WhileTestWord.Execute(env);

            if (env.IsExitMode) return false;

            return BoolType.IsTrue(env.DataStack.Pop());
        }


        protected override string SeeNodeStartDelimiter { get { return "begin"; } }
        protected override string SeeNodeMidDelimiter { get { return "while"; } }
        protected override string SeeNodeEndDelimiter { get { return "repeat"; } }

        protected override string SeeNodeFrontBodyDescription { get { return WhileTestWord.SeeNodeDescription; } }
        protected override string SeeNodeRearBodyDescription { get { return CycleWord.SeeNodeDescription; } }
    }
}
