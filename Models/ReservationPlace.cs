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
        /// <summary>
        /// 活动室id
        /// </summary>
        private Guid placeId;

        /// <summary>
        /// 活动室名称
        /// </summary>
        private string placeName;

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
    }
}