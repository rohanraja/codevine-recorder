using CodeRecordHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeRecordHelpersTests
{
    [TestClass]
    public class RedisHelperTests
    {
		RedisHelper redisHelper;

		//[TestInitialize]
        public void InitializeRedisInstance()
		{
			redisHelper = RedisHelper.GetConnectedRedisHelper();
			Assert.IsTrue(redisHelper.IsConnected());

		}

		//[TestCleanup]
        public void DisposeRedisInstance()
        {
			redisHelper.Dispose();
        }

        //[TestMethod]
        public void AddingToQueue()
        {
			redisHelper.AddToQueue("testK", "TestVal1");
			redisHelper.AddToQueue("testK", "TestVal2");
        }
    }
}
