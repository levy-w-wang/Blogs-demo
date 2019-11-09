using System;
using DemoFrame_Basic;
using DemoFrame_Basic.Dependency;
using DemoFrame_CoreMvc;
using DemoFrame_Models.Entitys;
using Microsoft.EntityFrameworkCore;

namespace DemoFrame_Dao
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options)
     : base(options)
        { }

        #region IOC得到所有实体
        private readonly IEntityBase[] _entitys = DemoWeb.IocManager.ResolveAll<IEntityBase>();
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (_entitys == null)
            {
                return;
            }
            foreach (var entity in _entitys)
            {
                modelBuilder.Model.AddEntityType(entity.GetType());
            }
        }
    }
}
