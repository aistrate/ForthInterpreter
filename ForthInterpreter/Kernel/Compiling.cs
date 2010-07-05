using ForthInterpreter.Interpret;
using ForthInterpreter.Interpret.Words;

namespace ForthInterpreter.Kernel
{
    public static class Compiling
    {
        public static Word[] Primitives { get { return primitives; } }

        private static Word[] primitives = new Word[]
        {
            new Word(":",
                env =>
                {
                    string wordName = Validate.ReadMandatoryWordName(env);
                    env.CompilingWord = new Word(wordName);
                    env.IsCompileMode = true;
                    
                    env.ControlFlowStack.Push(env.CompilingWord);
                }),
            new Word(";", true, true,
                env =>
                {
                    Word compilingWord = null;
                    if (env.ControlFlowStack.Count > 1)
                    {
                        Validate.PopControlFlowStack(env, false);
                        compilingWord = Validate.PopControlFlowStack(env, true, null, typeof(DefiningWord));
                    }
                    else
                        compilingWord = Validate.PopControlFlowStack(env, true);

                    env.Words.AddOrUpdate(compilingWord);
                    env.LastCompiledWord = compilingWord;
                    env.CompilingWord = null;
                    env.IsCompileMode = false;
                }),
            new Word("[", true, false,
                env =>
                {
                    env.IsCompileMode = false;
                }),
            new Word("]",
                env =>
                {
                    env.IsCompileMode = true;
                }),
            new Word("postpone", true, false,
                env =>
                {
                    Word word = Validate.ReadExistingWord(env);
                    (new PostponeWord(word)).Compile(env);
                }),
            new Word("immediate",
                env =>
                {
                    if (env.LastCompiledWord != null)
                        env.LastCompiledWord.IsImmediate = true;
                }),
            new Word("literal", true, true,
                env =>
                {
                    int cell = env.DataStack.Pop();
                    (new LiteralWord(cell)).CompileOrExecute(env);
                }),


            new Word("does>", true, true,
                env =>
                {
                    Word compilingWord = Validate.PopControlFlowStack(env, true);

                    DefiningWord definingWord = new DefiningWord(compilingWord.Name);
                    definingWord.ChildDefineWord.ExecuteWords.AddRange(compilingWord.ExecuteWords);
                    
                    env.CompilingWord = definingWord.ChildExecuteWord;
                    
                    env.ControlFlowStack.Push(definingWord);
                    env.ControlFlowStack.Push(definingWord.ChildExecuteWord);
                })
        };
    }
}
