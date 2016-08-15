using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Models
{
    internal class ReservationDbInitializer : IDatabaseInitializer<ReservationDbContext>
    {
        public void InitializeDatabase(ReservationDbContext context)
        {            
            //数据库初始化，不存在则创建
            if (!context.Database.Exists())
            {
                context.Database.Create();
                //初始化数据
                InitData(context);
            }
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
                new BlockType() { TypeId = Guid.NewGuid(), TypeName = "Ip地址" },
                new BlockType() { TypeId = Guid.NewGuid(), TypeName = "预约人姓名" }
            };
                context.BlockTypes.AddRange(blockTypes);
                //Places init
                List<ReservationPlace> places = new List<ReservationPlace>()
            {
                new ReservationPlace() { PlaceId = Guid.NewGuid(),PlaceName="Room001"},
                new ReservationPlace() { PlaceId = Guid.NewGuid(),PlaceName="Room002"},
                new ReservationPlace() { PlaceId = Guid.NewGuid(),PlaceName="Room003"},
                new ReservationPlace() { PlaceId = Guid.NewGuid(),PlaceName="Room004"},
                new ReservationPlace() { PlaceId = Guid.NewGuid(),PlaceName="Room005"}
            };
                context.ReservationPlaces.AddRange(places);
                //sys settings init
                List<SystemSettings> settings = new List<SystemSettings>()
            {
                new SystemSettings() { SettingId = Guid.NewGuid(),SettingName = "SystemTitle",DisplayName="系统标题",SettingValue="活动室预约系统"},
                new SystemSettings() { SettingId = Guid.NewGuid(),SettingName = "SystemKeyWords",DisplayName="系统关键词",SettingValue="预约,活动室,河南理工大学"}
            };
                context.SystemSettings.AddRange(settings);
                //update db
                context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}