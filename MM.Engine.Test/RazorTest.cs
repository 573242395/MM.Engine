using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MM.Engine.Test
{
    /// <summary>
    /// Razor模板引擎测试
    /// </summary>
    [TestClass()]
    public class RazorTest
    {
        public static TPL tpl = new TPL();

        /// <summary>
        /// 构造函数
        /// </summary>
        public RazorTest() {
            TPL.Init();
        }

        /// <summary>
        /// 视图渲染
        /// </summary>
        [TestMethod()]
        public void View()
        {
            tpl.Dir = Cache.runPath; //设定当前目录
            var model = new { Version = 1.0, Desc = "这是使用Razor模板引擎的视图渲染", Role = new { Name = "小白", Age = 18, Sex = true } };
            var ret = tpl.View("./wwwroot/template/default/test.cshtml", model);
            Console.WriteLine("第一次输出：" + ret);
            ret = tpl.View("~/test.cshtml", model);
            Console.WriteLine("第二次输出：" + ret);
            Console.WriteLine(tpl.Ex);
            Console.WriteLine("Razor模板引擎渲染结果：" + (ret != null));
        }
    }
}
