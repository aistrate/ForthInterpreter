
namespace ForthInterpreter.Interpret.Words.ControlFlow
{
    public class DoLoopWord : BasicLoopWord
    {
        public DoLoopWord(Environment env)
            : base("do", env)
        {
            PerformsInitialTest = false;
            ReadsIncrement = false;
        }

        public override string[] RecognizedExitWordNames { get { return new[] { "leave", "?leave" }; } }

        // False for DO, True for ?DO
        public bool PerformsInitialTest { get; set; }

        // False for LOOP, True for +LOOP
        public bool ReadsIncrement { get; set; }

        protected override bool BeforeAction(Environment env)
        {
            int startLimit = env.DataStack.Pop(), endLimit = env.DataStack.Pop();

            if (!PerformsInitialTest || startLimit != endLimit)
            {
                env.ReturnStack.Push(endLimit);
                env.ReturnStack.Push(startLimit);
                return true;
            }
            
            return false;
        }

        protected override bool AfterEachCycleAction(Environment env)
        {
            int increment = ReadsIncrement ? env.DataStack.Pop() : 1;
            int newIndex = env.ReturnStack.Pop() + increment, endLimit = env.ReturnStack.Peek();

            env.ReturnStack.Push(newIndex);

            return increment > 0 ? newIndex < endLimit : 
                   increment < 0 ? newIndex >= endLimit : 
                                   true;
        }

        protected override bool AfterAction(Environment env)
        {
            env.ReturnStack.Pop();
            env.ReturnStack.Pop();
            return true;
        }


        protected override string SeeNodeStartDelimiter { get { return PerformsInitialTest ? "?do" : "do"; } }
        protected override string SeeNodeEndDelimiter { get { return ReadsIncrement ? "+loop" : "loop"; } }

        protected override string SeeNodeFrontBodyDescription { get { return CycleWord.SeeNodeDescription; } }
    }
}
