using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_CoreMvc.Models;
using DemoFrame_Dao.DemoDao;
using DemoFrame_Models.CusEntitys;
using DemoFrame_Models.Entitys;

namespace DemoFrame_Biz
{
    public class DemoModelBiz
    {
        private static object locker = new object();
        private static DemoModelBiz _demoModelBiz;

        public static DemoModelBiz Instance
        {
            get
            {
                if (_demoModelBiz != null) return _demoModelBiz;
                lock (locker)
                {
                    if (_demoModelBiz == null)
                    {
                        _demoModelBiz = new DemoModelBiz();
                    }
                }
                return _demoModelBiz;
            }
        }

        public string AddDemoModel(DemoModel demoModel)
        {
            DemoModelDao.Instance.Add(demoModel);
            var count = DemoModelDao.Instance.SaveChanges();
            return count > 0 ? "success" : "save error";
        }
        public string AddDemoModel(List<DemoModel> demoModels)
        {
            DemoModelDao.Instance.Add(demoModels);
            DemoModelDao.Instance.Delete(c=>c.Id == 1);
            DemoModelDao.Instance.Delete(c=>c.CustomerName.StartsWith("2"));
            TestModelDao.Instance.Add(new TestModel()
            {
                BlogName = "NET CORE",
                BlogPhone = 123,
                BlogUseDay = 90
            });
            var count = DemoModelDao.Instance.SaveChanges();
            return count > 0 ? "success" : "save error";
        }
        /// <summary>
        /// 得到分页数据
        /// </summary>
        /// <param name="queryDemo"></param>
        /// <returns></returns>
        public PageResponse<DemoModel> DemoModelList(QueryDemoDto queryDemo)
        {
           return DemoModelDao.Instance.DemoPageResponse(queryDemo);
        }
    }
}
