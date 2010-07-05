using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ForthInterpreter.Interpret;
using NUnit.Framework;

namespace ForthInterpreterTests
{
    public abstract class TestHelper
    {
        #region Test SetUp

        [TestFixtureSetUp]
        public virtual void Init()
        {
            Interpreter = new Interpreter();
            System.Console.SetOut(new StringWriter(consoleStringBuilder));
        }

        [TestFixtureTearDown]
        public void Cleanup()
        {
            System.Console.SetOut(originalConsoleOut);
        }

        [SetUp]
        public void MethodSetUp()
        {
            Environment.Reset();
            Environment.LastCompiledWord = null;
            ResetConsoleOutput();
        }

        private readonly TextWriter originalConsoleOut = System.Console.Out;
        private readonly StringBuilder consoleStringBuilder = new StringBuilder(100, 5000);

        protected Interpreter Interpreter { get; private set; }

        #endregion


        #region Helpers

        protected Environment Environment { get { return Interpreter.Environment; } }

        protected bool OutputBufferOverflow { get; private set; }
        
        protected string LastConsoleOutput
        {
            get
            {
                string output = consoleStringBuilder.ToString();
                ResetConsoleOutput();
                return output;
            }
        }

        protected void ResetConsoleOutput()
        {
            consoleStringBuilder.Length = 0;
            OutputBufferOverflow = false;
        }

        protected void ClearStack()
        {
            Environment.DataStack.Clear();
        }

        protected void ClearReturnStack()
        {
            Environment.ReturnStack.Clear();
        }

        protected void Interpret(string text)
        {
            try
            {
                Interpreter.Interpret(text);
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                if (Regex.IsMatch(ex.Message, @"capacity was less than the current size", RegexOptions.IgnoreCase))
                    OutputBufferOverflow = true;
                else
                    throw;
            }
        }

        #endregion


        #region Return Stack Assertions

        protected void ReturnStackIsEmpty()
        {
            ReturnStackIsEmpty("");
        }

        protected void ReturnStackIsEmpty(string message)
        {
            StackIsEmpty(Environment.ReturnStack, concatenate(message, "Empty return stack expected."));
        }

        protected void ReturnStackEquals(params int[] expected)
        {
            ReturnStackEquals("", expected);
        }

        protected void ReturnStackEquals(string message, params int[] expected)
        {
            StackEquals(Environment.ReturnStack, concatenate(message, "Return stack comparison failed."), expected);
        }

        #endregion


        #region Stack Assertions

        protected void StacksAreEmpty()
        {
            StacksAreEmpty("");
        }

        protected void StacksAreEmpty(string message)
        {
            StackIsEmpty(message);
            ReturnStackIsEmpty(message);
        }

        protected void StackIsEmpty()
        {
            StackIsEmpty("");
        }

        protected void StackIsEmpty(string message)
        {
            StackIsEmpty(Environment.DataStack, concatenate(message, "Empty data stack expected."));
        }

        protected void StackIsEmpty(DataStack stack, string message)
        {
            if (stack.Count != 0)
                Assert.Fail("  {0}\n  Actual stack count: {1}  (Top cell: {2})", message, stack.Count, stack[0]);
        }

        protected void StackEquals(params int[] expected)
        {
            StackEquals("", expected);
        }

        protected void StackEquals(string message, params int[] expected)
        {
            StackEquals(Environment.DataStack, concatenate(message, "Data stack comparison failed."), expected);
        }

        protected void StackEquals(DataStack stack, string message, params int[] expected)
        {
            Assert.AreEqual(expected.Length, stack.Count, concatenate(message, "Wrong number of stack cells."));

            expected = expected.Reverse().ToArray();
            int[] actual = stack.ToArray();

            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i], concatenate(message, "Wrong stack cell value."));
        }

        #endregion


        #region Console Output Assertions

        protected void ConsoleOutputIsEmpty()
        {
            ConsoleOutputIsEmpty("");
        }

        protected void ConsoleOutputIsEmpty(string message)
        {
            string output = LastConsoleOutput;
            if (output != "")
                Assert.Fail(concatenate(" " + message, "No console output expected.\n  Actual console output: \"{0}\" (length {1})", 
                                                       output, output.Length));
        }

        protected void ConsoleOutputEquals(string expected)
        {
            ConsoleOutputEquals(expected, "");
        }

        protected void ConsoleOutputEquals(string expected, string message)
        {
            Assert.AreEqual(expected, LastConsoleOutput, concatenate(message, "Wrong console output."));
        }

        protected void IsOutputBufferOverflow()
        {
            IsOutputBufferOverflow("");
        }

        protected void IsOutputBufferOverflow(string message)
        {
            if (!OutputBufferOverflow)
                Assert.Fail(concatenate(" " + message, "Console output buffer overflow expected.\n  Actual output length: {0}", 
                                                        consoleStringBuilder.Length));
            ResetConsoleOutput();
        }

        private string concatenate(string first, string second, params object[] formatArgs)
        {
            return string.Format("{0}{1}{2}", first, first != "" ? " " : "", string.Format(second, formatArgs));
        }

        #endregion
    }
}
