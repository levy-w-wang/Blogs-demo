using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_Basic;

namespace DemoFrame_Dao.BaseDaoFile
{
    /// <summary>
    /// 数据库工厂
    /// </summary>
    public class DbContextFactory
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        /// <returns></returns>
        public static DemoDbContext GetCurrentDbContext()
        {
            if (DemoWeb.HttpContext.Items["DbContext"] is DemoDbContext dbContext) return dbContext;
            dbContext = DemoWeb.IocManager.Resolve<DemoDbContext>();//从容器中得到数据库上下文 放置在 Items 中， 访问结束自动销毁
            //dbContext = DemoWeb.HttpContext.RequestServices.GetService(typeof(DemoDbContext)) as DemoDbContext;
            DemoWeb.HttpContext.Items["DbContext"] = dbContext;
            return dbContext;
        }
        /// <summary>
        /// 释放DBContext对象
        /// </summary>
        public static void DisposeDbContext()
        {
            if (DemoWeb.HttpContext.Items.ContainsKey("DbContext"))
            {
                DemoWeb.HttpContext.Items.Remove("DbContext");
            }
        }
    }
}
