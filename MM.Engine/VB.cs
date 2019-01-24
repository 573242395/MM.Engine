using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MM.Engine
{
    /// <summary>
    /// Vbs脚本引擎
    /// </summary>
    public class VB : IEngine
    {
        #region 属性
        /// <summary>
        /// 脚本引擎帮助类
        /// </summary>
        public EngineHelper Engine { get; set; } = new EngineHelper();

        /// <summary>
        /// 错误提示
        /// </summary>
        public string Ex { get; set; }

        /// <summary>
        /// 脚本函数字典
        /// </summary>
        internal static ConcurrentDictionary<string, dynamic> dict = new ConcurrentDictionary<string, dynamic>();
        /// <summary>
        /// 脚本函数字典
        /// </summary>
        public ConcurrentDictionary<string, dynamic> Dict
        {
            get { return dict; }
            set { dict = value; }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dir">脚本目录</param>
        public VB(string dir = null)
        {
            if (string.IsNullOrEmpty(dir))
            {
                Engine.Dir = Cache.runPath;
            }
            else
            {
                Engine.Dir = dir;
            }
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <returns>返回错误信息</returns>
        public string GetEx()
        {
            return Ex;
        }

        /// <summary>
        /// 遍历加载
        /// </summary>
        /// <param name="appList">应用列表</param>
        /// <returns>加载成功返回true，失败返回false</returns>
        public bool EachLoad(List<string> appList)
        {
            return false;
        }

        /// <summary>
        /// 载入脚本
        /// </summary>
        /// <param name="file">文件名</param>
        /// <returns>载入成功返回true，失败返回false</returns>
        public bool Load(string file)
        {
            return false;
        }

        /// <summary>
        /// 卸载脚本
        /// </summary>
        /// <param name="file">文件名</param>
        /// <returns>卸载成功返回true，失败返回false</returns>
        public bool Unload(string file)
        {
            if (!string.IsNullOrEmpty(file))
            {
                if (dict.ContainsKey(file))
                {
                    dict[file] = null;
                    return dict.TryRemove(file, out var value);
                }
            }
            return false;
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="appName">应用名</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="param4">参数4</param>
        /// <returns>返回执行结果</returns>
        public object Run(string appName, string funName = null, object param1 = null, object param2 = null, object param3 = null)
        {
            return null;
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="appName">应用名</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="param4">参数4</param>
        /// <returns>返回执行结果</returns>
        public void RunAsync(string appName, string funName = null, object param1 = null, object param2 = null, object param3 = null)
        {

        }

        /// <summary>
        /// 执行脚本文件
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="param4">参数4</param>
        /// <returns>返回执行结果</returns>
        public object RunFile(string file, string funName = null, object param1 = null, object param2 = null, object param3 = null)
        {
            return null;
        }

        /// <summary>
        /// 执行脚本代码
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="param4">参数4</param>
        /// <returns>返回执行结果</returns>
        public object RunCode(string code, string funName = null, object param1 = null, object param2 = null, object param3 = null)
        {
            return null;
        }

        /// <summary>
        /// 获取函数 
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="fun">函数名</param>
        /// <returns>返回函数</returns>
        public dynamic GetFun(string file, string fun = "Main")
        {
            return null;
        }
    }
}
