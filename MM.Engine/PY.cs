using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MM.Engine
{
    /// <summary>
    /// Python脚本引擎
    /// </summary>
    public class PY : IEngine
    {
        private static ScriptEngine Eng;
        private readonly string _Dir;

        #region 属性
        /// <summary>
        /// 脚本引擎帮助类
        /// </summary>
        public EngineHelper Engine { get; set; } = new EngineHelper();

        /// <summary>
        /// 错误提示
        /// </summary>
        public string Ex           { get; set; }

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
        public PY(string dir = null)
        {
            if (string.IsNullOrEmpty(dir))
            {
                _Dir = Cache.runPath;
            }
            else
            {
                _Dir = dir;
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
        /// <param name="files">文件列表</param>
        /// <returns>加载成功返回true，失败返回false</returns>
        public bool EachLoad(List<string> files)
        {
            var bl = true;
            foreach (var file in files)
            {
                bl = Load(file);
                if (!bl)
                {
                    break;
                }
            }
            return bl;
        }

        /// <summary>
        /// 载入脚本
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="fun">函数名</param>
        /// <returns>载入成功返回true，失败返回false</returns>
        public bool Load(string file, string fun = "Main")
        {
            var bl = false;
            file = Cache.ToFullName(file, _Dir);
            var key = file.Replace(Cache.runPath, "") + ":" + fun;
            var funObj = GetFun(file, fun);
            //var obj = (object)funObj("1", "2", "3");
            //Debug.WriteLine(obj);
            if (funObj != null)
            {
                if (dict.ContainsKey(key))
                {
                    dict[key] = funObj;
                    bl = true;
                }
                else
                {
                    bl = dict.TryAdd(key, funObj);
                }
            }
            return bl;
        }

        /// <summary>
        /// 卸载脚本
        /// </summary>
        /// <param name="appName">应用名，由文件名 + 函数名组成</param>
        /// <returns>卸载成功返回true，失败返回false</returns>
        public bool Unload(string appName)
        {
            if (!string.IsNullOrEmpty(appName))
            {
                return dict.TryRemove(appName, out var value);
            }
            return false;
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="appName">应用名，由文件名 + 函数名组成</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <returns>返回执行结果</returns>
        public object Run(string appName, object param1 = null, object param2 = null, object param3 = null)
        {
            if (dict.TryGetValue(appName, out dynamic funObj))
            {
                if (funObj != null)
                {
                    if (param1 == null)
                    {
                        return funObj();
                    }
                    else if (param2 == null)
                    {
                        return funObj(param1);
                    }
                    else if (param3 == null)
                    {
                        return funObj(param1, param2);
                    }
                    else
                    {
                        return funObj(param1, param2, param3);
                    }
                }
            }
            else
            {
                Ex = "程序未加载！";
            }
            return null;
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="fun">函数名</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <returns>返回执行结果</returns>
        public object Run(string file, string fun, object param1 = null, object param2 = null, object param3 = null)
        {
            var appName = file.Replace(Cache.runPath, "") + ":" + fun;
            if (!dict.ContainsKey(appName))
            {
                var bl = Load(file, fun);
                if (!bl)
                {
                    return null;
                }
            }
            return Run(appName, param1, param2, param3);
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="file">应用名</param>
        /// <param name="fun">函数名</param>
        /// <param name="param1">参数2</param>
        /// <param name="param2">参数3</param>
        /// <param name="param3">参数4</param>
        /// <returns>返回执行结果</returns>
        public Task RunAsync(string file, string fun = null, object param1 = null, object param2 = null, object param3 = null)
        {
            return Task.Run(() => Run(file, fun, param1, param2, param3));
        }

        /// <summary>
        /// 执行脚本文件
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="fun">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <param name="param4">参数4</param>
        /// <returns>返回执行结果</returns>
        public object RunFile(string file, string fun = null, object param1 = null, object param2 = null, object param3 = null)
        {
            if (string.IsNullOrEmpty(file))
            {
                Ex = "脚本文件名不能为空";
                return null;
            }
            file = Cache.ToFullName(file, _Dir);
            if (!File.Exists(file))
            {
                Ex = "脚本不存在！请确认脚本：“" + file + "”是否存在。";
                return null;
            }
            try
            {
                var eng = NewEngine();
                var scope = eng.CreateScope();
                scope.SetVariable("Cache", new Cache());
                Engine.Dir = Path.GetDirectoryName(file) + "\\";
                scope.SetVariable("Engine", Engine);
                var source = eng.CreateScriptSourceFromFile(file);
                if (source != null)
                {
                    var compiled = source.Execute(scope);
                    if (scope.TryGetVariable(fun, out dynamic funObj))
                    {
                        if (param1 == null)
                        {
                            return funObj();
                        }
                        else if (param2 == null)
                        {
                            return funObj(param1);
                        }
                        else if (param3 == null)
                        {
                            return funObj(param1, param2);
                        }
                        else {
                            return funObj(param1, param2, param3);
                        }
                    }
                }
                else
                {
                    Ex = "引用文件错误";
                }
            }
            catch (Exception ex)
            {
                Ex = ex.Message;
            }
            return null;
        }

        /// <summary>
        /// 执行脚本代码
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="fun">函数名</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <returns>返回执行结果</returns>
        public object RunCode(string code, string fun = null, object param1 = null, object param2 = null, object param3 = null)
        {
            if (string.IsNullOrEmpty(code))
            {
                Ex = "脚本不能为空";
                return null;
            }
            try
            {
                var scope = Eng.CreateScope();
                scope.SetVariable("Cache", new Cache());
                Engine.Dir = _Dir;
                scope.SetVariable("Engine", Engine);
                var source = Eng.CreateScriptSourceFromString(code);
                var compiled = source.Execute(scope);
                if (scope.TryGetVariable(fun, out dynamic funObj))
                {
                    if (param1 == null)
                    {
                        return funObj();
                    }
                    else if (param2 == null)
                    {
                        return funObj(param1);
                    }
                    else if (param3 == null)
                    {
                        return funObj(param1, param2);
                    }
                    else
                    {
                        return funObj(param1, param2, param3);
                    }
                }
            }
            catch (Exception ex)
            {
                Ex = ex.Message;
            }
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
            if (string.IsNullOrEmpty(file))
            {
                Ex = "脚本文件名不能为空";
                return null;
            }
            file = Cache.ToFullName(file, _Dir);
            if (!File.Exists(file))
            {
                Ex = "脚本不存在！请确认脚本：“" + file + "”是否存在。";
                return null;
            }
            try
            {
                var scope = Eng.CreateScope();
                scope.SetVariable("Cache", new Cache());
                Engine.Dir = Path.GetDirectoryName(file) + "\\";
                scope.SetVariable("Engine", Engine);
                var source = Eng.CreateScriptSourceFromFile(file);
                if (source != null)
                {
                    var compiled = source.Compile().Execute(scope);
                    if (scope.TryGetVariable(fun, out dynamic funObj))
                    {
                        return funObj;
                    }
                }
                else
                {
                    Ex = "引用文件错误";
                }
            }
            catch (Exception ex)
            {
                Ex = ex.Message;
            }
            return null;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init() {
            Eng = NewEngine();
        }

        /// <summary>
        /// 新建脚本引擎
        /// </summary>
        /// <returns>返回脚本引擎类</returns>
        public static ScriptEngine NewEngine()
        {
            var runtime = Python.CreateRuntime();
            runtime.IO.RedirectToConsole();
            runtime.LoadAssembly(Assembly.GetExecutingAssembly());
            var engine = runtime.GetEngine("python");
            engine.SetSearchPaths(new string[] { ".", Cache.runPath + "Lib", Cache.runPath + "DLLs" }); //设置函数库搜索路径
            return engine;
        }
    }
}
