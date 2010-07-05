using System.Reflection;
using ForthInterpreter.Interpret;
using ForthInterpreter.IO;
using NUnit.Framework;

namespace ForthInterpreterTests.KernelTests
{
    [TestFixture]
    public class ControlFlowTests : TestHelper
    {
        [TestFixtureSetUp]
        public override void Init()
        {
            base.Init();

            string setUpCode = FileLoader.ReadResource(Assembly.GetAssembly(typeof(ControlFlowTests)),
                                                       "ForthInterpreterTests.KernelTests.ControlFlowTests.fth");

            Interpret(setUpCode);
            StackIsEmpty();
            ConsoleOutputIsEmpty();
        }


        [Test]
        public void IfElseThen()
        {
            Interpret(@"1 testIf");
            StackIsEmpty();
            ConsoleOutputEquals("number is greater than zero");

            Interpret(@"0 testIf");
            StackIsEmpty();
            ConsoleOutputEquals("number is equal or less than zero");

            Interpret(@"-35 testIf");
            StackIsEmpty();
            ConsoleOutputEquals("number is equal or less than zero");
        }
        
        [Test]
        public void IfElseThen_InDoLoop()
        {
            string expectedOutput = @"
0 div by 4
1 not div by 2
2 div by 2
3 not div by 2
4 div by 4
5 not div by 2
6 div by 2
7 not div by 2
8 div by 4
9 not div by 2
10 div by 2
11 not div by 2
12 div by 4
13 not div by 2
14 div by 2
15 not div by 2
";

            Interpret(@"testDivLoop");
            StacksAreEmpty();

            ConsoleOutputEquals(expectedOutput);
        }

        [Test]
        public void FizzBuzz()
        {
            string expectedOutput = @"
1 2 fizz 4 buzz fizz 7 8 fizz buzz 
11 fizz 13 14 fizzbuzz 16 17 fizz 19 buzz 
fizz 22 23 fizz buzz 26 fizz 28 29 fizzbuzz 
31 32 fizz 34 buzz fizz 37 38 fizz buzz 
41 fizz 43 44 fizzbuzz 46 47 fizz 49 buzz 
fizz 52 53 fizz buzz 56 fizz 58 59 fizzbuzz 
61 62 fizz 64 buzz fizz 67 68 fizz buzz 
71 fizz 73 74 fizzbuzz 76 77 fizz 79 buzz 
fizz 82 83 fizz buzz 86 fizz 88 89 fizzbuzz 
91 92 fizz 94 buzz fizz 97 98 fizz buzz 
";

            Interpret(@"fizzBuzz");
            StacksAreEmpty();

            ConsoleOutputEquals(expectedOutput);
        }

        [Test]
        public void Exit()
        {
            Interpret(@"testExit");
            StackEquals(945);
            ClearStack();
            ConsoleOutputIsEmpty();
        }

        [Test]
        public void Exit_EmbeddedIfs()
        {
            Interpret(@"101 testExitCall");
            StackIsEmpty();
            ConsoleOutputEquals(@"result: number is not div by 2 (end callee) (end caller)");

            Interpret(@"102 testExitCall");
            StackIsEmpty();
            ConsoleOutputEquals(@"result: number is div by 2 (end if) (end callee) (end caller)");

            Interpret(@"104 testExitCall");
            StackIsEmpty();
            ConsoleOutputEquals(@"result: number is div by 4 (end caller)");
        }

        [Test]
        public void DoLoop()
        {
            Interpret(@"20 stars");
            StacksAreEmpty();
            ConsoleOutputEquals(@"Loop: * 0 * 1 * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9 * 10 * 11 * 12 * 13 * 14 * 15 * 16 * 17 * 18 * 19 end");
        }

        [Test]
        public void DoLoop_Once()
        {
            Interpret(@"5 5 loopOnce");
            StacksAreEmpty();
            ConsoleOutputEquals(@"5 ");
            
            Interpret(@"3 5 loopOnce");
            StacksAreEmpty();
            ConsoleOutputEquals(@"5 ");

            Interpret(@"6 5 loopOnce");
            StacksAreEmpty();
            ConsoleOutputEquals(@"5 ");

            Interpret(@"7 5 loopOnce");
            StacksAreEmpty();
            ConsoleOutputEquals(@"5 6 ");
        }

        [Test]
        public void DoLoops_Embedded()
        {
            Interpret(@"2loops");
            StacksAreEmpty();
            ConsoleOutputEquals(@"0 0   0 1   0 2   1 0   1 1   1 2   2 0   2 1   2 2   3 0   3 1   3 2   ");
        }

        [Test]
        public void DoPlusLoop()
        {
            Interpret(@"20 evenNums");
            StacksAreEmpty();
            ConsoleOutputEquals(@"b 0 2 4 6 8 10 12 14 16 18 e");

            Interpret(@"21 evenNums");
            StacksAreEmpty();
            ConsoleOutputEquals(@"b 0 2 4 6 8 10 12 14 16 18 20 e");
        }

