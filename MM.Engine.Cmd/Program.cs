using System;
using System.Collections.Generic;
using System.IO;

namespace MM.Engine.Cmd
{
    class Program
    {
        public static LUA eng = new LUA();

        public static TPL tpl = new TPL();

        static void Main(string[] args)
        {
            Cache.runPath = Directory.GetCurrentDirectory() + "\\";
            //EachLoad();
            //Unload();
            //Load();
            //Run();
            //RunFile();
            //RunCode();
            TPL.Init();
            View();
            Console.ReadLine();
        }

        /// <summary>
        /// Razor模板引擎渲染
        /// </summary>
        public static void View() {
            tpl.Dir = Cache.runPath;
            var model = new { Version = 1.0, Desc = "这是使用Razor模板引擎的视图渲染", Role = new { Name = "小白", Age = 18, Sex = true } };
            var ret = tpl.View("./wwwroot/template/default/test.cshtml", model);
            Console.WriteLine("第一次输出：" + ret);
            ret = tpl.View("~/test.cshtml", model);
            Console.WriteLine("第二次输出：" + ret);
            Console.WriteLine("Razor模板引擎渲染结果：" + (ret != null));
            Console.WriteLine(tpl.Ex);
        }

        /// <summary>
        /// 遍历加载
        /// </summary>
        public static void EachLoad()
        {
            List<string> appList = new List<string>() { Cache.runPath + "script\\test.lua", Cache.runPath + "script\\lua\\test1.lua" };
            var bl = eng.EachLoad(appList);
            Console.WriteLine("EachLoad结果：" + bl);
        }

        /// <summary>
        /// 载入脚本
        /// </summary>
        public static void Load()
        {
            string file = Cache.runPath + "script\\lua\\test1.lua";

            var bl = eng.Load(file);
            Console.WriteLine("Load结果：" + bl);
        }

        /// <summary>
        /// 卸载脚本
        /// </summary>
        public static void Unload()
        {
            string file = Cache.runPath + "script\\lua\\test1.lua";
            eng.Load(file);
            string appName = file;
            var count = eng.Dict.Count;
            var bl = eng.Unload(appName);
            Console.WriteLine(count + ">" + eng.Dict.Count);
            Console.WriteLine("Unload结果：" + (count > eng.Dict.Count));
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        public static void Run()
        {
            string file = Cache.runPath + "script\\lua\\test1.lua";
            string fun = "Main";
            //var bl = eng.Load(file, fun);
            int param1 = 1;
            int param2 = 2;
            int param3 = 3;

            var ret = eng.Run(file, fun, param1, param2, param3);
            Console.WriteLine(ret);
            Console.WriteLine(eng.Ex);
            ret = eng.Run(file, fun, param1, param2, param3);
            Console.WriteLine(ret);
            Console.WriteLine(eng.Ex);
            Console.WriteLine("Run结果：" + (ret != null));
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        public static void RunAsync()
        {
            string file = Cache.runPath + "script\\test.lua";
            string fun = "Main";
            // var bl = eng.Load(file, fun);
            int param1 = 1;
            int param2 = 2;
            int param3 = 3;

            eng.RunAsync(file, fun, param1, param2, param3);
            Console.WriteLine(Cache.res.ToJson());
            Console.WriteLine("Unload结果：" + (Cache.res.Count > 0));
        }

        /// <summary>
        /// 执行脚本文件
        /// </summary>
        /// <returns>返回执行结果</returns>
        public static void RunFile()
        {
            string file = Cache.runPath + "script\\test.lua";
            string fun = "Main";
            int param1 = 1;
            int param2 = 2;
            int param3 = 3;

            var ret = eng.RunFile(file, fun, param1, param2, param3);
            Console.WriteLine(eng.GetEx());
            Console.WriteLine(ret.ToString());
            ret = eng.RunFile(file, fun, param1, param2, param3);
            Console.WriteLine(eng.GetEx());
            Console.WriteLine(ret.ToString());

            Console.WriteLine("RunFile结果：" + (ret != null));
        }

        /// <summary>
        /// 执行脚本代码
        /// </summary>
        public static void RunCode()
        {
            string code = @"
function Main(fun, param1, param2, param3)
	local ret = param1 + param2 + param3
	return ret
end";
            string fun = "Main";
            int param1 = 1;
            int param2 = 2;
            int param3 = 3;

            var ret = eng.RunCode(code, fun, param1, param2, param3);
            Console.WriteLine(eng.GetEx());
            Console.WriteLine(ret.ToString());
            Console.WriteLine("RunCode结果：" + (ret != null));
        }
    }
}
