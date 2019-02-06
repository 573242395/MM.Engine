using RazorEngine.Templating;
using System;

namespace MM.Engine
{
    /// <summary>
    /// 基本模板类型
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public abstract class MmTemplateBase<T> : TemplateBase<T>
    {
        /// <summary>
        /// 主题风格
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// 当前文件路径
        /// </summary>
        public string Dir { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MmTemplateBase()
        {

        }

        /// <summary>
        /// 引用视图
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>返回视图</returns>
        public TemplateWriter View(string name)
        {
            var file = ToFullName(name);
            return Include(file);
        }

        /// <summary>
        /// 引用视图
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="model">模板</param>
        /// <param name="tp">模板类型</param>
        /// <returns>返回视图</returns>
        public TemplateWriter View(string name, object model, Type tp = null)
        {
            var file = ToFullName(name);
            return Include(file, model, tp);
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
    }
}