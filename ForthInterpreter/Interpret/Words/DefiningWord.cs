using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForthInterpreter.Interpret.Words
{
    public class DefiningWord : Word
    {
        public DefiningWord(string name)
            : base(name)
        {
            ChildDefineWord = new Word("");
            ChildExecuteWord = new Word("");

            PrimitiveExecuteAction = defineChild;
        }

        public Word ChildDefineWord { get; private set; }
        public Word ChildExecuteWord { get; private set; }

        private void defineChild(Environment env)
        {
            ChildDefineWord.Execute(env);

            if (env.IsExitMode) return;

            VariableWord variableWord = env.LastCompiledWord as VariableWord;
            if (variableWord != null)
            {
                DefinedChildWord definedChildWord = new DefinedChildWord(variableWord.Name, Name, variableWord.AllocatedAddress);

                definedChildWord.ExecuteWords.Add(variableWord);
                definedChildWord.ExecuteWords.AddRange(ChildExecuteWord.ExecuteWords);
                variableWord.HidesSeeNodeDescription = true;

                env.Words.Remove(variableWord.Name);
                env.Words.AddOrUpdate(definedChildWord);
            }
            // TODO: else, throw exception (?)
        }

        public override string SeeRootDescription
        {
            get
            {
                return CleanFormat(string.Format(": {0}\n{1}\nDOES> {2};", 
                                   Name, ChildDefineWord.SeeEnumerateChildNodes, ChildExecuteWord.SeeEnumerateChildNodes));
            }
        }
    }
}
