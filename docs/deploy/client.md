# 活动室预约客户端部署

## Intro

从 2.0 版本开始，开始了一些完全前后端分离的尝试，前端使用 angular + material，服务器端是 asp.net core webapi

## Docker 部署

支持通过 docker 部署

``` bash
docker pull weihanli/reservation-client:latest #拉取最新的单机版镜像

docker run -d -p 9000:80 --name=reservation-client weihanli/reservation-client:latest # 运行容器
```

容器启动成功之后，访问 `http://localhost:9000` 即可

## Kubernetes 部署

### 使用 helm

推荐使用 helm 安装

``` bash
helm repo add apphub https://apphub.aliyuncs.com/
helm install reservation apphub/reservation-server
```

这里默认使用的是 NodePort 暴露端口，默认端口号是 31256，在浏览器中访问：`http://<nodeip>>:31256`，

如果修改了端口请换成自己的端口号或上面获得到的端口号，可以通过 `service.nodePort` 来设置端口

```shell
helm install reservation-client --set service.nodePort=32156 reservation-client
```

如果不是使用 Node-Port 方式访问的，可以通过 `kubectl port-forward svc/<svcName> 31256:80` 来创建一个端口转发，然后访问 `http://localhost:31256` 来访问应用

### 不使用 helm

不使用 helm，也可以直接使用下面的命令来安装

``` bash
kubectl apply -f https://raw.githubusercontent.com/WeihanLi/ActivityReservation/dev/ActivityReservation.Clients/ReservationClient/k8s-deploy.yaml
```

提供了一个部署在我的 k8s 上的在线示例的 <https://reservation-client.weihanli.xyz>
