# 活动室预约单机部署

## Intro

活动室预约项目单机版是基于 asp.net core 2.2 的，支持 docker，支持跨平台部署，单机版对应 standalone 分支，
单机版默认使用 Sqlite 数据库，数据库访问基于 Entity Framework Core，大多数数据库都支持，如果要使用别的数据库，可以自行修改源代码，换成相应数据库的 Provider 就可以了。

## Docker 部署

支持通过 docker 部署

``` bash
docker pull weihanli/activityreservation:standalone #拉取最新的单机版镜像

docker run -d -p 8900:80 --name=reservation weihanli/activityreservation:standalone # 运行容器
```

单机版使用 sqlite 数据库，你可以通过挂载把数据库放在本地，以免 docker 容器异常退出或重启造成数据丢失。

``` bash

docker run -d -p 8900:80 --name=reservation -v ./sqlite.db:/app/reservation.db weihanli/activityreservation:standalone # 挂载 db 运行容器

docker run -d -p 8900:80 --name=reservation -v ./appsettings.json:/app/appsettings.production.json weihanli/activityreservation:standalone # 挂载 appsettings.production.json 运行容器

docker run -d -p 8900:80 --name=reservation -v ./appsettings.json:/app/appsettings.json weihanli/activityreservation:standalone # 挂载 appsettings.json 运行容器
```

## 源码编译

源码编译需要安装 dotnet core 2.2 sdk 以及 git

``` bash
git clone https://github.com/WeihanLi/ActivityReservation.git

cd ActivityReservation

git checkout standalone

# 发布
cd ActivityReservation
dotnet publish -c Release -o out
```
