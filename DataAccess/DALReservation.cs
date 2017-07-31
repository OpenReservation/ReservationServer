using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public partial interface IDALReservation
    {
        /// <summary>
        /// 获取预约数据列表
        /// </summary>
        /// <typeparam name="Tkey">排序字段1</typeparam>
        /// <typeparam name="TKey1">排序字段2</typeparam>
        /// <param name="pageIndex">页码索引</param>
        /// <param name="pageSize">每页的数据量</param>
        /// <param name="rowsCount">数据总量</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderBy">排序字段1</param>
        /// <param name="orderby1">排序字段2</param>
        /// <param name="isAsc">排序1是否正序排列</param>
        /// <param name="isAsc1">排序2是否正序</param>
        /// <returns></returns>
        List<Models.Reservation> GetReservationList<Tkey, TKey1>(int pageIndex, int pageSize, out int rowsCount, Expression<Func<Models.Reservation, bool>> whereLambda, Expression<Func<Models.Reservation, Tkey>> orderBy, Expression<Func<Models.Reservation, TKey1>> orderby1, bool isAsc, bool isAsc1);
    }

    public partial class DALReservation
    {
        /// <summary>
        /// 获取预约数据列表
        /// </summary>
        /// <typeparam name="Tkey">排序字段1</typeparam>
        /// <typeparam name="TKey1">排序字段2</typeparam>
        /// <param name="pageIndex">页码索引</param>
        /// <param name="pageSize">每页的数据量</param>
        /// <param name="rowsCount">数据总量</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderBy">排序字段1</param>
        /// <param name="orderby1">排序字段2</param>
        /// <param name="isAsc">排序1是否正序排列</param>
        /// <param name="isAsc1">排序2是否正序</param>
        /// <returns></returns>
        public List<Models.Reservation> GetReservationList<Tkey,TKey1>(int pageIndex, int pageSize, out int rowsCount, Expression<Func<Models.Reservation, bool>> whereLambda, Expression<Func<Models.Reservation,Tkey>> orderBy, Expression<Func<Models.Reservation, TKey1>> orderby1, bool isAsc, bool isAsc1)
        {
            try
            {
                //查询总的记录数
                rowsCount = db.Set<Models.Reservation>().Where(whereLambda).Count();
                // 分页 一定注意： Skip 之前一定要 OrderBy
                if (isAsc)
                {
                    if (isAsc1)
                    {

                        return db.Set<Models.Reservation>().Include("Place").Where(whereLambda).OrderBy(orderBy).ThenBy(orderby1).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                    else
                    {

                        return db.Set<Models.Reservation>().Include("Place").Where(whereLambda).OrderBy(orderBy).ThenByDescending(orderby1).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                else
                {
                    if (isAsc1)
                    {

                        return db.Set<Models.Reservation>().Include("Place").Where(whereLambda).OrderByDescending(orderBy).ThenBy(orderby1).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                    else
                    {

                        return db.Set<Models.Reservation>().Include("Place").Where(whereLambda).OrderByDescending(orderBy).ThenByDescending(orderby1).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                rowsCount = -1;
                return null;
            }
        }
    }
}
