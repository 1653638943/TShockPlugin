# ConsoleSql

- 作者: Cai
- 出处: 本仓库
- 允许你使用在控制台和BOT中执行SQL语句   
![image](https://github.com/ACaiCat/TShockPlugin/assets/62058454/e0d24d17-cea7-49b3-9172-0b8d49e3e23f)

## 示例
```
列出Tshock的数据表名：  
sql select name from sqlite_master where type='table'  
查询“用户数据表”有什么：  
sql select * from users  
修改ID为2的用户角色名字为熙恩（帮强制开荒玩家换角色名）：  
sql update users set username='熙恩' where id=2   
```

## 配置

```json
无
```
## 命令

| 命令名           |        权限         |        说明         |
| -------------- | :----------------- | :-----------------: 
| /sql <SQL语句>|ConsoleSql.Use |执行SQL


## 更新日志

```
暂无
```

## 反馈
- 共同维护的插件库：https://github.com/Controllerdestiny/TShockPlugin
- 国内社区trhub.cn 或 TShock官方群等
