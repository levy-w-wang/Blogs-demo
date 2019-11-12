using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using DemoFrame_Basic.Exceptions;
using DemoFrame_CoreMvc.Models;
using DemoFrame_Models.Entitys;
using Microsoft.EntityFrameworkCore;

namespace DemoFrame_Dao.BaseDaoFile
{
    /// <summary>
    /// 数据库基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseDao<T> : IBaseDao<T> where T : EntityBase, new()
    {
        
        public DemoDbContext DbContext => DbContextFactory.GetCurrentDbContext();

        public BaseDao()
        {
            //DbContext = DbContextFactory.GetCurrentDbContext();
        }

        #region 增删改的公共方法

        public T Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
            //DbContext.Entry(entity).State = EntityState.Added;
            return entity;
        }
        public List<T> Add(List<T> entitys)
        {
            DbContext.Set<T>().AddRange(entitys); //注释掉下面的快许多 且不影响保存
            //foreach (var model in entitys)
            //{
            //    DbContext.Entry(model).State = EntityState.Added;
            //}
            return entitys;
        }

        public void Delete(Expression<Func<T, bool>> whereFun)
        {
            IEnumerable<T> queryable = DbContext.Set<T>().Where(whereFun);
            //DbContext.Set<T>().RemoveRange(queryable);
            foreach (var model in queryable)
            {
                DbContext.Entry(model).State = EntityState.Deleted;
            }
        }

        public void Update(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Update(Expression<Func<T, bool>> @where, Dictionary<string, object> dic)
        {
            IEnumerable<T> queryable = DbContext.Set<T>().Where(@where).ToList();
            Type type = typeof(T);
            List<PropertyInfo> propertyList =
                type.GetProperties(BindingFlags.Public |
                                   BindingFlags.Instance).ToList();

            //遍历结果集
            foreach (T entity in queryable)
            {
                foreach (var propertyInfo in propertyList)
                {
                    string propertyName = propertyInfo.Name;
                    if (dic.ContainsKey(propertyName))
                    {
                        //设置值
                        propertyInfo.SetValue(entity, dic[propertyName], null);
                    }
                }

                Update(entity);
            }
        }

        public void Delete(params object[] keyValues)
        {
            var entity = DbContext.Set<T>().Find(keyValues);
            DbContext.Entry(entity).State = EntityState.Deleted;
        }
        public void Delete(object objectId)
        {
            var entity = DbContext.Set<T>().Find(objectId);
            DbContext.Entry(entity).State = EntityState.Deleted;
        }
        #endregion

        #region 查询方法

        /// <summary>
        /// 查看是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyLambda"></param>
        /// <returns></returns>
        public bool Exist(Expression<Func<T, bool>> anyLambda)
        {
            return DbContext.Set<T>().Any(anyLambda);
        }

        /// <summary>
        /// 根据主键得到数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public T Find(params object[] keyValues)
        {
            return DbContext.Set<T>().Find(keyValues);
        }

        /// <summary>
        /// 根据where条件查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> Where(Expression<Func<T, bool>> whereLambda)
        {
            return DbContext.Set<T>().Where(whereLambda);
        }
        /// <summary>
        /// 获取第一个或默认为空
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> whereLambda)
        {
            return DbContext.Set<T>().FirstOrDefault(whereLambda);
        }
        /// <summary>
        /// 得到条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="countLambda"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> countLambda)
        {
            return DbContext.Set<T>().AsNoTracking().Count(countLambda);
        }

        /// <summary>
        /// 获取第一个或默认的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="firstLambda"></param>
        /// <returns></returns>
        public T First(Expression<Func<T, bool>> firstLambda)
        {
            return DbContext.Set<T>().FirstOrDefault(firstLambda);
        }

        /// <summary>
        /// 得到IQueryable数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda = null)
        {
            if (whereLambda == null)
            {
                return DbContext.Set<T>().AsQueryable();
            }
            return DbContext.Set<T>().Where(whereLambda).AsQueryable();
        }

