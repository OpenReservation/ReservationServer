using System;
using System.Text;
using System.Web.Mvc;

namespace ActivityReservation.Helpers
{
    /// <summary>
    /// PagerHelper 分页帮助类
    /// </summary>
    public static class PagerHelper
    {
        public static MvcHtmlString Pager(this HtmlHelper helper, PagerModel pager)
        {
            StringBuilder sbHtmlText = new StringBuilder();
            sbHtmlText.Append("<div class='form-inline pager' style='text-align:center;padding:10px;'>");
            if (pager.PageIndex <= 1)
            {
                sbHtmlText.Append("<button type='button' class='btn btn-link disabled' disabled='disabled'>第一页</button>");
                sbHtmlText.Append("<button type='button' class='btn btn-link disabled' disabled='disabled'>上一页</button>");
            }
            else
            {
                sbHtmlText.Append("<button type='button' class='btn btn-link' onclick='loadData(1)'>上一页</button>");
                sbHtmlText.AppendFormat("<button type='button' class='btn btn-link' onclick='loadData({0})'>上一页</button>", pager.PageIndex - 1);
                sbHtmlText.AppendFormat("&nbsp;<button type = 'button' class='btn btn-link' onclick = 'loadData({0})'> {0} </button>", pager.PageIndex - 1);
            }
            sbHtmlText.AppendFormat("<span><strong>{0}</strong></span>", pager.PageIndex);
            if (pager.PageIndex >= pager.PageCount)
            {
                sbHtmlText.Append("<button type='button' class='btn btn-link disabled' disabled='disabled'>下一页</button>&nbsp;");
                sbHtmlText.Append("<button type='button' class='btn btn-link disabled' disabled='disabled'>最后一页</button>");
            }
            else
            {
                sbHtmlText.AppendFormat("<button type = 'button' class='btn btn-link' onclick = 'loadData({0})'> {0} </button>", pager.PageIndex + 1);
                sbHtmlText.AppendFormat("<button type='button' class='btn btn-link' onclick='loadData({0})'>下一页</button>", pager.PageIndex + 1);
                sbHtmlText.AppendFormat("<button type='button' class='btn btn-link' onclick='loadData({0})'>最后一页</button>", pager.PageSize);
            }

            sbHtmlText.AppendFormat("<div style='display:inline-block'><span>每页有<strong>{0}</strong>条数据，一共有<strong>{1}</strong>页，总计<strong>{2}</strong>条数据</span></div>", pager.PageSize, pager.PageCount, pager.TotalCount);
            sbHtmlText.Append("</div>");
            return MvcHtmlString.Create(sbHtmlText.ToString());
        }
    }

    /// <summary>
    /// PagerModel 分页数据模型
    /// </summary>
    public class PagerModel
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get { return Convert.ToInt32(Math.Floor(TotalCount * 1.0 / PageSize)); } }

        public int TotalCount { get; set; }

        public PagerModel(int totalCount)
        {
            PageIndex = 1;
            PageSize = 10;
            TotalCount = totalCount;
        }

        public PagerModel(int pageSize, int totalCount)
        {
            PageIndex = 1;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }
}