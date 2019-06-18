using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityReservation.Models
{
    [Table("tabReservationPlace")]
    public class ReservationPlace
    {
        #region Private Field

        /// <summary>
        /// 活动室id
        /// </summary>
        private Guid placeId;

        /// <summary>
        /// 活动室名称
        /// </summary>
        private string placeName;

        /// <summary>
        /// 更新时间
        /// </summary>
        private DateTime updateTime = DateTime.UtcNow;

        /// <summary>
        /// 更新人
        /// </summary>
        private string updateBy;

        /// <summary>
        /// 是否启用
        /// </summary>
        private bool isActive = true;

        /// <summary>
        /// 是否删除
        /// </summary>
        private bool isDel;

        #endregion Private Field

        /// <summary>
        /// 活动室id
        /// </summary>
        [Key]
        [Column]
        public Guid PlaceId
        {
            get { return placeId; }

            set { placeId = value; }
        }

        /// <summary>
        /// 活动室名称
        /// </summary>
        [Column]
        public string PlaceName
        {
            get { return placeName; }

            set { placeName = value; }
        }

        /// <summary>
        /// 排序
        /// </summary>
        [Column]
        public int PlaceIndex { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column]
        public DateTime UpdateTime
        {
            get { return updateTime; }

            set { updateTime = value; }
        }

        /// <summary>
        /// 更新人
        /// </summary>
        [Column]
        public string UpdateBy
        {
            get { return updateBy; }

            set { updateBy = value; }
        }

        /// <summary>
        /// 最多可预约时间段数量
        /// </summary>
        [Column]
        public int MaxReservationPeriodNum { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Column]
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Column]
        public bool IsDel
        {
            get { return isDel; }

            set { isDel = value; }
        }

        [NotMapped]
        public IReadOnlyList<ReservationPeriod> ReservationPeriodList { get; set; }
    }
}
