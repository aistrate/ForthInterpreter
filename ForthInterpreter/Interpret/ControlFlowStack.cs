using System;
using System.Collections.Generic;
using ForthInterpreter.Interpret.Words;
using ForthInterpreter.Interpret.Words.ControlFlow;

namespace ForthInterpreter.Interpret
{
    public class ControlFlowStack : Stack<Word>
    {
        public bool TopWordIsValid(bool countIsOne, string definingWordName, Type type)
        {
            if (Count > 0)
            {
                Word topWord = Peek();

                if ((Count == 1) == countIsOne &&
                    (definingWordName == null || 
                        (topWord is ControlFlowWord && ((ControlFlowWord)topWord).DefiningWordName == definingWordName)) &&
                    (type == null || topWord.GetType() == type))
                    return true;
            }

            return false;
        }
    }
}
