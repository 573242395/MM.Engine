using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace MM.Engine.Test
{
    /// <summary>
    /// c#脚本引擎测试
    /// </summary>
    public class CsTest
    {
        public IEngine eng = new CS();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="output">输出接口</param>
        public CsTest(ITestOutputHelper output)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.TestOutput(output, Serilog.Events.LogEventLevel.Verbose)
                .CreateLogger();
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        [Fact]
        private void GetEx()
        {
            var ex = eng.GetEx();
            Log.Debug(ex.ToString());
            Assert.True(!string.IsNullOrEmpty(ex));
        }

        /// <summary>
        /// 遍历加载
        /// </summary>
        [Fact]
        private void EachLoad()
        {
            List<string> appList = new List<string>() { "" };

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
            string appName = "";
            string file = "";

            var bl = eng.Load(appName, file);
            Log.Debug(bl.ToString());
            Assert.True(bl);
        }

        /// <summary>
        /// 卸载脚本
        /// </summary>
        [Fact]
        private void Unload()
        {
            string appName = "";
            var bl = eng.Unload(appName);

            Log.Debug(bl.ToString());
            Assert.True(bl);
        }

        /// <summary>
        /// 执行脚本——无返回结果
        /// </summary>
        [Fact]
        private void Execute()
        {
            string appName = "";
            object param1 = null;
            object param2 = null;
            object param3 = null;
            object param4 = null;

            eng.Execute(appName, param1, param2, param3, param4);
            Log.Debug(Cache.res.ToJson());
            Assert.True(Cache.res.Count > 0);
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        [Fact]
        public void Run()
        {
            string appName = "";
            object param1 = null;
            object param2 = null;
            object param3 = null;
            object param4 = null;

            var ret = eng.Run(appName, param1, param2, param3, param4);
            Log.Debug(ret.ToJson());
            Assert.True(ret != null);
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        [Fact]
        public void RunAsync()
        {
            string appName = "";
            object param1 = null;
            object param2 = null;
            object param3 = null;
            object param4 = null;

            eng.RunAsync(appName, param1, param2, param3, param4);
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
            string file = "";
            object param1 = null;
            object param2 = null;
            object param3 = null;
            object param4 = null;

            var ret = eng.RunFile(file, param1, param2, param3, param4);
            Log.Debug(ret.ToString());
            Assert.True(ret != null);
        }

        /// <summary>
        /// 执行脚本代码
        /// </summary>
        [Fact]
        private void RunCode()
        {
            string code = "";
            object param1 = null;
            object param2 = null;
            object param3 = null;
            object param4 = null;

            var ret = eng.RunCode(code, param1, param2, param3, param4);
            Log.Debug(ret.ToString());
            Assert.True(ret != null);
        }
    }
}
