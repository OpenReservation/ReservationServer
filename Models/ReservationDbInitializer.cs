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
                new BlockType() { TypeId = Guid.NewGuid(), TypeName = "Phone" },
                new BlockType() { TypeId = Guid.NewGuid(), TypeName = "IpAddress" },
                new BlockType() { TypeId = Guid.NewGuid(), TypeName = "PersonName" }
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
                new SystemSettings() { SettingId = Guid.NewGuid(),SettingName = "SystemTitle",SettingValue="ReservationSystem"},
                new SystemSettings() { SettingId = Guid.NewGuid(),SettingName = "AdminBlockIpAddress",SettingValue="192.168.1.100"}
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