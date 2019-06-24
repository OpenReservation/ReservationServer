# ActivityReservation [![Build Status](https://weihanli.visualstudio.com/Pipelines/_apis/build/status/WeihanLi.ActivityReservation?branchName=dev)](https://weihanli.visualstudio.com/Pipelines/_build/latest?definitionId=7?branchName=dev)

## Intro

活动室预约系统，起初的设计和开发是因为学校活动室预约流程希望从之前繁琐低效的完全线下预约
修改为线上预约+线下盖章审批的方式来预约学校的活动室。

目前使用 ASP.NET Core 开发, 使用 Docker + k8s + nginx 部署，

演示地址：<https://reservation.weihanli.xyz>
新版预览版演示地址：<https://reservation-preview.weihanli.xyz>

后台登录地址： <https://reservation.weihanli.xyz/Admin/Account/Login>

后台登录账号：

管理员用户名: admin 密码: Admin888
普通用户： Alice 密码：Test1234

管理员有更多的权限，可以设置更多系统相关的配置，也可以增加系统普通管理员

## 关于技术

使用的技术演化：

ASP.NET WebForm => ASP.NET MVC => ASP.NET Core

部署方式：

IIS => `Docker`+`nginx` => `Docker`+`kubernetes`+`nginx`

CI/CD:

appveyor => travis => Azure Pipeline

## Roadmap

Check it [here](./Roadmap.md)
