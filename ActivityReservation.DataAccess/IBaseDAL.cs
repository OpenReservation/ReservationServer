using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ActivityReservation.DataAccess
{
    /// <summary>
    /// IBaseDAL
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public interface IBaseDAL<T>
    {
        /// <summary>
        /// 判断是否存在指定条件的数据
        /// </summary>
        /// <param name="whereLambda">条件</param>
        /// <returns></returns>
        bool Exist(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 添加一个实体
        /// </summary>
        /// <param name="t">entity</param>
        /// <returns>更新数据库影响的行数</returns>
        int Add(T t);

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="ts">实体列表</param>
        /// <returns></returns>
        int Add(List<T> ts);

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        int Delete(T t);

        T Fetch(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        List<T> GetAll();

        /// <summary>
        /// 查询所有数据并根据指定项排序
        /// </summary>
        /// <typeparam name="TKey">排序项</typeparam>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="isAsc">是否是正序排列</param>
        /// <returns></returns>
        List<T> GetAll<TKey>(Expression<Func<T, TKey>> orderBy, bool isAsc);

        /// <summary>
        /// 查询符合条件的数据集合
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <returns></returns>
        List<T> GetAll(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 根据指定条件查询数据并按指定项排序
        /// </summary>
        /// <typeparam name="TKey">排序项</typeparam>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="isAsc">是否正序排列</param>
        /// <returns></returns>
        List<T> GetAll<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, bool isAsc);

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
        List<T> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda,
            Expression<Func<T, TKey>> orderBy, bool isAsc = true);

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
        List<T> GetPagedList<TKey>(int pageIndex, int pageSize, out int rowsCount,
            Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy, bool isAsc = true);

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
        List<T> GetPagedList<TKey, TKey1>(int pageIndex, int pageSize, out int rowsCount,
            Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy,
            Expression<Func<T, TKey1>> orderby1, bool isAsc = true, bool isAsc1 = true);

        #endregion 查询分页数据（返回符合要求的记录总数）+ GetPagedList

        /// <summary>
        /// 更新一个实体
        /// </summary>
        /// <param name="t">实体</param>
        /// <param name="propertyNames">要修改的属性名称</param>
        /// <returns>数据库受影响的行数</returns>
        int Update(T t, params string[] propertyNames);
    }
}
