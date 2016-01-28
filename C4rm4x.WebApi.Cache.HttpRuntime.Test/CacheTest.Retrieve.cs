﻿#region Using

using C4rm4x.Tools.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Caching;
using Caching = System.Web.Caching.Cache;

#endregion

namespace C4rm4x.WebApi.Cache.HttpRuntime.Test
{
    public partial class CacheTest
    {
        [TestClass]
        public class CacheRetrieveTest : CacheTestFixture
        {
            [TestMethod, UnitTest]
            public void Retrieve_Returns_Null_When_No_Entry_Exists_With_Specified_Key()
            {
                Assert.IsNull(_sut.Retrieve(ObjectMother.Create<string>()));
            }

            [TestMethod, UnitTest]
            public void Retrieve_Returns_Value_When_Entry_Exists_With_Specified_Key()
            {
                var Key = ObjectMother.Create<string>();
                var Value = ObjectMother.Create<string>();

                AddEntry(Key, Value);

                var result = _sut.Retrieve(Key);

                Assert.IsNotNull(result);
                Assert.AreSame(Value, result);
            }

            [TestMethod, UnitTest]
            [ExpectedException(typeof(InvalidCastException))]
            public void Retrieve_Throws_InvalidCastException_When_Value_Of_Entry_With_Specified_Key_Is_Not_Of_Specified_Type()
            {
                var Key = ObjectMother.Create<string>();
                var Value = ObjectMother.Create<string>();

                AddEntry(Key, Value);

                _sut.Retrieve<int>(Key);
            }

            [TestMethod, UnitTest]
            public void Retrieve_Returns_Value_As_Specified_Type_When_Entry_With_Specified_Key_Exists_And_Value_Is_Of_Specified_Type()
            {
                var Key = ObjectMother.Create<string>();
                var Value = ObjectMother.Create<TestClass>();

                AddEntry(Key, Value);

                var result = _sut.Retrieve<TestClass>(Key);

                Assert.IsNotNull(result);
                Assert.AreSame(Value, result);
            }

            private static void AddEntry<TValue>(
                string key,
                TValue value)
            {
                HttpCache.Add(key, value, null,
                    DateTime.Now.AddSeconds(10),
                    Caching.NoSlidingExpiration,
                    CacheItemPriority.Normal, null);
            }
        }
    }
}
