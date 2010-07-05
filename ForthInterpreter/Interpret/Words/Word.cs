using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ForthInterpreter.Interpret.Words.ControlFlow;
using System.Text.RegularExpressions;

namespace ForthInterpreter.Interpret.Words
{
    public class Word
    {
        public Word(string name)
            : this(name, null)
        {
        }

        public Word(string name, Action<Environment> primitiveExecuteAction)
            : this(name, false, false, primitiveExecuteAction)
        {
        }

        public Word(string name, bool isImmediate, bool isCompileOnly, Action<Environment> primitiveExecuteAction)
        {
            Name = name;
            IsImmediate = isImmediate;
            IsCompileOnly = isCompileOnly;
            
            PrimitiveExecuteAction = primitiveExecuteAction;
            ExecuteWords = new List<Word>();
        }

        public string Name { get; private set; }
        public bool IsImmediate { get; set; }
        public bool IsCompileOnly { get; private set; }

        public Action<Environment> PrimitiveExecuteAction { get; protected set; }
        public List<Word> ExecuteWords { get; private set; }

        public bool IsPrimitive { get { return PrimitiveExecuteAction != null && ExecuteWords.Count == 0; } }

        public virtual string[] RecognizedExitWordNames { get { return new[] { "exit" }; } }

        public void Compile(Environment env)
        {
            if (env.IsCompileMode) 
                env.CompilingWord.ExecuteWords.Add(this);
        }

        public void Execute(Environment env)
        {
            if (!env.IsExitMode)
            {
                if (IsPrimitive)
                    PrimitiveExecuteAction(env);
                else
                    ExecuteWords.ForEach(word => { if (!env.IsExitMode) word.Execute(env); });

                if (env.IsExitMode && RecognizedExitWordNames.Contains(env.ActiveExitWordName))
                    env.ActiveExitWordName = null;
            }
        }

        public void CompileOrExecute(Environment env)
        {
            if (env.IsCompileMode)
            {
                if (!IsImmediate)
                    Compile(env);
                else
                    Execute(env);
            }
        }

        public void Interpret(Environment env)
        {
            if (env.IsCompileMode)
                CompileOrExecute(env);
            else if (!IsCompileOnly)
                Execute(env);
            else
                throw new ApplicationException("Interpreting a compile-only word.");
        }

        
        public string SeeEnumerateChildNodes
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                ExecuteWords.ForEach(word => sb.Append(word.SeeNodeDescription + " "));
                return sb.ToString();
            }
        }

        public virtual string SeeNodeDescription { get { return Name; } }

        public virtual string SeeRootDescription
        {
            get
            {
                if (IsPrimitive)
                    return string.Format("Word {0} is primitive", Name);
                else
                    return CleanFormat(string.Format(": {0}\n{1};", Name, SeeEnumerateChildNodes));
            }
        }

        protected string CleanFormat(string description)
        {
            description = Regex.Replace(description, @"\n\s*\n", "\n");
            description = Regex.Replace(description, @"\t\n\s*", "\t");
            description = Regex.Replace(description, @"\n\s*;", " ;");
            description = description.Replace("\n ", "\n").Replace("\t ", "\t")
                                     .Replace("\n", "\n  ");

            return description;
        }
    }
}
