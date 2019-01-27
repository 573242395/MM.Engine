using System.Collections.Generic;
using System.IO;

namespace MM.Engine
{
    public class Cache
    {
        /// <summary>
        /// 运行路径
        /// </summary>
        public static string runPath = Directory.GetCurrentDirectory() + "\\";
        /// <summary>
        /// 运行路径
        /// </summary>
        public string RunPath { get { return runPath; } set { runPath = value; } }

        /// <summary>
        /// 请求参数
        /// </summary>
        public static Dictionary<string, object> req = new Dictionary<string, object>();
        /// <summary>
        /// 请求参数
        /// </summary>
        public Dictionary<string, object> Req { get { return req; } set { req = value; } }

        /// <summary>
        /// 响应结果
        /// </summary>
        public static Dictionary<string, object> res = new Dictionary<string, object>();
        /// <summary>
        /// 响应结果
        /// </summary>
        public Dictionary<string, object> Res { get { return res; } set { res = value; } }

        /// <summary>
        /// 转为文件全名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="dir">当前路径</param>
        /// <returns>返回文件全名</returns>
        internal static string ToFullName(string fileName, string dir = "")
        {
            var file = fileName.Replace("/", "\\");
            if (file.StartsWith(".\\"))
            {
                file = dir + file.Substring(2);
            }
            else if (file.StartsWith("..\\"))
            {
                file = dir + file;
            }
            else if (file.StartsWith("\\"))
            {
                file = runPath + file.Substring(1);
            }
            return file;
        }
    }
}
