using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class VbsTest
    {
        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext { get; set; }

        public VBS eng = new VBS();


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="output">输出接口</param>
        public VbsTest()
        {
            Cache.runPath = Directory.GetCurrentDirectory() + "\\";
        }

        /// <summary>
        /// 遍历加载
        /// </summary>
        [TestMethod()]
        public void EachLoad()
        {
            List<string> appList = new List<string>() { Cache.runPath + "script\\test.vbs", Cache.runPath + "script\\vbs\\test1.vbs" };

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
            string file = Cache.runPath + "script\\vbs\\test1.vbs";

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
            string file = Cache.runPath + "script\\vbs\\test1.vbs";
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
            string file = Cache.runPath + "script\\vbs\\test1.vbs";
            string fun = "Main";
            //var bl = eng.Load(file, fun);
            object param1 = 1;
            object param2 = 2;
            object param3 = 3;

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
            string file = Cache.runPath + "script\\test.vbs";
            string fun = "Main";
            // var bl = eng.Load(file, fun);
            object param1 = 1;
            object param2 = 2;
            object param3 = 3;

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
            string file = Cache.runPath + "script\\test.vbs";
            string fun = "Main";
            object param1 = 1;
            object param2 = 2;
            object param3 = 3;

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
public function Main(fun, param1, param2, param3)
	Dim count
	count = param1 + param2 + param3
	Main = count
end function";
            string fun = "Main";
            object param1 = 1;
            object param2 = 2;
            object param3 = 3;

            var ret = eng.RunCode(code, fun, param1, param2, param3);
            Console.WriteLine(eng.GetEx());
            Console.WriteLine(ret.ToString());
            Assert.IsTrue(ret != null);
        }
    }
}
