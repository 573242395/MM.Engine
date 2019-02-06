using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MM.Engine
{
    /// <summary>
    /// Razor模板引擎帮助类
    /// </summary>
    public class TPL
    {
        /// <summary>
        /// 缓存驱动
        /// </summary>
        private static MmCachingProvider cache = new MmCachingProvider();
        private static FileSystemWatcher watcher = new FileSystemWatcher();
        private static MmTemplateManager mg;

        private static readonly Regex layoutEx = new Regex("Layout\\s*=\\s*@?\"(\\S*)\";");//匹配视图中的layout

        /// <summary>
        /// 主题
        /// </summary>
        public static string theme = "default";

        /// <summary>
        /// 主题
        /// </summary>
        public string Theme
        {
            get { return theme; }
            set { theme = value; }
        }

        /// <summary>
        /// 视图模型
        /// </summary>
        public DynamicViewBag ViewBag { get; set; } = new DynamicViewBag();

        /// <summary>
        /// 模板路径
        /// </summary>
        public string Dir { get; set; } = Cache._Path.Template;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Ex { get; private set; } = string.Empty;

        private static IRazorEngineService razor = RazorEngine.Engine.Razor;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            SetManager();
            //添加文件修改监控，以便在cshtml文件修改时重新编译该文件
            watcher.Path = Cache._Path.Web;
            watcher.IncludeSubdirectories = true;
            watcher.Filter = "*.*html";
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;

            var config = new TemplateServiceConfiguration()
            {
                Namespaces = new HashSet<string>() { "Microsoft.CSharp", "MM.Engine" }, // 引入命名空间
                Language = Language.CSharp,
                EncodedStringFactory = new HtmlEncodedStringFactory(),
                DisableTempFileLocking = true,
                TemplateManager = mg,
                BaseTemplateType = typeof(MmTemplateBase<>),
                CachingProvider = cache
            };
            cache.InvalidateAll();
            razor = RazorEngineService.Create(config);
        }

        private void SetManager()
        {
            mg = new MmTemplateManager(InFunc);
        }

        private string InFunc(string arg)
        {
            return Load(arg);
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            string key = e.FullPath.ToLower();
            cache.InvalidateCache(key);
        }

        /// <summary>
        /// 新建视图背包
        /// </summary>
        public DynamicViewBag NewViewBag()
        {
            return new DynamicViewBag();
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearCache()
        {
            cache.InvalidateAll();
        }

        /// <summary>
        /// 新建字典
        /// </summary>
        /// <returns>返回新字典</returns>
        public Dictionary<string, object> NewDt()
        {
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// 新建字典
        /// </summary>
        /// <returns>返回新字典</returns>
        public void Add(string key, string value)
        {
            ViewBag.AddValue(key, value);
        }

        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="dt">字典类型</param>
        public void AddDictionary(Dictionary<string, object> dt)
        {
            ViewBag.AddDictionary(dt);
        }

        /// <summary>
        /// 获取所有成员名称
        /// </summary>
        public List<string> GetNames()
        {
            var arr = ViewBag.GetDynamicMemberNames();
            if (arr != null)
            {
                return arr.ToList();
            }
            return new List<string>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dir">模板路径</param>
        public TPL(string dir = "")
        {
            Dir = dir;
        }

        #region 文本渲染,适用于客户端
        /// <summary>
        /// 预编译模板
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="template">模板内容</param>
        public void Compile(string key, string template)
        {
            razor.Compile(template, key);
        }

        /// <summary>
        /// 渲染数据
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="template">模板</param>
        /// <param name="model">数据模型</param>
        /// <param name="tp">模板类型</param>
        /// <returns>返回渲染后的字符串</returns>
        public string Format(string key, string template, object model = null, Type tp = null)
        {
            var ret = "";
            try
            {
                var keyString = key.Replace(Cache.runPath, "");
                ITemplateKey km = razor.GetKey(keyString);
                var bl = razor.IsTemplateCached(km, tp);
                if (bl)
                {
                    ret = razor.Run(km, tp, model, ViewBag);
                }
                else
                {
                    ret = razor.RunCompile(template, km, tp, model, ViewBag);
                }
            }
            catch (Exception ex)
            {
                Ex = ex.ToString();
            }
            return ret;
        }

        /// <summary>
        /// 渲染数据
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="template">模板</param>
        /// <param name="model">数据模型</param>
        public string FormatS(string key, LoadedTemplateSource template, object model)
        {
            var ret = "";
            try
            {
                var km = razor.GetKey(key);
                Type tp = null;
                var bl = razor.IsTemplateCached(km, tp);
                if (bl)
                {
                    ret = razor.Run(km, tp, model, ViewBag);
                }
                else
                {
                    ret = razor.RunCompile(template, km, tp, model, ViewBag);
                }
            }
            catch (Exception ex)
            {
                Ex = ex.ToString();
            }
            return ret;
        }

        /// <summary>
        /// 加载模板资源
        /// </summary>
        /// <param name="template">模板</param>
        /// <param name="templateFile">模板文件</param>
        /// <returns>返回模板资源模型</returns>
        public LoadedTemplateSource LoadTpl(string template, string templateFile = null)
        {
            return new LoadedTemplateSource(template, templateFile);
        }

        /// <summary>
        /// 转换路径
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="dir">当前路径</param>
        /// <returns>返回完整物理路径</returns>
        public string ToFullName(string file, string dir = "")
        {
            if (file == null)
            {
                return "";
            }
            file = file.Replace("/", @"\").Replace(@"\\", @"\");
            if (file.StartsWith(@"\"))
            {
                file = Cache._Path.Web + file.Substring(1);
            }
            else if (file.StartsWith(@"~\"))
            {
                var p = Cache._Path.Template;
                if (!string.IsNullOrEmpty(Theme))
                {
                    p += Theme + "\\";
                }
                file = p + file.Substring(2);
            }
            else
            {
                if (string.IsNullOrEmpty(dir))
                {
                    dir = Dir;
                }
                if (!string.IsNullOrEmpty(dir))
                {
                    if (!dir.EndsWith(@"\"))
                    {
                        dir += @"\";
                    }
                    if (file.StartsWith(@".\"))
                    {
                        file = dir + file.Substring(2);
                    }
                    else if (file.StartsWith(@"..\"))
                    {
                        file = dir + file.Substring(3);
                    }
                    else
                    {
                        file = Cache._Path.Template + file;
                    }
                }
            }
            return file.ToLower();
        }

        /// <summary>
        /// 模板渲染
        /// </summary>
        /// <param name="file">模板文件</param>
        /// <returns>返回渲染后文本</returns>
        public string Load(string file)
        {
            var fe = ToFullName(file);
            if (File.Exists(fe))
            {
                return File.ReadAllText(fe, System.Text.Encoding.UTF8);
            }
            Ex = "模板文件不存在";
            return "";
        }

        /// <summary>
        /// 模板渲染
        /// </summary>
        /// <param name="file">文件全名</param>
        /// <param name="model">模型</param>
        /// <returns>返回渲染后文本</returns>
        public string View(string file, object model = null)
        {
            file = ToFullName(file);
            var text = Load(file);
            if (!string.IsNullOrEmpty(text))
            {
                return Format(file, text, model);
            }
            return "";
        }
        #endregion

        /// <summary>
        /// 释放模板帮助类
        /// </summary>
        public void Dispose()
        {
            cache.Dispose();
            mg.Dispose();
        }
    }
}
