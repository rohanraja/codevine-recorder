using System;
using System.Threading;
using CodeRecordHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CodeRecordHelpersTests
{
    [TestClass]
    public class PerfTests
    {
		CodeHooks codeHooks;
        private RedisHelper redisHelper;
        string KEY = "resque:queue:CODE_RUN_EVENTS";
        private int times;

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

            //MessageDispatcher.WaitToFinish();
            //Assert.IsTrue(redisHelper.ListCount(KEY) == times);

            //redisHelper.DelKey(KEY);
            redisHelper.Dispose();
        }

        [TestMethod]
        public void TestBenchmarkHookCalls()
        {

            times = 10000;

            ExecuteHookNTimes(times);
            Console.WriteLine("BENCHMARKING COMPLETE");

        }

        private void ExecuteHookNTimes(int times = 1)
		{

            for(int i=0; i< times; i++)
            {
                codeHooks.AddSourceFile("classBDummy.cs", string.Format("Call #{0} - This\nis\na\nreally\ninteresting\ncode", i));
            }

		}
    }
}
