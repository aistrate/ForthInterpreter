using ForthInterpreter.DataTypes;

namespace ForthInterpreter.Interpret.Words.ControlFlow
{
    public class BeginWord : BasicLoopWord
    {
        public BeginWord(Environment env)
            : base("begin", env)
        {
        }
        
        protected override bool AfterEachCycleAction(Environment env)
        {
            return TestsUntil ? BoolType.IsFalse(env.DataStack.Pop()) : true;
        }

        public bool TestsUntil { get; set; }


        protected override string SeeNodeStartDelimiter { get { return "begin"; } }
        protected override string SeeNodeEndDelimiter { get { return TestsUntil ? "until" : "again"; } }

        protected override string SeeNodeFrontBodyDescription { get { return CycleWord.SeeNodeDescription; } }
    }
}
