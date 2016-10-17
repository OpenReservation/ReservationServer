using System;
using System.Collections;
using System.Collections.Generic;

namespace ActivityReservation.Helpers
{
    #region IPagedListModel
    public interface IPagedListModel
    {
        IPagerModel Pager { get; set; }
    }

    public interface IPagedListModel<out T> : IPagedListModel
    {
        IEnumerable<T> Data { get; }
        IEnumerator<T> GetEnumerator();
    } 
    #endregion

    #region PagedListModel
    /// <summary>
    /// 数据分页模型
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class PagedListModel<T>:IPagedListModel<T>
    {
        public IEnumerable<T> Data { get;private set; }

        public IPagerModel Pager { get; set; }

        public PagedListModel(IEnumerable<T> data)
        {
            Data = data;
        }

        public PagedListModel(IEnumerable<T> data, IPagerModel pager)
        {
            Data = data;
            Pager = pager;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Data.GetEnumerator();
        }
    } 
    #endregion

    #region 扩展方法
    /// <summary>
    /// PagedListModel 扩展方法
    /// </summary>
    public static class PagedListModelExtensions
    {
        public static IPagedListModel<T> ToPagedList<T>(this IEnumerable<T> data)
        {
            return new PagedListModel<T>(data);
        }

        public static IPagedListModel<T> ToPagedList<T>(this IEnumerable<T> data, IPagerModel pager)
        {
            return new PagedListModel<T>(data, pager);
        }

        public static IPagedListModel<T> ToPagedList<T>(this IEnumerable<T> data, int pageIndex, int pageSize, int totalCount)
        {
            return new PagedListModel<T>(data, new PagerModel(pageIndex, pageSize, totalCount));
        }

        public static IPagedListModel<T> ToPagedList<T>(this ICollection<T> data, int pageIndex, int pageSize)
        {
            return new PagedListModel<T>(data, new PagerModel(pageIndex, pageSize, data.Count));
        }
    } 
    #endregion
}