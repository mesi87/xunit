using TestDriven.Framework;
using Xunit.Abstractions;

namespace Xunit.Runner.TdNet
{
    public class ResultVisitor : TestMessageVisitor<ITestAssemblyFinished>
    {
        public ResultVisitor(ITestListener listener)
        {
            TestListener = listener;
            TestRunState = TestRunState.NoTests;
        }

        public ITestListener TestListener { get; private set; }
        public TestRunState TestRunState { get; set; }

        protected override void Visit(ITestFailed failed)
        {
            TestRunState = TestRunState.Failure;

            TestResult testResult = failed.ToTdNetTestResult(TestState.Failed);

            testResult.Message = failed.Exception.Message;
            testResult.StackTrace = failed.Exception.StackTrace;

            TestListener.TestFinished(testResult);

            //WriteOutput(name, output);
        }

        protected override void Visit(ITestPassed passed)
        {
            if (TestRunState == TestRunState.NoTests)
                TestRunState = TestRunState.Success;

            TestResult testResult = passed.ToTdNetTestResult(TestState.Passed);

            TestListener.TestFinished(testResult);

            //WriteOutput(name, output);
        }

        protected override void Visit(ITestSkipped skipped)
        {
            if (TestRunState == TestRunState.NoTests)
                TestRunState = TestRunState.Success;

            TestResult testResult = skipped.ToTdNetTestResult(TestState.Ignored);

            testResult.Message = skipped.Reason;

            TestListener.TestFinished(testResult);
        }
    }
}