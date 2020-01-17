using System;
using System.Collections.Generic;
using System.Linq;
using ActivityReservation.Database;
using ActivityReservation.Models;
using ActivityReservation.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions;

namespace ActivityReservation.Helpers
{
    public static class DatabaseInitializer
    {
        public static void Initialize(this IServiceProvider serviceProvider)
        {
            IReadOnlyCollection<SystemSettings> settings;

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();
                dbContext.Database.EnsureCreated();

                // TODO: update with dbContext.Database.IsRelational when new ef core released, https://github.com/dotnet/efcore/pull/19521
                if (dbContext.Database.ProviderName.EqualsIgnoreCase("Microsoft.EntityFrameworkCore.InMemory"))
                {
                    if (!dbContext.Users.AsNoTracking().Any())
                    {
                        settings = InitData(dbContext);
                    }
                    else
                    {
                        settings = dbContext.SystemSettings.AsNoTracking().ToArray();
                    }
                }
                else
                {
                    using (var transaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                    {
                        if (!dbContext.Users.AsNoTracking().Any())
                        {
                            settings = InitData(dbContext);
                            transaction.Commit();
                        }
                        else
                        {
                            settings = dbContext.SystemSettings.AsNoTracking().ToArray();
                        }
                    }
                }
            }

            if (settings.Count > 0) // init settings cache
            {
                var applicationSettingService = serviceProvider.GetRequiredService<IApplicationSettingService>();
                applicationSettingService.AddSettings(settings.ToDictionary(s => s.SettingName, s => s.SettingValue));
            }
        }

        private static IReadOnlyCollection<SystemSettings> InitData(ReservationDbContext dbContext)
        {
            dbContext.Users.Add(new User
            {
                UserId = Guid.NewGuid(),
                UserName = "admin",
                UserMail = "admin@weihanli.xyz",
                UserPassword = SecurityHelper.SHA256("Admin888"),
                IsSuper = true
            });
            dbContext.Users.Add(new User
            {
                UserId = Guid.NewGuid(),
                UserName = "Alice",
                UserMail = "Alice@weihanli.xyz",
                UserPassword = SecurityHelper.SHA256("Test1234"),
                IsSuper = false
            });
            dbContext.Users.Add(new User
            {
                UserId = Guid.NewGuid(),
                UserName = "test",
                UserMail = "test@weihanli.xyz",
                UserPassword = SecurityHelper.SHA256("Test1234"),
                IsSuper = false
            });

            var blockTypes = new List<BlockType>
            {
                new BlockType { TypeId = Guid.NewGuid(), TypeName = "Contact Phone" },
                new BlockType { TypeId = Guid.NewGuid(), TypeName = "IP" },
                new BlockType { TypeId = Guid.NewGuid(), TypeName = "Contact Name" }
            };
            dbContext.BlockTypes.AddRange(blockTypes);

            var placeId = Guid.NewGuid();
            var placeId1 = Guid.NewGuid();
            //Places init
            dbContext.ReservationPlaces.AddRange(new[]
                {
                    new ReservationPlace { PlaceId = placeId, PlaceName = "第一多功能厅", UpdateBy = "System", PlaceIndex = 0,MaxReservationPeriodNum = 2 },
                    new ReservationPlace { PlaceId = placeId1, PlaceName = "第二多功能厅", UpdateBy = "System", PlaceIndex = 1,MaxReservationPeriodNum = 2},
                });

            dbContext.ReservationPeriods.AddRange(new[]
            {
                new ReservationPeriod
                {
                    PeriodId = Guid.NewGuid(),
                    PeriodIndex = 0,
                    PeriodTitle = "8:00~10:00",
                    PeriodDescription = "8:00~10:00",
                    PlaceId = placeId,
                    CreateBy = "System",
                    CreateTime = DateTime.UtcNow,
                    UpdateBy = "System",
                    UpdateTime = DateTime.UtcNow
                },
                new ReservationPeriod
                {
                    PeriodId = Guid.NewGuid(),
                    PeriodIndex = 1,
                    PeriodTitle = "10:00~12:00",
                    PeriodDescription = "10:00~12:00",
                    PlaceId = placeId,
                    CreateBy = "System",
                    CreateTime = DateTime.UtcNow,
                    UpdateBy = "System",
                    UpdateTime = DateTime.UtcNow
                },
                new ReservationPeriod
                {
                    PeriodId = Guid.NewGuid(),
                    PeriodIndex = 2,
                    PeriodTitle = "13:00~16:00",
                    PeriodDescription = "13:00~16:00",
                    PlaceId = placeId,
                    CreateBy = "System",
                    CreateTime = DateTime.UtcNow,
                    UpdateBy = "System",
                    UpdateTime = DateTime.UtcNow
                },
                new ReservationPeriod
                {
                    PeriodId = Guid.NewGuid(),
                    PeriodIndex = 0,
                    PeriodTitle = "上午",
                    PeriodDescription = "上午",
                    PlaceId = placeId1,
                    CreateBy = "System",
                    CreateTime = DateTime.UtcNow,
                    UpdateBy = "System",
                    UpdateTime = DateTime.UtcNow
                },
                new ReservationPeriod
                {
                    PeriodId = Guid.NewGuid(),
                    PeriodIndex = 1,
                    PeriodTitle = "下午",
                    PeriodDescription = "下午",
                    PlaceId = placeId1,
                    CreateBy = "System",
                    CreateTime = DateTime.UtcNow,
                    UpdateBy = "System",
                    UpdateTime = DateTime.UtcNow
                },
            });
            var notice = new Notice()
            {
                NoticeId = Guid.NewGuid(),
                CheckStatus = true,
                NoticeTitle = "test",
                NoticeCustomPath = "test-notice",
                NoticePath = "test-notice.html",
                NoticeContent = "just for test",
                NoticePublishTime = DateTime.UtcNow,
                NoticeDesc = "just for test",
                NoticePublisher = "System"
            };
            dbContext.Notices.Add(notice);

            //sys settings init
            var settings = new List<SystemSettings>
            {
                new SystemSettings
                {
                    SettingId = Guid.NewGuid(),
                    SettingName = "SystemTitle",
                    DisplayName = "系统标题/SystemTitle",
                    SettingValue = "OpenReservation"
                },
                new SystemSettings
                {
                    SettingId = Guid.NewGuid(),
                    SettingName = "SystemKeywords",
                    DisplayName = "系统关键词/Keywords",
                    SettingValue = "预约,活动室,预定,reservation,booking"
                },
                new SystemSettings
                {
                    SettingId = Guid.NewGuid(),
                    SettingName = "SystemDescription",
                    DisplayName = "系统简介/Description",
                    SettingValue = "online reservation system powered by powerful asp.net core"
                },
                new SystemSettings
                {
                    SettingId = Guid.NewGuid(),
                    SettingName = "SystemContactPhone",
                    DisplayName = "系统联系人联系电话/ContactPhone",
                    SettingValue = "13245642365"
                },
                new SystemSettings
                {
                    SettingId = Guid.NewGuid(),
                    SettingName = "SystemContactEmail",
                    DisplayName = "系统联系邮箱/ContactEmail",
                    SettingValue = "weihanli@outlook.com"
                }
            };
            dbContext.SystemSettings.AddRange(settings);

            dbContext.SaveChanges();

            return settings;
        }
    }
}
