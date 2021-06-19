# 活动室预约单机部署

## Intro

活动室预约项目单机版是基于 asp.net core 3.1 的，支持 docker，支持跨平台部署，单机版对应 standalone 分支，
单机版默认使用 Sqlite 数据库，数据库访问基于 Entity Framework Core，大多数数据库都支持，如果要使用别的数据库，可以自行修改源代码，换成相应数据库的 Provider 就可以了。

## Docker 部署(**推荐**)

支持通过 docker 部署，docker 安装可以参考官方文档介绍，详细参考：<https://docs.docker.com/engine/install/>

``` bash
docker rmi weihanli/ActivityReservation:standalone #删除旧镜像

docker run -d -p 8900:80 --name=reservation weihanli/activityreservation:standalone # 运行容器
```

单机版使用 sqlite 数据库，你可以通过挂载把数据库放在本地，以免 docker 容器异常退出或重启造成数据丢失。

``` bash

docker run -d -p 8900:80 --name=reservation -v ./sqlite.db:/app/reservation.db weihanli/activityreservation:standalone # 挂载 db 运行容器

docker run -d -p 8900:80 --name=reservation -v ./appsettings.json:/app/appsettings.production.json weihanli/activityreservation:standalone # 挂载 appsettings.production.json 运行容器

docker run -d -p 8900:80 --name=reservation -v ./appsettings.json:/app/appsettings.json weihanli/activityreservation:standalone # 挂载 appsettings.json 运行容器
```

## 源码编译

源码编译需要安装 dotnet core 3.1 sdk 以及 git

``` bash
git clone https://github.com/OpenReservation/ReservationServer.git

cd ReservationServer

git checkout standalone

cd ActivityReservation

# 发布
dotnet publish -c Release -o out
```

## 独立部署

### .NET SDK

根据自己的系统下载安装 .NET Core 3.1 SDK，下载地址：<https://dotnet.microsoft.com/download/dotnet/3.1>，在 Linux 上安装可以参考微软的文档 <https://docs.microsoft.com/en-us/dotnet/core/install/linux>

安装好之后，可以使用 `dotnet --info` 来测试是否安装成功，如果没有报错就安装成功啦

### 打包部署

如果要独立部署，需要自己手动按照上面源码编译的方式进行打包，在服务器上打包出来最后用于部署的文件上面的 `out` 目录中的文件，将打包后的文件放在合适的目录下，在这个目录下运行下面的命令即可

``` sh
dotnet ActivityReservation.dll
```

默认会使用 `80` 端口，如果端口不可用或者希望使用别的端口可以在执行命令时指定端口号，使用示例如下：

``` sh
dotnet ActivityReservation.dll  --urls="http://*:8080"
```

### 守护进程

通常独立部署需要有一个守护进程服务来保证自己的应用不会意味退出，守护进程服务主要就是保证我们的服务意外退出的时候可以及时恢复。

使用 docker 的话只要 docker 可以充当我们的守护进程服务，如果不用 docker 的话通常需要我们自己配置，可以将应用注册为一个系统服务(如 SystemD、Windows 服务【Windows 可以部署到 IIS】)，或者使用第三方的服务如 Supervisor 守护运行，可以参考后面的链接

## More

通常我们会在服务暴露到公网之前加一个反向代理服务器如 Nginx，因为我们 .NET Core 自带的服务器是比较简单的，没有那么丰富的功能，需要 Nginx 来提供更多的防护和配置，可以参考微软文档上的介绍，后面的链接中有介绍的

## References

- <https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/>

- <https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-5.0>

- <https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-5.0>

- <https://docs.microsoft.com/en-us/dotnet/core/install/linux>

- [CentOS 安装 docker](https://www.cnblogs.com/weihanli/p/14028047.html)

- [Asp.NET Core轻松学-部署到IIS进行托管](https://www.cnblogs.com/viter/p/10388902.html)

- ###### [Asp.NET Core轻松学-部署到Linux进行托管](https://www.cnblogs.com/viter/p/10408012.html)

- ###### [Asp.NET Core轻松学-使用Supervisor进行托管部署](https://www.cnblogs.com/viter/p/10441409.html)

- ###### [Asp.NET Core轻松学-使用Docker进行容器化托管](https://www.cnblogs.com/viter/p/10463907.html)
