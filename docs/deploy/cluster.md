# 活动室预约单机部署

## Intro

活动室预约系统分单机版和集群版，主要致力于集群版的开发，单机版是为了小型系统使用，使用的是 sqlite 数据库，不支持集群模式，集群版增加了 redis 缓存中间件，通过 redis 实现了一些分布式系统必备的组件，分布式锁，基于 redis pub/sub 的 RedisEventBus，计数器等。

在线示例：

- 新版活动室预约 <https://reservation-client.weihanli.xyz/>（angular8 + asp.net core api)
- 后台管理 <https://reservation.weihanli.xyz/Admin/Account/Login>

## 部署需要

- redis
- mysql
- elasticsearch(日志，可以不用，但是推荐使用)
- [sentry](https://sentry.io)（异常日志报警）
- [腾讯验证码](https://007.qq.com/product.html?ADTAG=index.head)

