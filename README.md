# ActivityReservation
## Build Status
[![Build status](https://ci.appveyor.com/api/projects/status/4d06b7edjjsx6n8r?svg=true)](https://ci.appveyor.com/project/WeihanLi/activityreservation)

## Intro

活动室预约系统，起初的设计和开发是因为学校活动室预约流程希望从之前繁琐低效的完全线下预约
修改为线上预约+线下盖章审批的方式来预约学校的活动室。

原本是用 ASP.NET WebForm 写的，最近打算用 ASP.NET MVC 重写一下并增加一些功能。

演示地址：<https://reservation.weihanli.xyz>

后台登录地址： <https://reservation.weihanli.xyz/Admin/Account/Login>

后台登录账号：管理员用户名:admin ,密码:Admin888

## Architecture

前端：Bootstrap + layer + Angularjs + jquery-ui + KindEditor

后端：EntityFramework + log4net + 三层架构 

## Function

1. 活动室预约
1. 预约管理
1. 活动室管理
1. 公告管理
1. 用户管理
1. 预约黑名单管理
1. 系统设置管理
1. 某段时间段禁用预约，如节假日/寒（暑）假等

## Todo

- [ ] 预约时间段修改，与所要预约的活动室进行关联
- [ ] 预约提交前验证改为 Google 人机验证服务
- [ ] 微信小程序预约
- [ ] 微信公共号预约


## Summary

目前还有一些功能还未开发，还需要进一步的开发和完善，如果有兴趣欢迎加入。

联系我：<weihanli@outlook.com>
