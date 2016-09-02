using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models
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
        private DateTime updateTime = DateTime.Now;

        /// <summary>
        /// 更新人
        /// </summary>
        private string updateBy;

        /// <summary>
        /// 是否删除
        /// </summary>
        private bool isDel = false; 
        #endregion

        /// <summary>
        /// 活动室id
        /// </summary>
        [Key]
        [Column]
        public Guid PlaceId
        {
            get
            {
                return placeId;
            }

            set
            {
                placeId = value;
            }
        }

        /// <summary>
        /// 活动室名称
        /// </summary>
        [Column]
        public string PlaceName
        {
            get
            {
                return placeName;
            }

            set
            {
                placeName = value;
            }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column]
        public DateTime UpdateTime
        {
            get
            {
                return updateTime;
            }

            set
            {
                updateTime = value;
            }
        }

        /// <summary>
        /// 更新人
        /// </summary>
        [Column]
        public string UpdateBy
        {
            get
            {
                return updateBy;
            }

            set
            {
                updateBy = value;
            }
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Column]
        public bool IsDel
        {
            get
            {
                return isDel;
            }

            set
            {
                isDel = value;
            }
        }
    }
}