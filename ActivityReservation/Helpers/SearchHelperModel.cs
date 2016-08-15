namespace ActivityReservation.Helpers
{
    public class SearchHelperModel
    {
        private int pageIndex,pageSize;

        /// <summary>
        /// 当前页码索引
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (pageIndex <=0)
                {
                    pageIndex = 1;
                }
                return pageIndex;
            }
            set
            {
                pageIndex = value;
            }
        }

        /// <summary>
        /// 页码容量，每页数据量
        /// </summary>
        public int PageSize
        {
            get
            {
                if (pageSize<=0)
                {
                    pageSize = 10;
                }
                return pageSize;
            }
            set
            {
                pageSize = value;
            }
        }

        public string SearchItem0 { get; set; }

        public string SearchItem1 { get; set; }

        public string SearchItem2 { get; set; }

        public string SearchItem3 { get; set; }

        public string SearchItem4 { get; set; }

        public string SearchItem5 { get; set; }

        public string SearchItem6 { get; set; }
    }
}