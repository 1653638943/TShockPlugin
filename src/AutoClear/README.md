# Autoclear 智能自动扫地

- 作者: 大豆子[Mute适配1447]，肝帝熙恩更新
- 出处: TShock中文官方群
- 当地面物品数量到达一定值开始清扫倒计时
- 可自定义哪些/哪类物品不被清扫

## 指令

```
暂无
```

## 配置
> 配置文件位置：tshock/Autoclear.zh-CN.json
```json5
{
  "多久检测一次(s)": 10,
  "不清扫的物品ID列表": [],
  "智能清扫数量临界值": 100,
  "延迟清扫(s)": 10,
  "延迟清扫自定义消息": "",
  "是否清扫挥动武器": true,
  "是否清扫投掷武器": true,
  "是否清扫普通物品": true,
  "是否清扫装备": true,
  "是否清扫时装": true,
  "完成清扫自定义消息": "",
  "具体消息": true
}
```

## 更新日志

### v1.1.0
- 存在事件或BOSS时跳过清理
- 修正英文配置文件的错别字 `SweepRegaular` -> `SweepRegular`
- 代码清理

### v1.0.7
- 卸载函数
### v1.0.6
- 准备更新TS 5.2.1,修正文档，初始配置内容更改
### v1.0.4
- 添加英文翻译

## 反馈
- 优先发issued -> 共同维护的插件库：https://github.com/UnrealMultiple/TShockPlugin
- 次优先：TShock官方群：816771079
- 大概率看不到但是也可以：国内社区trhub.cn ，bbstr.net , tr.monika.love
