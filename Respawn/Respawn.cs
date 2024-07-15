using System.Collections.Concurrent;
using Microsoft.Xna.Framework;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace Respawn;

[ApiVersion(2, 1)]
public class Respawn : TerrariaPlugin
{
    public override string Author => "leader，肝帝熙恩";
    public override string Description => "原地复活";
    public override string Name => "Respawn";
    public override Version Version => new Version(2, 0, 0, 0);

    private readonly ConcurrentDictionary<TSPlayer, Vector2> DeadPos = new();//线程安全的dic


    public Respawn(Main game) : base(game)
    {
    }

    public override void Initialize()
    {
        GetDataHandlers.KillMe.Register(OnKillMe);
        GetDataHandlers.PlayerSpawn.Register(OnSpawn);
    }

    private void OnSpawn(object? sender, GetDataHandlers.SpawnEventArgs e)
    {
        if (DeadPos.TryGetValue(e.Player, out var pos))
        {
            Task.Run(async () =>
            {
                await Task.Delay(100);
                e.Player.Teleport(pos.X, pos.Y);
                DeadPos.Remove(e.Player, out var _);
            });
        }
    }
        

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            GetDataHandlers.KillMe.UnRegister(OnKillMe);
            GetDataHandlers.PlayerSpawn.UnRegister(OnSpawn);
        }
        base.Dispose(disposing);
    }

    private void OnKillMe(object? sender, GetDataHandlers.KillMeEventArgs e)
    {
        if(e.Handled) return;
        DeadPos[e.Player] = e.Player.TPlayer.position;
    }

   
}
