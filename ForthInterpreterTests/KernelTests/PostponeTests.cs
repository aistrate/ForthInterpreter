using ForthInterpreter.Interpret;
using NUnit.Framework;

namespace ForthInterpreterTests.KernelTests
{
    [TestFixture]
    public class PostponeTests : TestHelper
    {
        [Test]
        public void SimpleInterpret()
        {
            Interpret(@"postpone literal");
            StackIsEmpty();

            Interpret(@"postpone +");
            StackIsEmpty();

            ConsoleOutputIsEmpty();
        }

        [Test]
        public void PostponedImm_CompiledImm()
        {
            Interpret(@": ch
                          char postpone literal ; immediate");
            StackIsEmpty();

            Interpret(@"ch x");
            StackIsEmpty();

            Interpret(@": ccc
                          ch y emit ;");
            StackIsEmpty();

            ConsoleOutputIsEmpty();

            Interpret(@"ccc");
            StackIsEmpty();
            ConsoleOutputEquals("y");
        }

        [Test]
        public void PostponedNonImm_CompiledNonImm()
        {
            Interpret(@": plus0
                          postpone + ;");
            StackIsEmpty();

            Interpret(@"plus0");
            StackIsEmpty();


            Interpret(@": add0
                          321 654 plus0 ;");
            StackIsEmpty();

            Interpret(@"add0");
            StackEquals(321, 654);
            ClearStack();


            Interpret(@": plus1
                          plus0 ; immediate");
            StackIsEmpty();

            Interpret(@": add1
                          566 252 plus1 ;");
            StackIsEmpty();

            Interpret(@"add1");
            StackEquals(818);
            ClearStack();

            ConsoleOutputIsEmpty();
        }

        [Test]
        public void PostponedNonImm_CompiledImm()
        {
            Interpret(@": plus
                          postpone + ; immediate");
            StackIsEmpty();

            Interpret(@": add
                          789 987 plus ;");
            StackIsEmpty();

            Interpret(@"add");
            StackEquals(1776);
            ClearStack();

            ConsoleOutputIsEmpty();
        }

        [Test]
        public void PostponedImm_CompiledNonImm()
        {
            Interpret(@": ch0
                          char postpone literal ;");
            StackIsEmpty();

            Interpret(@"ch0");
            StackIsEmpty();

            Interpret(@"ch0 a");
            StackIsEmpty();

            Interpret(@"ch0 a 57");
            StackEquals(57);
            ClearStack();

            Interpret(@": ch1
                          ch0 ; immediate");
            StackIsEmpty();

            Interpret(@"ch1 s");
            StackIsEmpty();

            Interpret(@": ccc1
                          ch1 k emit ;");
            StackIsEmpty();

            ConsoleOutputIsEmpty();

            Interpret(@"ccc1");
            StackIsEmpty();
            ConsoleOutputEquals("k");
        }

        [Test]
        [ExpectedException(typeof(InvalidWordException), 
                           ExpectedMessage = "Undefined word.", MatchType = MessageMatch.Contains)]
        public void PostponedImm_CompiledNonImm_UndefWord()
        {
            Interpret(@": ch0
                          char postpone literal ;");

            Interpret(@": ccc0
                          ch0 t emit ;");
        }

        [Test]
        public void Piping()
        {
            Interpret(@": p0   postpone + ;");
            StackIsEmpty();
            
            Interpret(@": p1   p0 ;");
            Interpret(@": p2   p1 ;");
            Interpret(@": p3   p2 ;");
            StackIsEmpty();

            Interpret(@": p4   p3 ; immediate");
            StackIsEmpty();

            Interpret(@"p0");
            Interpret(@"p3");
            Interpret(@"p4");
            StackIsEmpty();

            Interpret(@": pp   222 333 p4 ;");
            StackIsEmpty();

            Interpret(@"pp");
            StackEquals(555);
            ClearStack();

            ConsoleOutputIsEmpty();
        }
    }
}
