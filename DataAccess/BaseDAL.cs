using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess
{
    public class BaseDaL<T> where T : class
    {
        /// <summary>
        /// logger
        /// </summary>
        protected static Common.LogHelper logger = new Common.LogHelper(typeof(BaseDaL<T>));
        /// <summary>
        /// db operator
        /// </summary>
        protected Models.ReservationDbContext db = new Models.ReservationDbContext();

        public bool Exist(Expression<Func<T, bool>> whereLambda)
        {
            T t = GetOne(whereLambda);
            if (t == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 添加一个实体
        /// </summary>
        /// <param name="t">entity</param>
        /// <returns>更新数据库影响的行数</returns>
        public int Add(T t)
        {
            db.Set<T>().Add(t);
            return db.SaveChanges();
        }

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="ts">实体列表</param>
        /// <returns></returns>
        public int Add(List<T> ts)
        {
            db.Set<T>().AddRange(ts);
            return db.SaveChanges();
        }

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int Delete(T t)
        {
            RemoveHoldingEntityInContext(t);
            db.Set<T>().Attach(t);
            db.Set<T>().Remove(t);
            return db.SaveChanges();
        }

        public T GetOne(Expression<Func<T, bool>> whereLambda)
        {
            try
            {
                T t = db.Set<T>().Where(whereLambda).FirstOrDefault();
                return t;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll()
        {
            return db.Set<T>().ToList();
        }

        /// <summary>
        /// 查询所有数据并根据指定项排序
        /// </summary>
        /// <typeparam name="TKey">排序项</typeparam>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="isAsc">是否是正序排列</param>
        /// <returns></returns>
        public List<T> GetAll<TKey>(Expression<Func<T, TKey>> orderBy,bool isAsc)
        {
            if (isAsc)
            {
                return db.Set<T>().OrderBy(orderBy).ToList();
            }
            else
            {
                return db.Set<T>().OrderByDescending(orderBy).ToList();
            }
        }
        
        /// <summary>
        /// 查询符合条件的数据集合
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <returns></returns>
        public List<T> GetAll(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).AsNoTracking().ToList();
        }
        
        /// <summary>
        /// 根据指定条件查询数据并按指定项排序
        /// </summary>
        /// <typeparam name="TKey">排序项</typeparam>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="isAsc">是否正序排列</param>
        /// <returns></returns>
        public List<T> GetAll<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, bool isAsc)
        {
            if (isAsc)
            {
                return db.Set<T>().Where(whereLambda).OrderBy(orderBy).ToList();
            }
            else
            {
                return db.Set<T>().Where(whereLambda).OrderByDescending(orderBy).ToList();
            }
        }

        /// <summary>
        /// 查询分页数据 + List<T> GetPagedList
        /// </summary>
        /// <typeparam name="TKey">排序用到的键值</typeparam>
        /// <param name="pageIndex">页码索引，第几页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="whereLambda">查询条件Linq表达式</param>
        /// <param name="orderBy">排序条件Linq表达式</param>
        /// <param name="isAsc">是否是正向排序</param>
        /// <returns>符合要求的数据列表</returns>
        public List<T> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            // 分页 一定注意： Skip 之前一定要 OrderBy
            if (isAsc)
            {
                return db.Set<T>().Where(whereLambda).AsNoTracking().OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return db.Set<T>().Where(whereLambda).AsNoTracking().OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        #region 查询分页数据（返回符合要求的记录总数）+ GetPagedList

        /// <summary>
        /// 查询分页数据（返回符合要求的记录总数）+ GetPagedList
        /// </summary>
        /// <typeparam name="TKey">排序用到的键值</typeparam>
        /// <param name="pageIndex">页码索引，第几页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="rowsCount">总记录数</param>
        /// <param name="whereLambda">查询条件Linq表达式</param>
        /// <param name="orderBy">排序条件Linq表达式</param>
        /// <param name="isAsc">是否正序排列</param>
        /// <returns>符合要求的列表</returns>
        public virtual List<T> GetPagedList<TKey>(int pageIndex, int pageSize, out int rowsCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, bool isAsc = true)
        {
            try
            {
                //查询总的记录数
                rowsCount = whereLambda == null ? db.Set<T>().Count() : db.Set<T>().Where(whereLambda).Count();
                // 分页 一定注意： Skip 之前一定要 OrderBy
                if (isAsc)
                {
                    return db.Set<T>().Where(whereLambda).AsNoTracking().OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    return db.Set<T>().Where(whereLambda).AsNoTracking().OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                rowsCount = -1;
                logger.Error(ex);
                return null;
            }            
        }

        /// <summary>
        /// 获取分页数据，双排序
        /// </summary>
        /// <typeparam name="TKey">排序键值1</typeparam>
        /// <typeparam name="TKey1">排序键值2</typeparam>
        /// <param name="pageIndex">页码索引</param>
        /// <param name="pageSize">页码容量，每页数据量</param>
        /// <param name="rowsCount">符合条件的总数据量</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderBy">排序条件1，首要排序条件</param>
        /// <param name="orderby1">排序条件2，次要排序条件</param>
        /// <param name="isAsc">首要排序条件的排序顺序，是否正序排列</param>
        /// <param name="isAsc1">次要排序条件的排序顺序，是否正序排列 </param>
        /// <returns>符合条件的数据集合</returns>
        public virtual List<T> GetPagedList<TKey,TKey1>(int pageIndex, int pageSize, out int rowsCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy,Expression<Func<T,TKey1>> orderby1, bool isAsc = true,bool isAsc1=true)
        {
            try
            {
                //查询总的记录数
                rowsCount = db.Set<T>().Where(whereLambda).Count();
                // 分页 一定注意： Skip 之前一定要 OrderBy
                if (isAsc)
                {
                    if (isAsc1)
                    {

                        return db.Set<T>().Where(whereLambda).AsNoTracking().OrderBy(orderBy).ThenBy(orderby1).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                    else
                    {

                        return db.Set<T>().Where(whereLambda).AsNoTracking().OrderBy(orderBy).ThenByDescending(orderby1).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                else
                {
                    if (isAsc1)
                    {

                        return db.Set<T>().Where(whereLambda).AsNoTracking().OrderByDescending(orderBy).ThenBy(orderby1).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                    else
                    {

                        return db.Set<T>().Where(whereLambda).AsNoTracking().OrderByDescending(orderBy).ThenByDescending(orderby1).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                rowsCount = -1;
                logger.Error(ex);
                return null;
            }
        }

        #endregion 查询分页数据（返回符合要求的记录总数）+ GetPagedList

        /// <summary>
        /// 更新一个实体
        /// </summary>
        /// <param name="t">实体</param>
        /// <param name="propertyNames">要修改的属性名称</param>
        /// <returns>数据库受影响的行数</returns>
        public int Update(T t, params string[] propertyNames)
        {
            RemoveHoldingEntityInContext(t);
            //4.1将 对象 添加到 EF中
            DbEntityEntry entry = db.Entry(t);
            //4.2先设置 对象的包装 状态为 Unchanged
            entry.State = EntityState.Unchanged;
            //4.3循环 被修改的属性名 数组
            foreach (string proName in propertyNames)
            {
                //4.4将每个 被修改的属性的状态 设置为已修改状态;后面生成update语句时，就只为已修改的属性 更新
                entry.Property(proName).IsModified = true;
            }
            //4.4一次性 生成sql语句到数据库执行
            return db.SaveChanges();
        }

        /// <summary>
        /// 执行SQL 语句 SqlCommand
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="paras">参数</param>
        /// <returns>执行sql语句后受影响的行数</returns>
        public virtual int ExcuteSql(string strSql, params System.Data.SqlClient.SqlParameter[] paras)
        {
            return db.Database.ExecuteSqlCommand(strSql, paras);
        }

        /// <summary>
        /// 监测Context中的Entity是否存在，如果存在，将其Detach，防止出现问题
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        private bool RemoveHoldingEntityInContext(T entity)
        {
            ObjectContext objContext = ((IObjectContextAdapter)db).ObjectContext;
            var objSet = objContext.CreateObjectSet<T>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);
            object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);
            if (exists)
            {
                objContext.Detach(foundEntity);
            }
            return (exists);
        }
    }
}