        /// <summary>
        /// 从某个表中获取分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="isAsc"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<T> LoadPageEntities<TKey>(int pageIndex, int pageSize, out int totalCount, out int pageCount, Expression<Func<T, bool>> whereLambda,
            bool isAsc, Expression<Func<T, TKey>> orderBy)
        {
            var temp = DbContext.Set<T>().AsNoTracking().Where(whereLambda); //去掉.AsQueryable().AsNoTracking()，将下面改为

            totalCount = temp.Count();
            pageCount = (int)Math.Ceiling((double)totalCount / pageSize);
            if (isAsc)
            {
                return temp.OrderBy(orderBy)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize).ToList(); //去掉.AsQueryable()，添加.select(t=>new Dto()).ToList()
            }

            return temp.OrderByDescending(orderBy)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize).ToList(); //.select(t=>new Dto()).ToList()

        }

        /// <summary>
        /// 返回分页模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLambda"></param>
        /// <param name="isAsc"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public PageResponse<T> LoadPageEntities<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, TKey>> orderBy)
        {
            var temp = DbContext.Set<T>().AsNoTracking().Where(whereLambda); 

            var rest = new PageResponse<T>();
            rest.PageIndex = pageIndex;
            rest.PageSize = pageSize;
            rest.RecordTotal = temp.Count();//记录总条数时，自动设置了总页数
            if (isAsc)
            {
                rest.Data = temp.OrderBy(orderBy)
                     .Skip(pageSize * (pageIndex - 1))
                     .Take(pageSize).ToList(); 
            }

            rest.Data = temp.OrderByDescending(orderBy)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize).ToList(); 

            return rest;
        }

        /// <summary>
        /// 将查询出来的数据 转换成IQueryable，然后进行分页   不跟踪数据状态
        /// </summary>
        /// <typeparam name="TQ">返回类型</typeparam>
        /// <typeparam name="TKey">根据哪个字段排序（必须）</typeparam>
        /// <param name="query">数据集</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="totalCount">总条数</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="isAsc">是否倒序</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns>IQueryable分页结果</returns>
        public IQueryable<TQ> LoadPageEntities<TQ, TKey>(IQueryable<TQ> query, int pageIndex, int pageSize, out int totalCount, out int pageCount, bool isAsc, Expression<Func<TQ, TKey>> orderBy) where TQ : class, new()
        {
            IQueryable<TQ> temp = query.AsNoTracking();
            totalCount = temp.Count();
            pageCount = (int)Math.Ceiling((double)totalCount / pageSize);
            if (isAsc)
            {
                temp = temp.OrderBy(orderBy)
                           .Skip(pageSize * (pageIndex - 1))
                           .Take(pageSize).AsQueryable();
            }
            else
            {
                temp = temp.OrderByDescending(orderBy)
                          .Skip(pageSize * (pageIndex - 1))
                          .Take(pageSize).AsQueryable();
            }
            return temp;
        }

        /// <summary>
        /// 将查询出来的数据 转换成IQueryable，然后进行分页   不跟踪数据状态
        /// </summary>
        /// <typeparam name="TQ">返回类型</typeparam>
        /// <typeparam name="TKey">根据哪个字段排序（必须）</typeparam>
        /// <param name="query">数据集</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="isAsc">是否倒序</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns>PageResponse分页结果</returns>
        public PageResponse<TQ> LoadPageEntities<TQ, TKey>(IQueryable<TQ> query, int pageIndex, int pageSize, bool isAsc, Expression<Func<TQ, TKey>> orderBy) where TQ : class, new()
        {
            var rest = new PageResponse<TQ>();
            IQueryable<TQ> temp = query.AsNoTracking();
            rest.RecordTotal = temp.Count();
            if (isAsc)
            {
                rest.Data = temp.OrderBy(orderBy)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize).ToList();
            }
            else
            {
                rest.Data = temp.OrderByDescending(orderBy)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize).ToList();
            }
            return rest;
        }

        #endregion

        /// <summary>
        /// 自带事务，调用此方法保存
        /// </summary>
        public int SaveChanges()
        {
            var res = -1;
            try
            {
                res = DbContext.SaveChanges();
                //Dispose();
            }
            catch (DbException ex)
            {
                throw new CustomSystemException($"数据库保存失败!{ex.Message}", 999);
            }
            catch (Exception ex)
            {
                throw new CustomSystemException($"数据库保存失败!{ex.Message}", 999);
            }
            return res;
        }

        public void Dispose()
        {
            this.DbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
