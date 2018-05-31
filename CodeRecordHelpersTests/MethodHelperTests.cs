using System;
using System.Threading;
using CodeRecordHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CodeRecordHelpersTests
{
    [TestClass]
    public class MethodHelperTests
    {
		CodeHooks codeHooks;
        private RedisHelper redisHelper;
        string KEY = "resque:queue:CODE_RUN_EVENTS";

        [TestInitialize]
        public void Init()
		{
            codeHooks = CodeHooks.Instance();
            redisHelper = RedisHelper.GetConnectedRedisHelper();
            redisHelper.DelKey(KEY);
		}

		[TestCleanup]
        public void Clean()
        {
            redisHelper.DelKey(KEY);
            redisHelper.Dispose();
        }

        [TestMethod]
        public void TestLoggingLine_MockDispatcher()
        {
			var mock = new Mock<IMessageDispatcher>();

			RedisMessage msg = new RedisMessage("","");

            codeHooks = new CodeHooks(mock.Object);

			mock.Setup(x => x.DispatchMessage(It.IsAny<RedisMessage>())).Callback( (RedisMessage pmsg) => msg = pmsg ) ;

			codeHooks.LogLineRun(System.Guid.NewGuid(), 95, "testDate");

			mock.Verify(x => x.DispatchMessage(It.IsAny<RedisMessage>()), Times.Once());

            string tid = Thread.CurrentThread.ManagedThreadId.ToString();

			Assert.IsTrue(msg.GetKey().Contains("CODE_RUN_EVENTS"));
			Assert.IsTrue(msg.GetMessage().Contains("95"));
			Assert.IsTrue(msg.GetMessage().Contains("testDate"));
			Assert.IsTrue(msg.GetMessage().Contains("LINE_EXEC"));
			Assert.IsTrue(msg.GetMessage().Contains(tid));

        }

		[TestMethod]
        public void TestLoggingLine_REDIS()
		{
            int times = 20;
            for(int i=0; i< times; i++)
            {
                codeHooks.AddSourceFile("classBDummy.cs", "This\nis\na\nreally\ninteresting\ncode");
            }

            MessageDispatcher.WaitToFinish();

            Assert.IsTrue(redisHelper.ListCount(KEY) == times);

		}
    }
}
