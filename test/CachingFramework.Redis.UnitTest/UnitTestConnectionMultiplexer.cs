﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CachingFramework.Redis.Serializers;
using CachingFramework.Redis.RedisObjects;

namespace CachingFramework.Redis.UnitTest
{
    [TestFixture]
    public class UnitTestConnectionMultiplexer
    {
        [Test]
        public void Test_CustomMultiplexer()
        {
            var myMultiplexer = new PooledConnectionMultiplexer(Common.Config);
            using (var ctx = new Context(myMultiplexer))
            {
                ctx.Cache.SetObject("Test_CustomMultiplexer_obj", "Test_CustomMultiplexer_value");
                var list = ctx.Collections.GetRedisDictionary<string, string>("Test_CustomMultiplexer_hash");
                list.Add("test", "value");
            }

            using (var ctx = new Context(myMultiplexer))
            {
                Assert.AreEqual("Test_CustomMultiplexer_value", ctx.Cache.GetObject<string>("Test_CustomMultiplexer_obj"));
                var dict = ctx.Collections.GetRedisDictionary<string, string>("Test_CustomMultiplexer_hash");
                Assert.AreEqual("value", dict["test"]);
                ctx.Cache.Remove("Test_CustomMultiplexer_obj");
                ctx.Cache.Remove("Test_CustomMultiplexer_hash");
            }
        }
    }
}
