using System;
using System.Collections.Generic;
using System.Text;
using DemoFrame_Dao.BaseDaoFile;
using DemoFrame_Models.Entitys;

namespace DemoFrame_Dao.DemoDao
{
    public class TestModelDao : BaseDao<TestModel>
    {
        private static object locker = new object();
        private static TestModelDao _testModelDao;

        public static TestModelDao Instance
        {
            get
            {
                if (_testModelDao != null) return _testModelDao;
                lock (locker)
                {
                    if (_testModelDao == null)
                    {
                        _testModelDao = new TestModelDao();
                    }
                }
                return _testModelDao;
            }
        }
    }
}
