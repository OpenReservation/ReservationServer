using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Models
{
    class ReservationDbInitializer : DropCreateDatabaseIfModelChanges<ReservationDbContext>
    {
        public override void InitializeDatabase(ReservationDbContext context)
        {
            //数据库初始化，不存在则创建
            if (!context.Database.Exists())
            {
                context.Database.Create();
                //初始化数据
                InitData(context);
                //update db
                context.SaveChanges();
            }
            else
            {
                base.InitializeDatabase(context);
            }
        }

        protected override void Seed(ReservationDbContext context)
        {
            InitData(context);
        }

        /// <summary>
        /// 初始化数据库中数据
        /// </summary>
        /// <param name="context">数据上下文</param>
        private static void InitData(ReservationDbContext context)
        {
            try
            {
                //user init
                User u = new User() { UserId = Guid.NewGuid(), UserName = "admin", UserPassword = Common.SecurityHelper.SHA256_Encrypt("Admin888"), IsSuper = true };
                context.Users.Add(u);
                //block types init
                List<BlockType> blockTypes = new List<BlockType>()
                {
                    new BlockType() { TypeId = Guid.NewGuid(), TypeName = "联系方式" },
                    new BlockType() { TypeId = Guid.NewGuid(), TypeName = "IP地址" },
                    new BlockType() { TypeId = Guid.NewGuid(), TypeName = "预约人姓名" }
                };
                context.BlockTypes.AddRange(blockTypes);
                //Places init
                List<ReservationPlace> places = new List<ReservationPlace>()
                {
                    new ReservationPlace() { PlaceId = Guid.NewGuid(),PlaceName="小礼堂",UpdateBy = "System"},
                    new ReservationPlace() { PlaceId = Guid.NewGuid(),PlaceName="第一多功能厅",UpdateBy = "System"},
                    new ReservationPlace() { PlaceId = Guid.NewGuid(),PlaceName="第二多功能厅",UpdateBy = "System"},
                    new ReservationPlace() { PlaceId = Guid.NewGuid(),PlaceName="第一排练厅",UpdateBy = "System"},
                    new ReservationPlace() { PlaceId = Guid.NewGuid(),PlaceName="宣传制作室",UpdateBy = "System"},
                    new ReservationPlace() { PlaceId = Guid.NewGuid(),PlaceName="第一会议室",UpdateBy = "System"},
                    new ReservationPlace() { PlaceId = Guid.NewGuid(),PlaceName="第二会议室",UpdateBy = "System"}
                };
                context.ReservationPlaces.AddRange(places);
                //sys settings init
                List<SystemSettings> settings = new List<SystemSettings>()
                {
                    new SystemSettings() { SettingId = Guid.NewGuid(),SettingName = "SystemTitle",DisplayName="系统标题",SettingValue="活动室预约系统"},
                    new SystemSettings() { SettingId = Guid.NewGuid(),SettingName = "SystemKeywords",DisplayName="系统关键词",SettingValue="预约,活动室,河南理工大学"},
                    new SystemSettings() {SettingId = Guid.NewGuid(),SettingName = "SystemDescription",DisplayName = "系统简介",SettingValue = "河南理工大学活动室预约系统是活动室预约数字化平台，为方便学生预约使用活动室而开发，由河南理工大学大学生网络工作室进行开发并提供技术支持。" },
                    new SystemSettings() { SettingId = Guid.NewGuid(),SettingName = "SystemContactPhone",DisplayName = "系统联系人联系电话",SettingValue = "0391-3987123"},
                    new SystemSettings() { SettingId = Guid.NewGuid(),SettingName = "SystemContactEmail",DisplayName = "系统联系邮箱",SettingValue = "lilingjuan@hpu.edu.cn"}
                };
                context.SystemSettings.AddRange(settings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}