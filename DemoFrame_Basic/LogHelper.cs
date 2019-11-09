using System;
using System.Collections.Generic;
using System.Text;

namespace DemoFrame_Basic
{
    /// <summary>
    /// 日志记录类；
    /// 严重级别从小到大：Trace、Debug、Info、Warn、Error、Fatal
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// NLog的实例对象
        /// </summary>
        public static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    }
}
