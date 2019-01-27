using System.Collections.Generic;
using System.Threading.Tasks;

namespace MM.Engine
{
    /// <summary>
    /// 脚本引擎接口
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <returns>返回错误信息</returns>
        string GetEx();

        /// <summary>
        /// 遍历加载
        /// </summary>
        /// <param name="appList">应用列表</param>
        /// <returns>加载成功返回true，失败返回false</returns>
        bool EachLoad(List<string> appList);

        /// <summary>
        /// 载入脚本
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="funName">函数名</param>
        /// <returns>载入成功返回true，失败返回false</returns>
        bool Load(string file, string funName = "Main");

        /// <summary>
        /// 卸载脚本
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <returns>卸载成功返回true，失败返回false</returns>
        bool Unload(string appName);

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="file">应用名</param>
        /// <param name="funName">函数名</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <returns>返回执行结果</returns>
        object Run(string file, string funName, object param1 = null, object param2 = null, object param3 = null);

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="file">应用名</param>
        /// <param name="funName">函数名</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <returns>返回执行结果</returns>
        Task RunAsync(string file, string funName, object param1 = null, object param2 = null, object param3 = null);

        /// <summary>
        /// 执行脚本文件
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="funName">函数名</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <returns>返回执行结果</returns>
        object RunFile(string file, string funName, object param1 = null, object param2 = null, object param3 = null);

        /// <summary>
        /// 执行脚本代码
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="funName">函数名</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <returns>返回执行结果</returns>
        object RunCode(string code, string funName, object param1 = null, object param2 = null, object param3 = null);

        /// <summary>
        /// 获取函数 
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="fun">函数名</param>
        /// <returns>返回函数</returns>
        dynamic GetFun(string file, string fun);
    }
}