        [Test]
        public void DoPlusLoop_Desc()
        {
            Interpret(@"descLoop");
            StacksAreEmpty();
            ConsoleOutputEquals(@"9 8 7 6 5 4 3 2 1 0 ");
        }

        [Test]
        public void DoPlusLoop_ZeroInc()
        {
            Interpret(@"-10 0 zeroIncLoop");
            StacksAreEmpty();
            IsOutputBufferOverflow();

            Interpret(@"10 0 zeroIncLoop");
            StacksAreEmpty();
            IsOutputBufferOverflow();

            Interpret(@"0 0 zeroIncLoop");
            StacksAreEmpty();
            IsOutputBufferOverflow();
        }

        [Test]
        public void QMarkDoLoop()
        {
            Interpret(@"0 spaces'");
            StacksAreEmpty();
            ConsoleOutputIsEmpty();

            Interpret(@"1 spaces'");
            StacksAreEmpty();
            ConsoleOutputEquals(@" ");

            Interpret(@"7 spaces'");
            StacksAreEmpty();
            ConsoleOutputEquals(@"       ");

            Interpret(@"-2 spaces'");
            StacksAreEmpty();
            ConsoleOutputEquals(@" ");
        }

        [Test]
        public void Unloop()
        {
            Interpret(@"unloopTest");
            StacksAreEmpty();
            ConsoleOutputEquals(@"0 0 * 0 1 * 0 2 * $   1 0 * 1 1 * 1 2 * $   2 0 ");
        }

        [Test]
        public void Leave()
        {
            Interpret(@"10 starsLeave");
            StacksAreEmpty();
            ConsoleOutputEquals(@"Loop: * 0 * 1 * 2 * 3 * end");
        }

        [Test]
        public void QMarkLeave()
        {
            Interpret(@"qleave");
            StacksAreEmpty();
            ConsoleOutputEquals(@"a 1 2 3 4 5 6 z");
        }

        [Test]
        public void Quit_DoLoop()
        {
            Interpret(@"111 222 333   10 starsQuit2Call");
            StackEquals(111, 222, 333);
            ReturnStackIsEmpty();
            ConsoleOutputEquals(@"(before 2) (before) Loop: * 0 * 1 * 2 * 3 * ");

            ClearStack();
            
            Interpret(@"15 starsQuit2Call");
            StacksAreEmpty();
            ConsoleOutputEquals(@"(before 2) (before) Loop: * 0 * 1 * 2 * 3 * ");
        }

        [Test]
        [ExpectedException(typeof(InvalidWordException),
                           ExpectedMessage = "Aborted.", MatchType = MessageMatch.Contains)]
        public void Abort_DoLoop()
        {
            Interpret(@"555 777 999   12 starsAbort");
        }

        [Test]
        public void BeginAgain()
        {
            Interpret(@"beginAgain");
            StacksAreEmpty();
            ConsoleOutputEquals(@"10 9 8 7 6 5 4 3 2 1 ");
        }

        [Test]
        public void BeginUntil()
        {
            Interpret(@"beginUntil");
            StacksAreEmpty();
            ConsoleOutputEquals(@"14 13 12 11 10 9 8 7 6 5 4 3 2 1 end");
        }

        [Test]
        public void BeginWhileRepeat()
        {
            Interpret(@"1000 fibonacci");
            StacksAreEmpty();
            ConsoleOutputEquals(@"0 1 1 2 3 5 8 13 21 34 55 89 144 233 377 610 987 end");
            
            Interpret(@"89 fibonacci'");
            StacksAreEmpty();
            ConsoleOutputEquals(@"0 1 1 2 3 5 8 13 21 34 55 89 end'");
        }

        [Test]
        public void BeginWhileRepeat_Exit()
        {
            Interpret(@"2000 fib-exit1");
            StacksAreEmpty();
            ConsoleOutputEquals(@"0 1 1 2 3 5 8 13 21 34 55 89 144 233 377 ");

            Interpret(@"3000 fib-exit2");
            StacksAreEmpty();
            ConsoleOutputEquals(@"0 1 1 2 3 5 8 13 21 34 55 89 144 233 377 610 ");
        }
        
        [Test]
        public void CaseEndCase()
        {
            string expectedOutput = @"
-1 : And who are you?
0 : And who are you?
1 : Mummy, I like you
2 : Pleased to meet you
3 : Hi!
4 : Hello
5 : Where's the coffee
6 : Yes?
7 : And who are you?
8 : And who are you?
";
            
            Interpret(@"teststyles");
            StacksAreEmpty();

            ConsoleOutputEquals(expectedOutput);
        }

        [Test]
        public void CaseEndCase_Exit()
        {
            Interpret(@"2 style-exit");
            StacksAreEmpty();
            ConsoleOutputEquals(@": Pleased to meet you Bye");
            
            Interpret(@"3 style-exit");
            StacksAreEmpty();
            ConsoleOutputEquals(@": ");
        }
    }
}
