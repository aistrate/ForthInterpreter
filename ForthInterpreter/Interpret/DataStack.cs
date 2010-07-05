using System;
using System.Collections.Generic;
using System.Linq;

namespace ForthInterpreter.Interpret
{
    public class StackUnderflowException : InvalidOperationException
    {
        public StackUnderflowException(string stackName)
            : base (string.Format("{0} underflow.", stackName))
        {
        }

        public static InvalidOperationException SubstituteNewIfStackEmpty(InvalidOperationException ex, string stackName)
        {
            if (ex.Message == "Stack empty.")
                return new StackUnderflowException(stackName);
            else
                return ex;
        }
    }


    public class DataStack : Stack<int>
    {
        public DataStack()
            : this("Stack")
        {
        }

        public DataStack(string stackName)
        {
            StackName = stackName;
        }

        public string StackName { get; private set; }

        public new int Peek()
        {
            try
            {
                return base.Peek();
            }
            catch (InvalidOperationException ex)
            {
                throw StackUnderflowException.SubstituteNewIfStackEmpty(ex, StackName);
            }
        }

        public new int Pop()
        {
            try
            {
                return base.Pop();
            }
            catch (InvalidOperationException ex)
            {
                throw StackUnderflowException.SubstituteNewIfStackEmpty(ex, StackName);
            }
        }

        public new void Push(int item)
        {
            if (Count < 1048576)
                base.Push(item);
            else
                throw new StackOverflowException("Stack overflow.");
        }

        public int this[int index]
        {
            get
            {
                foreach (int item in this)
                    if (index-- == 0)
                        return item;
                return 0;
            }
        }

        public void Print(int maxCount)
        {
            Console.Write("<{0}> ", Count);
            foreach (int cell in this.Take(maxCount).Reverse())
                Console.Write("{0} ", cell);
        }
    }
}
