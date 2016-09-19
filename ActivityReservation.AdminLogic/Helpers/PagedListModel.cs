using System;
using System.Collections;
using System.Collections.Generic;

namespace ActivityReservation.Helpers
{
    /// <summary>
    /// 数据分页模型
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class PagedListModel<T> where T : class, new()
    {
        public List<T> Data { get; set; }

        public PagerModel Pager { get; set; }
    }

    public static class Extensions
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> data)
        {
            return new PagedList<T>(data);
        }

        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> data,PagerModel pager)
        {
            return new PagedList<T>(data,pager);
        }

        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> data,int pageIndex,int pageSize,int totalCount)
        {
            return new PagedList<T>(data,new PagerModel(pageIndex,pageSize,totalCount));
        }
    }
}