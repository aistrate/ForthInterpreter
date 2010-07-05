using System;
using ForthInterpreter.DataTypes;

namespace ForthInterpreter.Interpret.Words
{
    public class ExitWord : Word
    {
        public ExitWord(string name)
            : this(name, false)
        {
        }

        public ExitWord(string name, bool readsFlag)
            : this(name, readsFlag, null)
        {
        }

        public ExitWord(string name, Action<Environment> beforeAction)
            : this(name, false, beforeAction)
        {
        }

        public ExitWord(string name, bool readsFlag, Action<Environment> beforeAction)
            : base(name, false, true, null)
        {
            ReadsFlag = readsFlag;
            BeforeAction = beforeAction;

            PrimitiveExecuteAction = doExit;
        }

        public override string[] RecognizedExitWordNames { get { return new string[] { }; } }

        public bool ReadsFlag { get; private set; }

        public Action<Environment> BeforeAction { get; private set; }

        private void doExit(Environment env)
        {
            if (!ReadsFlag || BoolType.IsTrue(env.DataStack.Pop()))
            {
                if (BeforeAction != null)
                    BeforeAction(env);
                
                env.ActiveExitWordName = Name;
            }
        }
    }
}
