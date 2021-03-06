﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace MM.Engine
{
    /// <summary>
    /// 缓存
    /// </summary>
    public class Cache
    {
        /// <summary>
        /// 运行路径
        /// </summary>
        public static string runPath = Directory.GetCurrentDirectory() + "\\";
        /// <summary>
        /// 运行路径
        /// </summary>
        public string RunPath { get { return runPath; } set { runPath = value; _Path = new PathModel(runPath);} }

        /// <summary>
        /// 路径模型
        /// </summary>
        internal static PathModel _Path = new PathModel(runPath);
        /// <summary>
        /// 路径模型
        /// </summary>
        public PathModel Path { get { return _Path; }}

        /// <summary>
        /// 请求参数
        /// </summary>
        public static ConcurrentDictionary<string, object> req = new ConcurrentDictionary<string, object>();
        /// <summary>
        /// 请求参数
        /// </summary>
        public ConcurrentDictionary<string, object> Req { get { return req; } set { req = value; } }

        /// <summary>
        /// 响应结果
        /// </summary>
        public static ConcurrentDictionary<string, object> res = new ConcurrentDictionary<string, object>();
        /// <summary>
        /// 响应结果
        /// </summary>
        public ConcurrentDictionary<string, object> Res { get { return res; } set { res = value; } }

        /// <summary>
        /// 模板主题风格
        /// </summary>
        internal static string _Theme = "default";
        /// <summary>
        /// 模板主题风格
        /// </summary>
        public string Theme { get { return _Theme; } set { if (!string.IsNullOrEmpty(value)) { _Theme = value; } } }

        /// <summary>
        /// 转为文件全名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="dir">当前路径</param>
        /// <returns>返回文件全名</returns>
        internal static string ToFullName(string fileName, string dir = "")
        {
            var file = fileName.Replace("/", "\\");
            if (file.StartsWith(@".\"))
            {
                file = dir + file.Substring(2);
            }
            else if (file.StartsWith(@"..\"))
            {
                file = dir + @"..\" + file;
            }
            else if (file.StartsWith(@"\"))
            {
                file = _Path.Web + file.Substring(1);
            }
            else if (file.StartsWith(@"~\"))
            {
                file = _Path.Template + _Theme + "\\" + file.Substring(2);
            }
            return file.ToLower();
        }

        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="tag">标签</param>
        /// <returns>返回请求参数</returns>
        public object GetReq(string tag)
        {
            req.TryGetValue(tag, out object reqM);
            return reqM;
        }

        /// <summary>
        /// 设置请求参数
        /// </summary>
        /// <param name="tag">标签</param>
        /// <param name="reqM">请求参数</param>
        /// <returns>设置成功返回true，是失败返回false</returns>
        public bool SetReq(string tag, object reqM)
        {
            return req.AddOrUpdate(tag, reqM, (key, value) => reqM) != null;
        }

        /// <summary>
        /// 删除请求参数
        /// </summary>
        /// <param name="tag">标签</param>
        /// <returns>返回请求参数</returns>
        public object DelReq(string tag)
        {
            return req.TryRemove(tag, out object reqM);
        }

        /// <summary>
        /// 获取响应结果
        /// </summary>
        /// <param name="tag">标签</param>
        /// <returns>返回响应结果</returns>
        public object GetRes(string tag)
        {
            res.TryGetValue(tag, out object resM);
            return resM;
        }

        /// <summary>
        /// 设置响应结果
        /// </summary>
        /// <param name="tag">标签</param>
        /// <param name="resM">响应结果</param>
        /// <returns>设置成功返回true，是失败返回false</returns>
        public bool SetRes(string tag, object resM)
        {
            return res.AddOrUpdate(tag, resM, (key, value) => resM) != null;
        }

        /// <summary>
        /// 删除响应结果
        /// </summary>
        /// <param name="tag">标签</param>
        /// <returns>返回响应结果</returns>
        public object DelRes(string tag)
        {
            return res.TryRemove(tag, out object resM);
        }
    }
}
