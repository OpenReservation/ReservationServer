# ReservationClient

活动室预约 SPA 客户端，使用 angular 9 + material 开发，支持部署到 docker 以及 k8s

## Docker

``` bash
docker run -d -p 8081:80 weihanli/reservation-client:latest
```

## Kubernetes

``` bash
kubectl apply -f k8s-deploy.yaml
```

## More

活动室预约服务器端：<https://github.com/WeihanLi/ActivityReservation>
