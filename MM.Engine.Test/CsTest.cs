﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MM.Engine.Test
{
    /// <summary>
    /// python脚本引擎测试
    /// </summary>
    [TestClass()]
    public class CsTest
    {
        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext { get; set; }

        public CS eng = new CS();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="output">输出接口</param>
        public CsTest()
        {
            Cache.runPath = Directory.GetCurrentDirectory() + "\\";
        }

        /// <summary>
        /// 遍历加载
        /// </summary>
        [TestMethod()]
        public void EachLoad()
        {
            List<string> appList = new List<string>() { Cache.runPath + "script\\test.cs", Cache.runPath + "script\\cs\\test1.cs" };

            var bl = eng.EachLoad(appList);
            Console.WriteLine(bl.ToString());
            Assert.IsTrue(bl);
        }

        /// <summary>
        /// 载入脚本
        /// </summary>
        [TestMethod()]
        public void Load()
        {
            string file = Cache.runPath + "script\\cs\\test1.cs";

            var bl = eng.Load(file);
            Console.WriteLine(bl.ToString());
            Assert.IsTrue(bl);
        }

        /// <summary>
        /// 卸载脚本
        /// </summary>
        [TestMethod()]
        public void Unload()
        {
            string file = Cache.runPath + "script\\cs\\test1.cs";
            eng.Load(file);
            string appName = file;
            var count = eng.Dict.Count;
            var bl = eng.Unload(appName);
            Console.WriteLine(count + ">" + eng.Dict.Count);
            Assert.IsTrue(bl);
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        [TestMethod()]
        public void Run()
        {
            string file = Cache.runPath + "script\\cs\\test1.cs";
            string fun = "Main";
            //var bl = eng.Load(file, fun);
            int param1 = 1;
            int param2 = 2;
            int param3 = 3;
            
            var ret = eng.Run(file, fun, param1, param2, param3);
            Console.WriteLine(ret.ToJson());
            ret = eng.Run(file, fun, param1, param2, param3);
            Console.WriteLine(ret.ToJson());
            Assert.IsTrue(ret != null);
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        [TestMethod()]
        public void RunAsync()
        {
            string file = Cache.runPath + "script\\test.cs";
            string fun = "Main";
            // var bl = eng.Load(file, fun);
            int param1 = 1;
            int param2 = 2;
            int param3 = 3;

            eng.RunAsync(file, fun, param1, param2, param3);
            Thread.Sleep(10000);
            Console.WriteLine(Cache.res.ToJson());
            Assert.IsTrue(Cache.res.Count > 0);
        }

        /// <summary>
        /// 执行脚本文件
        /// </summary>
        /// <returns>返回执行结果</returns>
        [TestMethod()]
        public void RunFile()
        {
            string file = Cache.runPath + "script\\test.cs";
            string fun = "Main";
            int param1 = 1;
            int param2 = 2;
            int param3 = 3;

            var ret = eng.RunFile(file, fun, param1, param2, param3);
            Console.WriteLine(eng.GetEx());
            Assert.IsTrue(ret != null);
            Console.WriteLine(ret.ToString());
            ret = eng.RunFile(file, fun, param1, param2, param3);
            Console.WriteLine(eng.GetEx());
            Assert.IsTrue(ret != null);
            Console.WriteLine(ret.ToString());
        }

        /// <summary>
        /// 执行脚本代码
        /// </summary>
        [TestMethod()]
        public void RunCode()
        {
            string code = @"
public object Main(object fun, object param1 = null, object param2 = null, object param3 = null){
	var num1 = (int)param1;
	var num2 = (int)param2;
	var num3 = (int)param3;
	var ret = " + "\"测试CS脚本：\"" + @" + ( num1 + num2 + num3 );
    return ret;
}";
            string fun = "Main";
            int param1 = 1;
            int param2 = 2;
            int param3 = 3;

            var ret = eng.RunCode(code, fun, param1, param2, param3);
            Console.WriteLine(eng.GetEx());
            Console.WriteLine(ret.ToString());
            Assert.IsTrue(ret != null);
        }
    }
}
