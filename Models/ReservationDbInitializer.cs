using System;
using System.Collections.Generic;
using System.Data.Entity;
using WeihanLi.Common.Helpers;

namespace Models
{
    internal class ReservationDbInitializer : DropCreateDatabaseIfModelChanges<ReservationDbContext>
    {
        private static LogHelper Logger = LogHelper.GetLogHelper<ReservationDbInitializer>();

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
        ///     初始化数据库中数据
        /// </summary>
        /// <param name="context">数据上下文</param>
        private static void InitData(ReservationDbContext context)
        {
            try
            {
                //user init
                var u = new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = "admin",
                    UserPassword = SecurityHelper.SHA256_Encrypt("Admin888"),
                    IsSuper = true
                };
                context.Users.Add(u);
                //block types init
                var blockTypes = new List<BlockType>
                {
                    new BlockType {TypeId = Guid.NewGuid(), TypeName = "联系方式"},
                    new BlockType {TypeId = Guid.NewGuid(), TypeName = "IP地址"},
                    new BlockType {TypeId = Guid.NewGuid(), TypeName = "预约人姓名"}
                };
                context.BlockTypes.AddRange(blockTypes);
                var placeId = Guid.NewGuid();
                //Places init
                context.ReservationPlaces.AddRange(new[] {
                    new ReservationPlace { PlaceId = placeId, PlaceName = "第一多功能厅", UpdateBy = "System", PlaceIndex = 0 },
                    new ReservationPlace { PlaceId = Guid.NewGuid(), PlaceName = "第二多功能厅", UpdateBy = "System", PlaceIndex = 1 },
                    new ReservationPlace { PlaceId = Guid.NewGuid(), PlaceName = "第一排练厅", UpdateBy = "System", PlaceIndex = 2 },
                    new ReservationPlace { PlaceId = Guid.NewGuid(), PlaceName = "宣传制作室", UpdateBy = "System", PlaceIndex = 3 } });
                context.ReservationPeriods.AddRange(new[]
                {
                    new ReservationPeriod
                    {
                        PeriodId = Guid.NewGuid(),
                        PeriodIndex = 0,
                        PeriodTitle = "8:00~10:00",
                        PeriodDescription = "8:00~10:00",
                        PlaceId = placeId,
                        UpdateBy = "System",
                        UpdateTime = DateTime.Now
                    },
                    new ReservationPeriod
                    {
                        PeriodId = Guid.NewGuid(),
                        PeriodIndex = 1,
                        PeriodTitle = "10:00~12:00",
                        PeriodDescription = "10:00~12:00",
                        PlaceId = placeId,
                        UpdateBy = "System",
                        UpdateTime = DateTime.Now
                    },
                    new ReservationPeriod
                    {
                        PeriodId = Guid.NewGuid(),
                        PeriodIndex = 2,
                        PeriodTitle = "13:00~16:00",
                        PeriodDescription = "13:00~16:00",
                        PlaceId = placeId,
                        UpdateBy = "System",
                        UpdateTime = DateTime.Now
                    }
                });
                //sys settings init
                var settings = new List<SystemSettings>
                {
                    new SystemSettings
                    {
                        SettingId = Guid.NewGuid(),
                        SettingName = "SystemTitle",
                        DisplayName = "系统标题",
                        SettingValue = "活动室预约系统"
                    },
                    new SystemSettings
                    {
                        SettingId = Guid.NewGuid(),
                        SettingName = "SystemKeywords",
                        DisplayName = "系统关键词",
                        SettingValue = "预约,活动室,预定,reservation"
                    },
                    new SystemSettings
                    {
                        SettingId = Guid.NewGuid(),
                        SettingName = "SystemDescription",
                        DisplayName = "系统简介",
                        SettingValue = "活动室预约系统是一个基于ASP.NET MVC 开发的一个在线预约系统。"
                    },
                    new SystemSettings
                    {
                        SettingId = Guid.NewGuid(),
                        SettingName = "SystemContactPhone",
                        DisplayName = "系统联系人联系电话",
                        SettingValue = "15601655489"
                    },
                    new SystemSettings
                    {
                        SettingId = Guid.NewGuid(),
                        SettingName = "SystemContactEmail",
                        DisplayName = "系统联系邮箱",
                        SettingValue = "weihanli@outlook.com"
                    }
                };
                context.SystemSettings.AddRange(settings);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                throw ex;
            }
        }
    }
}