using System;
using System.Collections.Generic;
using System.Linq;
using ActivityReservation.Models;
using ActivityReservation.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeihanLi.Common;
using WeihanLi.Common.Helpers;

namespace ActivityReservation.Helpers
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            var dbContext = DependencyResolver.Current.ResolveService<ReservationDbContext>();
            dbContext.Database.EnsureCreated();
            IReadOnlyCollection<SystemSettings> settings;
            if (!dbContext.Users.AsNoTracking().Any())
            {
                dbContext.Users.Add(new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = "admin",
                    UserPassword = SecurityHelper.SHA256_Encrypt("Admin888"),
                    IsSuper = true
                });

                var blockTypes = new List<BlockType>
                {
                    new BlockType {TypeId = Guid.NewGuid(), TypeName = "联系方式"},
                    new BlockType {TypeId = Guid.NewGuid(), TypeName = "IP地址"},
                    new BlockType {TypeId = Guid.NewGuid(), TypeName = "预约人姓名"}
                };
                dbContext.BlockTypes.AddRange(blockTypes);

                var placeId = Guid.NewGuid();
                var placeId1 = Guid.NewGuid();
                //Places init
                dbContext.ReservationPlaces.AddRange(new[] {
                    new ReservationPlace { PlaceId = placeId, PlaceName = "第一多功能厅", UpdateBy = "System", PlaceIndex = 0 },
                    new ReservationPlace { PlaceId = placeId1, PlaceName = "第二多功能厅", UpdateBy = "System", PlaceIndex = 1 }});
                dbContext.ReservationPeriods.AddRange(new[]
                {
                    new ReservationPeriod
                    {
                        PeriodId = Guid.NewGuid(),
                        PeriodIndex = 3,
                        PeriodTitle = "8:00~10:00",
                        PeriodDescription = "8:00~10:00",
                        PlaceId = placeId,
                        CreateBy = "System",
                        CreateTime = DateTime.Now,
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
                        CreateBy = "System",
                        CreateTime = DateTime.Now.AddSeconds(2),
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
                        CreateBy = "System",
                        CreateTime = DateTime.Now.AddSeconds(3),
                        UpdateBy = "System",
                        UpdateTime = DateTime.Now
                    },
                    new ReservationPeriod
                    {
                        PeriodId = Guid.NewGuid(),
                        PeriodIndex = 1,
                        PeriodTitle = "08:00~18:00",
                        PeriodDescription = "08:00~18:00",
                        PlaceId = placeId1,
                        CreateBy = "System",
                        CreateTime = DateTime.Now.AddSeconds(3),
                        UpdateBy = "System",
                        UpdateTime = DateTime.Now
                    },
                });
                //sys settings init
                settings = new List<SystemSettings>
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
                dbContext.SystemSettings.AddRange(settings);

                dbContext.SaveChanges();
            }
            else
            {
                settings = dbContext.SystemSettings.AsNoTracking()
                .ToArray();
            }
            if (settings.Count > 0) // init settings cache
            {
                var applicationSettingService = DependencyResolver.Current.GetRequiredService<IApplicationSettingService>();
                applicationSettingService.AddSettings(settings.ToDictionary(s => s.SettingName, s => s.SettingValue));
            }
        }
    }
}
