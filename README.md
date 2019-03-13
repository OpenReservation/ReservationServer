# ActivityReservation [![Build Status](https://weihanli.visualstudio.com/Pipelines/_apis/build/status/WeihanLi.ActivityReservation?branchName=dev)](https://weihanli.visualstudio.com/Pipelines/_build/latest?definitionId=7?branchName=dev)

## Intro

活动室预约系统，起初的设计和开发是因为学校活动室预约流程希望从之前繁琐低效的完全线下预约
修改为线上预约+线下盖章审批的方式来预约学校的活动室。

原本是用 ASP.NET WebForm 写的，后来用 ASP.NET MVC 重写一下并增加一些功能，最近迁移到 asp.net core 重新上线

演示地址：<https://reservation.weihanli.xyz>

后台登录地址： <https://reservation.weihanli.xyz/Admin/Account/Login>

后台登录账号：

管理员用户名: admin 密码: Admin888
普通用户： Alice 密码：Test1234

管理员有更多的权限，可以设置更多系统相关的配置，也可以增加系统普通管理员

演示地址的部署方式是通过 Docker 部署在我的虚拟上的，网站前面有个 Nginx 作为反向代理，现在已经配置了 Azure pipeline 可以自动化的 build docker 镜像并 push 镜像到 docker hub，之后 SSH 到虚拟机上，重新部署到 docker。

## Roadmap

Check it [here](./Roadmap.md)