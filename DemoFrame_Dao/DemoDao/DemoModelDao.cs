using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_CoreMvc.Models;
using DemoFrame_Dao.BaseDaoFile;
using DemoFrame_Models.CusEntitys;
using DemoFrame_Models.Entitys;

namespace DemoFrame_Dao.DemoDao
{
    public class DemoModelDao : BaseDao<DemoModel>
    {
        private static object locker = new object();
        private static DemoModelDao _demoModelDao;

        public static DemoModelDao Instance
        {
            get
            {
                if (_demoModelDao != null) return _demoModelDao;
                lock (locker)
                {
                    if (_demoModelDao == null)
                    {
                        _demoModelDao = new DemoModelDao();
                    }
                }
                return _demoModelDao;
            }
        }

        /// <summary>
        /// 得到分页数据
        /// </summary>
        /// <param name="queryDemo"></param>
        /// <returns></returns>
        public PageResponse<DemoModel> DemoPageResponse(QueryDemoDto queryDemo)
        {
            var date = LoadPageEntities(queryDemo.Page, queryDemo.PageSize, 
                c => c.CustomerName.Contains(queryDemo.Name), false, c => c.Id);
            return date;
        }
    }
}
