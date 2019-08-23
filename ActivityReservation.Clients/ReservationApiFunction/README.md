# 预约 API 云函数

## Intro

微信小程序的域名需要备案，但是没有大陆的服务器，而且觉得备案有些繁琐，起初做的小程序都有点想要放弃了，后来了解到腾讯云的云函数，于是利用腾讯云的云函数实现了一个简单的 API 网关，通过云函数来调用真正的 API 地址，借此来绕过域名备案的问题

## 实现原理

请求转发

## 实现过程中遇到的问题

- `unable to verify the first certificate`

这个是 https 请求的问题

设置了一个环境变量 `NODE_TLS_REJECT_UNAUTHORIZED=0` 来解决了
