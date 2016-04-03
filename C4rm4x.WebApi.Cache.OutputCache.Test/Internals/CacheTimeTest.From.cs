#region Using

using C4rm4x.Tools.TestUtilities;
using C4rm4x.WebApi.Cache.OutputCache.Internals;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

#endregion

namespace C4rm4x.WebApi.Cache.OutputCache.Test.Internals
{
    public partial class CacheTimeTest
    {
        [TestClass]
        public class CacheTimeFromTest
        {
            [TestMethod, UnitTest]
            public void From_Returns_Instance_Of_CacheTime_Where_AbsoluteExpirationTime_Is_Now_Plus_ServerTimeSpan_In_Seconds()
            {
                var now = DateTime.UtcNow;
                var ServerTimeSpan = ObjectMother.Create<int>();

                Assert.AreEqual(
                    now.AddSeconds(ServerTimeSpan),
                    CacheTime.From(ServerTimeSpan, It.IsAny<int>(), now)
                        .AbsoluteExpirationTime);
            }

            [TestMethod, UnitTest]
            public void From_Returns_Instance_Of_CacheTime_Where_ClientTimeSpan_Is_ClientTimeSpan_In_Seconds()
            {
                var ClientTimeSpan = ObjectMother.Create<int>();

                Assert.AreEqual(
                    TimeSpan.FromSeconds(ClientTimeSpan),
                    CacheTime.From(It.IsAny<int>(), ClientTimeSpan, It.IsAny<DateTime>())
                        .ClientTimeSpan);
            }
        }
    }
}
