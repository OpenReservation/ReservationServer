﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenReservation.Database;
using OpenReservation.Models;
using OpenReservation.Services;

namespace OpenReservation.API.Test;

internal class TestDataInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        IReadOnlyCollection<SystemSettings> settings;

        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();
            dbContext.Database.EnsureCreated();
            if (!dbContext.SystemSettings.AsNoTracking().Any())
            {
                var blockTypes = new List<BlockType>
                {
                    new() {TypeId = Guid.NewGuid(), TypeName = "联系方式"},
                    new() {TypeId = Guid.NewGuid(), TypeName = "IP地址"},
                    new() {TypeId = Guid.NewGuid(), TypeName = "预约人姓名"}
                };
                dbContext.BlockTypes.AddRange(blockTypes);

                var placeId = Guid.NewGuid();
                var placeId1 = Guid.NewGuid();
                //Places init
                dbContext.ReservationPlaces.AddRange(new[] {
                    new ReservationPlace { PlaceId = placeId, PlaceName = "第一多功能厅", UpdateBy = "System", PlaceIndex = 0,MaxReservationPeriodNum = 2 },
                    new ReservationPlace { PlaceId = placeId1, PlaceName = "第二多功能厅", UpdateBy = "System", PlaceIndex = 1,MaxReservationPeriodNum = 2}}
                );

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
                        PeriodIndex = 1,
                        PeriodTitle = "08:00~18:00",
                        PeriodDescription = "08:00~18:00",
                        PlaceId = placeId1,
                        CreateBy = "System",
                        CreateTime = DateTime.UtcNow.AddSeconds(3),
                        UpdateBy = "System",
                        UpdateTime = DateTime.UtcNow
                    },
                });
                var notice = new Notice()
                {
                    NoticeId = Guid.NewGuid(),
                    CheckStatus = true,
                    NoticeTitle = "测试公告",
                    NoticeCustomPath = "test-notice",
                    NoticePath = "test-notice.html",
                    NoticeContent = "测试一下",
                    NoticePublishTime = DateTime.UtcNow,
                    NoticeDesc = "测试一下",
                    NoticePublisher = "System"
                };
                dbContext.Notices.Add(notice);

                //sys settings init
                settings = new List<SystemSettings>
                {
                    new()
                    {
                        SettingId = Guid.NewGuid(),
                        SettingName = "SystemTitle",
                        DisplayName = "系统标题",
                        SettingValue = "活动室预约系统"
                    },
                    new()
                    {
                        SettingId = Guid.NewGuid(),
                        SettingName = "SystemKeywords",
                        DisplayName = "系统关键词",
                        SettingValue = "预约,活动室,预定,reservation"
                    },
                    new()
                    {
                        SettingId = Guid.NewGuid(),
                        SettingName = "SystemDescription",
                        DisplayName = "系统简介",
                        SettingValue = "活动室预约系统是一个基于ASP.NET MVC 开发的一个在线预约系统。"
                    },
                    new()
                    {
                        SettingId = Guid.NewGuid(),
                        SettingName = "SystemContactPhone",
                        DisplayName = "系统联系人联系电话",
                        SettingValue = "13245642365"
                    },
                    new()
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
                settings = dbContext.SystemSettings.AsNoTracking().ToArray();
            }
        }

        if (settings.Count > 0) // init settings cache
        {
            var applicationSettingService = serviceProvider.GetRequiredService<IApplicationSettingService>();
            applicationSettingService.AddSettings(settings.ToDictionary(s => s.SettingName, s => s.SettingValue));
        }
    }
}