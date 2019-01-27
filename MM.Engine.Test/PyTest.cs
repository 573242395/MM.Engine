using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace MM.Engine.Test
{
    /// <summary>
    /// python脚本引擎测试
    /// </summary>
    public class PyTest
    {
        public PY eng = new PY();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="output">输出接口</param>
        public PyTest(ITestOutputHelper output)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.TestOutput(output, Serilog.Events.LogEventLevel.Verbose)
                .CreateLogger();
            Cache.runPath = Directory.GetCurrentDirectory() + "\\";
            PY.Init();
        }

        /// <summary>
        /// 遍历加载
        /// </summary>
        [Fact]
        private void EachLoad()
        {
            List<string> appList = new List<string>() { Cache.runPath + "script\\test.py", Cache.runPath + "script\\py\\test1.py" };

            var bl = eng.EachLoad(appList);
            Log.Debug(bl.ToString());
            Assert.True(bl);
        }

        /// <summary>
        /// 载入脚本
        /// </summary>
        [Fact]
        private void Load()
        {
            string file = Cache.runPath + "script\\py\\test1.py";

            var bl = eng.Load(file);
            Log.Debug(bl.ToString());
            Assert.True(bl);
        }

        /// <summary>
        /// 卸载脚本
        /// </summary>
        [Fact]
        private void Unload()
        {
            string file = Cache.runPath + "script\\py\\test1.py";
            eng.Load(file);
            string appName = file + ":Main";
            var bl = eng.Unload(appName);

            Log.Debug(bl.ToString());
            Assert.True(bl);
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        [Fact]
        public void Run()
        {
            string file = Cache.runPath + "script\\py\\test1.py";
            string fun = "Main";
            //var bl = eng.Load(file, fun);
            object param1 = 1;
            object param2 = 2;
            object param3 = 3;

            var ret = eng.Run(file, fun, param1, param2, param3);
            Log.Debug(ret.ToJson());
            Assert.True(ret != null);
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        [Fact]
        public void RunAsync()
        {
            string file = Cache.runPath + "script\\test.py";
            string fun = "Main";
            // var bl = eng.Load(file, fun);
            object param1 = 1;
            object param2 = 2;
            object param3 = 3;

            eng.RunAsync(file, fun, param1, param2, param3);
            Thread.Sleep(5000);
            Log.Debug(Cache.res.ToJson());
            Assert.True(Cache.res.Count > 0);
        }

        /// <summary>
        /// 执行脚本文件
        /// </summary>
        /// <returns>返回执行结果</returns>
        [Fact]
        private void RunFile()
        {
            string file = Cache.runPath  + "script\\test.py";
            string fun = "Main";
            object param1 = 1;
            object param2 = 2;
            object param3 = 3;

            var ret = eng.RunFile(file, fun, param1, param2, param3);
            Log.Debug(eng.GetEx());
            Assert.True(ret != null);
            Log.Debug(ret.ToString());
        }

        /// <summary>
        /// 执行脚本代码
        /// </summary>
        [Fact]
        private void RunCode()
        {
            string code = @"
def Main(param1 = None, param2 = None, param3 = None):
    return 'test'";
            string fun = "Main";
            object param1 = 1;
            object param2 = 2;
            object param3 = 3;

            var ret = eng.RunCode(code, fun, param1, param2, param3);
            Log.Debug(eng.GetEx());
            Log.Debug(ret.ToString());
            Assert.True(ret != null);
        }
    }
}
