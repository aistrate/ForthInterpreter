using ForthInterpreter.DataTypes;
using ForthInterpreter.Interpret;
using ForthInterpreter.Interpret.Words;
using ForthInterpreter.Interpret.Words.ControlFlow;

namespace ForthInterpreter.Kernel
{
    public static class ControlFlow
    {
        public static Word[] Primitives { get { return primitives; } }

        private static Word[] primitives = new Word[]
        {
            new Word("if", true, true,
                env =>
                {
                    IfElseThenWord ifElseThenWord = new IfElseThenWord(env);
                    ifElseThenWord.Compile(env);
                    env.CompilingWord = ifElseThenWord.TrueBranchWord;

                    env.ControlFlowStack.Push(ifElseThenWord);
                    env.ControlFlowStack.Push(ifElseThenWord.TrueBranchWord);
                }),
            new Word("else", true, true,
                env =>
                {
                    Validate.PopControlFlowStack(env, false, "branch", typeof(ControlFlowWord));
                    IfElseThenWord ifElseThenWord = (IfElseThenWord)Validate.PeekControlFlowStack(env, false, "if", typeof(IfElseThenWord));
                    
                    env.CompilingWord = ifElseThenWord.FalseBranchWord;
                    env.ControlFlowStack.Push(ifElseThenWord.FalseBranchWord);
                }),
            new Word("then", true, true,
                env =>
                {
                    Validate.PopControlFlowStack(env, false, "branch", typeof(ControlFlowWord));
                    Validate.PopControlFlowStack(env, false, "if", typeof(IfElseThenWord));
                    
                    env.CompilingWord = env.ControlFlowStack.Peek();
                }),
            
            
            new ExitWord("exit"),
            
            // 'quit' and 'abort' never caught by any word
            new ExitWord("quit", env => env.ReturnStack.Clear() ),
            new ExitWord("abort", env => { env.DataStack.Clear(); env.ReturnStack.Clear(); }),

            
            new Word("do", true, true,
                env =>
                {
                    DoLoopWord doLoopWord = new DoLoopWord(env);
                    doLoopWord.Compile(env);
                    env.CompilingWord = doLoopWord.CycleWord;

                    env.ControlFlowStack.Push(doLoopWord);
                    env.ControlFlowStack.Push(doLoopWord.CycleWord);
                }),
            new Word("?do", true, true,
                env =>
                {
                    DoLoopWord doLoopWord = new DoLoopWord(env);
                    doLoopWord.PerformsInitialTest = true;
                    doLoopWord.Compile(env);
                    env.CompilingWord = doLoopWord.CycleWord;

                    env.ControlFlowStack.Push(doLoopWord);
                    env.ControlFlowStack.Push(doLoopWord.CycleWord);
                }),
            new Word("loop", true, true,
                env =>
                {
                    Validate.PopControlFlowStack(env, false, "cycle", typeof(ControlFlowWord));
                    Validate.PopControlFlowStack(env, false, "do", typeof(DoLoopWord));
                    
                    env.CompilingWord = env.ControlFlowStack.Peek();
                }),
            new Word("+loop", true, true,
                env =>
                {
                    Validate.PopControlFlowStack(env, false, "cycle", typeof(ControlFlowWord));
                    DoLoopWord doLoopWord = (DoLoopWord)Validate.PopControlFlowStack(env, false, "do", typeof(DoLoopWord));
                    doLoopWord.ReadsIncrement = true;

                    env.CompilingWord = env.ControlFlowStack.Peek();
                }),
            new Word("i",
                env =>
                {
                    env.DataStack.Push(env.ReturnStack.Peek());
                }),
            new Word("j",
                env =>
                {
                    env.DataStack.Push(env.ReturnStack[2]);
                }),
            new Word("unloop",
                env =>
                {
                    env.ReturnStack.Pop();
                    env.ReturnStack.Pop();
                }),
            new ExitWord("leave", env => { env.ReturnStack.Pop(); env.ReturnStack.Pop(); }),
            new ExitWord("?leave", true, env => { env.ReturnStack.Pop(); env.ReturnStack.Pop(); }),
            
            
            new Word("begin", true, true,
                env =>
                {
                    BeginWord beginWord = new BeginWord(env);
                    beginWord.Compile(env);
                    env.CompilingWord = beginWord.CycleWord;

                    env.ControlFlowStack.Push(beginWord);
                    env.ControlFlowStack.Push(beginWord.CycleWord);
                }),
            new Word("again", true, true,
                env =>
                {
                    Validate.PopControlFlowStack(env, false, "cycle", typeof(ControlFlowWord));
                    Validate.PopControlFlowStack(env, false, "begin", typeof(BeginWord));

                    env.CompilingWord = env.ControlFlowStack.Peek();
                }),
            new Word("until", true, true,
                env =>
                {
                    Validate.PopControlFlowStack(env, false, "cycle", typeof(ControlFlowWord));
                    BeginWord beginWord = (BeginWord)Validate.PopControlFlowStack(env, false, "begin", typeof(BeginWord));
                    
                    beginWord.TestsUntil = true;
                    
                    env.CompilingWord = env.ControlFlowStack.Peek();
                }),

            
            new Word("while", true, true,
                env =>
                {
                    ControlFlowWord oldCycleWord = (ControlFlowWord)Validate.PopControlFlowStack(env, false, "cycle", typeof(ControlFlowWord));
                    Validate.PopControlFlowStack(env, false, "begin", typeof(BeginWord));
                    
                    Word parentWord = env.ControlFlowStack.Peek();
                    BeginWhileRepeatWord beginWhileRepeatWord = new BeginWhileRepeatWord(env);
                    parentWord.ExecuteWords[parentWord.ExecuteWords.Count - 1] = beginWhileRepeatWord;

                    beginWhileRepeatWord.WhileTestWord.ExecuteWords.AddRange(oldCycleWord.ExecuteWords);
                    
                    env.CompilingWord = beginWhileRepeatWord.CycleWord;

                    env.ControlFlowStack.Push(beginWhileRepeatWord);
                    env.ControlFlowStack.Push(beginWhileRepeatWord.CycleWord);
                }),
            new Word("repeat", true, true,
                env =>
                {
                    Validate.PopControlFlowStack(env, false, "cycle", typeof(ControlFlowWord));
                    Validate.PopControlFlowStack(env, false, "begin-while", typeof(BeginWhileRepeatWord));
                    
                    env.CompilingWord = env.ControlFlowStack.Peek();
                }),


            new Word("case", true, true,
                env =>
                {
                    CaseWord caseWord = new CaseWord(env);
                    caseWord.Compile(env);
                    env.CompilingWord = caseWord.CaseBodyWord;

                    env.ControlFlowStack.Push(caseWord);
                    env.ControlFlowStack.Push(caseWord.CaseBodyWord);
                }),
            new Word("endcase", true, true,
                env =>
                {
                    Validate.PopControlFlowStack(env, false, "case-body", typeof(ControlFlowWord));
                    Validate.PopControlFlowStack(env, false, "case", typeof(CaseWord));
                    
                    env.CompilingWord = env.ControlFlowStack.Peek();
                }),
            new Word("of", true, true,
                env =>
                {
                    Validate.PeekControlFlowStack(env, false, "case-body", typeof(ControlFlowWord));

                    OfWord ofWord = new OfWord(env);
                    ofWord.Compile(env);
                    env.CompilingWord = ofWord.OfBodyWord;

                    env.ControlFlowStack.Push(ofWord);
                    env.ControlFlowStack.Push(ofWord.OfBodyWord);
                }),
            new Word("endof", true, true,
                env =>
                {
                    Validate.PopControlFlowStack(env, false, "of-body", typeof(ControlFlowWord));
                    Validate.PopControlFlowStack(env, false, "of", typeof(OfWord));
                    
                    env.CompilingWord = env.ControlFlowStack.Peek();
                })
        };
    }
}
