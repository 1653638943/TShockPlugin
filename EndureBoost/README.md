# EndureBoost 物品一定数量后长时间buff

- 作者: 肝帝熙恩
- 出处: 无
- 当玩家背包某些物品到达一定数量后，给与指定buff

## 更新日志

```
暂无
```

## 指令

| 语法           |        权限         |   说明   |
| -------------- | :-----------------: | :------: |
| /ebbuff，/ldbuff，/loadbuff"  | 立即刷新长时间buff状态|

## 配置

```json
{
  "猪猪储钱罐": false,// 示例
  "保险箱": false,
  "护卫熔炉": false,
  "虚空宝藏袋": true,
  "持续时间(s)": 3600,
  "药水": [
    {
      "药水id": [
        288,
        289
      ],//可以是一个数组，也就是里面可以放单个或者多个物品id
      "药水数量": 30
    },
    {
      "药水id": [
        290
      ],
      "药水数量": 200
    }
  ],
  "其他物品": [
    {
      "物品id": [
        2,
        3
      ],
      "物品数量": 3,
      "给buff的id": 87
    },
    {
      "物品id": [
        5
      ],
      "物品数量": 3,
      "给buff的id": 89
    }
  ]
}
```

## 反馈
- 共同维护的插件库：https://github.com/Controllerdestiny/TShockPlugin
- 国内社区trhub.cn 或 TShock官方群等