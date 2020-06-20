# OpenReservation [![Build Status](https://weihanli.visualstudio.com/Pipelines/_apis/build/status/OpenReservation.ReservationServer?branchName=dev)](https://weihanli.visualstudio.com/Pipelines/_build/latest?definitionId=7&branchName=dev)

## Intro

活动室预约系统，起初的设计和开发是因为学校活动室预约流程希望从之前繁琐低效的完全线下预约
修改为线上预约+线下盖章审批的方式来预约学校的活动室。

目前使用 ASP.NET Core 开发, 使用 Docker + k8s + nginx 部署，

- 演示地址：<https://reservation.weihanli.xyz>
- 新版预约客户端演示地址：<https://reservation-client.weihanli.xyz>  (angular9 +material SPA)
- 小程序演示 demo:

  ![wxAppCode](./images/wxAppCode.jpg)

- 后台登录地址： <https://reservation.weihanli.xyz/Admin/>

  后台登录账号：

  管理员用户名: admin 密码: Admin@888

  普通用户： Alice 密码：Test@1234

  管理员有更多的权限，可以设置更多系统相关的配置，也可以增加系统普通管理员

## 关于技术

使用的技术演化：

ASP.NET WebForm => ASP.NET MVC => ASP.NET Core

部署方式：

IIS => `Docker`+`nginx` => `kubernetes`+`nginx`

CI/CD:

appveyor => travis => Azure Pipeline

[部署文档](./docs/README.md)

## Roadmap

### 1.0

- [x] 活动室预约
- [x] 预约管理
- [x] 活动室管理
- [x] 公告管理
- [x] 用户管理
- [x] 预约黑名单管理
- [x] 系统设置管理
- [x] 某段时间段禁用预约，如节假日/寒（暑）假等

### 2.0

- [x] 活动室预约 SPA <https://github.com/OpenReservation/ReservationServer/tree/dev/OpenReservation.Clients/ReservationClient>（angular8 + material)
- [x] 微信小程序预约 <https://github.com/OpenReservation/ReservationServer/tree/dev/OpenReservation.Clients/WxAppClient>
- [x] 从单机到集群，详细修改参考：<https://www.cnblogs.com/weihanli/p/aspnetcore-migrate-standalone-to-cluster.html>

### 3.0

- [x] 多语言支持
- [x] 用户系统（需要登录才能预约，登录支持 Github 登录）
- [x] 我的预约记录
- [ ] 取消预约
- [ ] 更通用的预约流程，待优化

### 4.0

- [ ] 增加组织的概念，多租户
- [ ] ReservationService as a Service，打造 Saas 预约服务平台

## Contact

Contact me if you need: <weihanli@outlook.com>
