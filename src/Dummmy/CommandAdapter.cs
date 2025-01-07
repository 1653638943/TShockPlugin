﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace Dummmy;
internal class CommandAdapter
{
    private static readonly Dictionary<string, CommandDelegate> _actions = new()
    {
        { "remove", RemoveDummmy },
        { "list", DummmyList },
        { "reconnect", ReConnect }
    };

    public static void Adapter(CommandArgs args)
    {
        if (args.Parameters.Count >= 1)
        {
            var subcmd = args.Parameters[0].ToLower();
            if (_actions.TryGetValue(subcmd, out var action))
            {
                action(args);
                return;
            }
        }
        args.Player.SendInfoMessage(GetString("dummmy remove [index] 移除目标假人"));
        args.Player.SendInfoMessage(GetString("dummmy list 假人列表"));
        args.Player.SendInfoMessage(GetString("dummmy reconnect [index] 重新连接"));
    }

    private static void ReConnect(CommandArgs args)
    {
        if (args.Parameters.Count == 2 && int.TryParse(args.Parameters[1], out var index))
        {
            var ply = Plugin._players[index];
            if (ply != null && !ply.connected)
            {
                ply.GameLoop("127.0.0.1", Plugin.Port, TShock.Config.Settings.ServerPassword);
            }
            else
            {
                args.Player.SendErrorMessage(GetString("目标假人不存在!"));
            }
        }
        else
        {
            args.Player.SendErrorMessage(GetString("请输入正确的序号!"));
        }
    }

    private static void DummmyList(CommandArgs args)
    {
        for (var i = 0; i < Plugin._players.Length; i++)
        { 
            var player = Plugin._players[i];
            if (player != null && player.TSPlayer.Active)
            { 
                args.Player.SendInfoMessage(GetString($"[{i}].{player.TSPlayer.Name} 连接状态: {player.connected}"));
            }
        }
    }

    private static void RemoveDummmy(CommandArgs args)
    {
        if (args.Parameters.Count == 2 && int.TryParse(args.Parameters[1], out var index))
        {
            var ply = Plugin._players[index];
            if (ply != null && ply.TSPlayer.Active)
            {
                ply.Close();
                args.Player.SendSuccessMessage(GetString("假人移除成功!"));
            }
            else
            {
                args.Player.SendErrorMessage(GetString("目标假人不存在!"));
            }
        }
        else
        {
            args.Player.SendErrorMessage(GetString("请输入正确的序号!"));
        }
    }
}
