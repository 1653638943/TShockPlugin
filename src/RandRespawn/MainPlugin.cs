﻿using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace RandReSpawn;

[ApiVersion(2, 1)]
public class MainPlugin : TerrariaPlugin
{
    private Random rand = new();
    public override string Name => "RandRespawn";
    public override string Author => "1413,肝帝熙恩适配1449";
    public override string Description => "随机出生点，任何回到出生点的操作都会被随机传送";
    public override Version Version => new Version(1, 0, 1);
    public MainPlugin(Main game) : base(game)
    {

    }
    public override void Initialize()
    {
        this.rand = new Random();
        GetDataHandlers.PlayerSpawn.Register(this.OnSpawn, HandlerPriority.Lowest);
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            GetDataHandlers.PlayerSpawn -= this.OnSpawn;
        }
        base.Dispose(disposing);
    }
    private void OnSpawn(object sender, GetDataHandlers.SpawnEventArgs args)
    {
        var (x, y) = this.Search();
        args.SpawnX = x;
        args.SpawnY = y;
        Task.Run(() => { try { Thread.Sleep(50); args.Player.Teleport(x * 16, y * 16);/*Console.WriteLine($"({x}, {y})");*/} catch { } });

    }
    private (int x, int y) Search()
    {
        int x, y;
        do
        {
            x = this.rand.Next(0, Main.maxTilesX - 2);
            y = this.rand.Next(0, Main.maxTilesY - 2);
        }
        while (!Avaiable(x, y));
        return (x, y);
    }
    private static bool Avaiable(int x, int y)
    {
        return
        !Main.tile[x + 0, y + 0].active() && !Main.tile[x + 1, y + 0].active() &&
        !Main.tile[x + 0, y + 1].active() && !Main.tile[x + 1, y + 1].active() &&
        !Main.tile[x + 0, y + 2].active() && !Main.tile[x + 1, y + 2].active() &&
        Main.tile[x + 0, y + 0].liquid == 0 && Main.tile[x + 1, y + 0].liquid == 0 &&
        Main.tile[x + 0, y + 1].liquid == 0 && Main.tile[x + 1, y + 1].liquid == 0 &&
        Main.tile[x + 0, y + 2].liquid == 0 && Main.tile[x + 1, y + 2].liquid == 0 &&
        Main.tile[x + 0, y + 3].active() && Main.tile[x + 1, y + 3].active();
    }
}