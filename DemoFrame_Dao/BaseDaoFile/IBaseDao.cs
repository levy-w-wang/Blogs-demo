using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DemoFrame_CoreMvc.Models;

namespace DemoFrame_Dao.BaseDaoFile
{
    public interface IBaseDao<T>
    {
        T Add(T entity);

        List<T> Add(List<T> entity);

        void Delete(params object[] keyValues);
        void Delete(object objectId);
        void Delete(Expression<Func<T, bool>> whereFun);
        void Update(T entity);
        void Update(Expression<Func<T, bool>> where, Dictionary<string, object> dic);
        bool Exist(Expression<Func<T, bool>> anyLambda);

        T Find(params object[] keyValues);
        IQueryable<T> Where(Expression<Func<T, bool>> whereLambda);
        T FirstOrDefault(Expression<Func<T, bool>> whereLambda);
        int Count(Expression<Func<T, bool>> countLambda);

        T First(Expression<Func<T, bool>> firstLambda);

        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda = null);

        List<T> LoadPageEntities<TKey>(int pageIndex, int pageSize,
            out int totalCount, out int pageCount,
            Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, TKey>> orderBy);

        PageResponse<T> LoadPageEntities<TKey>(int pageIndex, int pageSize,
            Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, TKey>> orderBy);

        IQueryable<TQ> LoadPageEntities<TQ, TKey>(IQueryable<TQ> query, int pageIndex, int pageSize,
            out int totalCount, out int pageCount, bool isAsc, Expression<Func<TQ, TKey>> orderBy) where TQ : class, new();

        PageResponse<TQ> LoadPageEntities<TQ, TKey>(IQueryable<TQ> query, int pageIndex, int pageSize,
            bool isAsc, Expression<Func<TQ, TKey>> orderBy) where TQ : class, new();

        int SaveChanges();
    }
}
