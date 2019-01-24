using System.Collections.Generic;
using System.IO;

namespace MM.Engine
{
    public class Cache
    {
        /// <summary>
        /// 运行路径
        /// </summary>
        public static string runPath = Directory.GetCurrentDirectory();
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
    }
}
