using System;
using System.Collections;
using System.Collections.Generic;

namespace ActivityReservation.Helpers
{
    /// <summary>
    /// 数据分页模型
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class PagedListModel<T>
    {
        public IEnumerable<T> Data { get; set; }

        public PagerModel Pager { get; set; }

        public PagedListModel(IEnumerable<T> data)
        {
            Data = data;
        }

        public PagedListModel(IEnumerable<T> data, PagerModel pager)
        {
            Data = data;
            Pager = pager;
        }
    }

    public static class Extensions
    {
        public static PagedListModel<T> ToPagedList<T>(this IEnumerable<T> data)
        {
            return new PagedListModel<T>(data);
        }

        public static PagedListModel<T> ToPagedList<T>(this IEnumerable<T> data,PagerModel pager)
        {
            return new PagedListModel<T>(data,pager);
        }

        public static PagedListModel<T> ToPagedList<T>(this IEnumerable<T> data,int pageIndex,int pageSize,int totalCount)
        {
            return new PagedListModel<T>(data,new PagerModel(pageIndex,pageSize,totalCount));
        }
    }
}