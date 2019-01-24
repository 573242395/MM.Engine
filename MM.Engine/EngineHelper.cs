namespace MM.Engine
{
    public class EngineHelper
    {
        /// <summary>
        /// 当前目录
        /// </summary>
        public string Dir { get; set; }

        /// <summary>
        /// 新建Csharp脚本引擎帮助类
        /// </summary>
        /// <returns>返回帮助类</returns>
        public CS NewCS() {
            return new CS(Dir);
        }

        /// <summary>
        /// 新建VB脚本引擎帮助类
        /// </summary>
        /// <returns>返回帮助类</returns>
        public VB NewVB()
        {
            return new VB(Dir);
        }

        /// <summary>
        /// 新建python脚本引擎帮助类
        /// </summary>
        /// <returns>返回帮助类</returns>
        public PY NewPY()
        {
            return new PY(Dir);
        }

        /// <summary>
        /// 新建Csharp脚本引擎帮助类
        /// </summary>
        /// <returns>返回帮助类</returns>
        public JS NewJS()
        {
            return new JS(Dir);
        }
    }
}
