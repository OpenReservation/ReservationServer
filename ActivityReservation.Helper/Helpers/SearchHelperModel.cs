namespace ActivityReservation.Helpers
{
    public class SearchHelperModel
    {
        private int pageNumber = 1, pageSize = 10;

        /// <summary>
        /// 当前页码索引
        /// </summary>
        public int PageIndex
        {
            get => pageNumber;
            set
            {
                if (value > 0)
                {
                    pageNumber = value;
                }
            }
        }

        /// <summary>
        /// 页码容量，每页数据量
        /// </summary>
        public int PageSize
        {
            get => pageSize;
            set
            {
                if (value > 0)
                {
                    pageSize = value;
                }
            }
        }

        public string SearchItem0 { get; set; }

        public string SearchItem1 { get; set; }

        public string SearchItem2 { get; set; }
    }
}
