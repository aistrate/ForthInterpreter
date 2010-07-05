using System.Reflection;
using ForthInterpreter.IO;
using NUnit.Framework;

namespace ForthInterpreterTests.KernelTests
{
    [TestFixture]
    public class CompileTests : TestHelper
    {
        [TestFixtureSetUp]
        public override void Init()
        {
            base.Init();

            string setUpCode = FileLoader.ReadResource(Assembly.GetAssembly(typeof(CompileTests)),
                                                       "ForthInterpreterTests.KernelTests.CompileTests.fth");

            Interpret(setUpCode);
            StackIsEmpty();
            ConsoleOutputIsEmpty();
        }


        [Test]
        public void CreateDoes_Array()
        {
            Interpret(@"test-array-size bb");
            StackIsEmpty();
            ConsoleOutputEquals("size test: success");

            Interpret(@"test-array");
            StackIsEmpty();
            ConsoleOutputEquals("assign-retrieve test: success");
        }

        [Test]
        public void CreateDoes_Constant()
        {
            Interpret(@"test-const");
            StackIsEmpty();
            ConsoleOutputEquals("success");
        }
    }
}
