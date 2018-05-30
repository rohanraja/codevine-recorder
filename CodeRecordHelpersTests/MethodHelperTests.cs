using System;
using CodeRecordHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CodeRecordHelpersTests
{
    [TestClass]
    public class MethodHelperTests
    {
		CodeHooks methodHelpers;

		[TestInitialize]
        public void Init()
		{
		}

		[TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void TestLoggingLine_MockDispatcher()
        {
			var mock = new Mock<IMessageDispatcher>();

			RedisMessage msg = new RedisMessage("","");

			mock.Setup(x => x.DispatchMessage(It.IsAny<RedisMessage>())).Callback( (RedisMessage pmsg) => msg = pmsg ) ;

			methodHelpers = new CodeHooks( mock.Object );

			methodHelpers.LogLineRun(System.Guid.NewGuid(), 95, "testDate");

			mock.Verify(x => x.DispatchMessage(It.IsAny<RedisMessage>()), Times.Once());

			Assert.IsTrue(msg.GetKey().Contains("CODE_RUN_EVENTS"));
			Assert.IsTrue(msg.GetMessage().Contains("95"));
			Assert.IsTrue(msg.GetMessage().Contains("testDate"));
			Assert.IsTrue(msg.GetMessage().Contains("LINE_EXEC"));

        }

		//[TestMethod]
        public void TestLoggingLine_REDIS()
		{
			methodHelpers = CodeHooks.Instance();
			//methodHelpers.AddSourceFile("newClass.cs", "line1\nline2\nline3\nline4\nline5");
			//methodHelpers.AddSourceFile("classB.cs", "This\nis\na\nreally\ninteresting\ncode");
			var mrid = methodHelpers.OnMethodEnter("Program.cs", "MethodA");
			methodHelpers.LogLineRun(mrid, 8, "09-04-1993");
            mrid = methodHelpers.OnMethodEnter( "ClassA.cs", "MethodB");
            methodHelpers.LogLineRun(mrid, 13, "09-04-1994");
            methodHelpers.LogLineRun(mrid, 15, "09-04-1995");

			//mrid = Guid.NewGuid();
   //         methodHelpers.OnMethodEnter(mrid, "ClassB.cs", "MethodC");
			//methodHelpers.LogLineRun(mrid, 10, "09-04-1994");
   //         methodHelpers.LogLineRun(mrid, 12, "09-04-1995");

			//mrid = Guid.NewGuid();
   //         methodHelpers.OnMethodEnter(mrid, "ClassA.cs", "MethodB");
   //         methodHelpers.LogLineRun(mrid, 15, "09-04-1996");

			//mrid = Guid.NewGuid();
   //         methodHelpers.OnMethodEnter(mrid, "Program.cs", "MethodB");
   //         methodHelpers.LogLineRun(mrid, 8, "09-04-1997");
			//methodHelpers.LogLineRun(mrid, 9, "09-04-1998");
			//methodHelpers.LogLineRun(mrid, 10, "09-04-1999");
		}
    }
}
