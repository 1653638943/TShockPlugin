# Ezperm 便捷权限

- 作者: 大豆子,肝帝熙恩优化1449
- 出处: TShock中文官方群
- 一个指令帮助小白给初始服务器添加缺失的权限，还可以批量添删权限
- 其实你也可以直接/group addperm 组名字 权限1 权限2 权限3

## 更新日志

```
暂无
```

## 指令

| 语法           |        权限         |   说明   |
| -------------- | :-----------------: | :------: |
| /inperms 或 /批量改权限 |  inperms.admin  | 批量改权限|

## 配置

```json
{
  "Groups": [
    {
      "组名字": "default",
      "添加的权限": [
        "tshock.world.movenpc",
        "tshock.tp.pylon",
        "tshock.tp.rod",
        "tshock.npc.startdd2",
        "tshock.tp.wormhole",
        "tshock.npc.summonboss",
        "tshock.npc.startinvasion",
        "tshock.world.time.usesundial"
      ],
      "删除的权限": [
        "tshock.admin"
      ]
    }
  ]
}
```
## 反馈
- 共同维护的插件库：https://github.com/Controllerdestiny/TShockPlugin
- 国内社区trhub.cn 或 TShock官方群等