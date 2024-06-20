﻿using Newtonsoft.Json;
using System.IO.Compression;
using System.Reflection;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace AutoUpdatePlugin;

[ApiVersion(2, 1)]
public class Plugin : TerrariaPlugin
{
    public override string Name => "AutoUpdatePlugin";

    public override Version Version => new(2024, 6, 20, 2);

    public override string Author => "少司命，Cai";

    public override string Description => "自动更新你的插件！";

    private const string ReleaseUrl = "https://github.com/Controllerdestiny/TShockPlugin/releases/download/V1.0.0.0/Plugins.zip";

    private const string PUrl = "https://github.moeyy.xyz/";

    private const string PluginsUrl = "https://raw.githubusercontent.com/Controllerdestiny/TShockPlugin/master/Plugins.json";

    private static readonly HttpClient _httpClient = new();

    private const string TempSaveDir = "TempFile";

    private const string TempZipName = "Plugins.zip";

    public Plugin(Main game) : base(game)
    {

    }

    public override void Initialize()
    {
        Commands.ChatCommands.Add(new("AutoUpdatePlugin", CheckCmd, "cplugin"));
        Commands.ChatCommands.Add(new("AutoUpdatePlugin", UpdateCmd, "uplugin"));
        ServerApi.Hooks.GamePostInitialize.Register(this, AutoCheckUpdate,int.MinValue); //最低优先级
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {

            ServerApi.Hooks.GamePostInitialize.Deregister(this, AutoCheckUpdate);
        }

        base.Dispose(disposing);
    }

    private void AutoCheckUpdate(EventArgs args)
    {
        Task.Run(() =>
        {
            try
            {
                Task.Delay(5000).Wait();
                TShock.Log.ConsoleInfo("[AutoUpdate]开始检查更新...");
                var updates = GetUpdate();
                if (updates.Count == 0)
                {
                    TShock.Log.ConsoleInfo("[AutoUpdate]你的插件全是最新版本，无需更新哦~");
                    return;
                }
                TShock.Log.ConsoleInfo("[以下插件有新的版本更新]\n" + string.Join("\n", updates.Select(i => $"[{i.Name}] V{i.OldVersion} >>> V{i.NewVersion}")));
                TShock.Log.ConsoleInfo("你可以使用命令/uplugin更新插件哦~");
            }
            catch (Exception ex)
            {
                TShock.Log.ConsoleInfo("[AutoUpdate]无法获取更新:" + ex.Message);
                return;
            }
        });
    }

    private void UpdateCmd(CommandArgs args)
    {
        try
        {
            var updates = GetUpdate();
            if (updates.Count == 0)
            {
                args.Player.SendSuccessMessage("你的插件全是最新版本，无需更新哦~");
                return;
            }
            args.Player.SendInfoMessage("正在下载最新插件包...");
            DownLoadPlugin();
            args.Player.SendInfoMessage("正在解压插件包...");
            ExtractDirectoryZip();
            args.Player.SendInfoMessage("正在升级插件...");
            var success = UpdatePlugin(updates);
            if (success.Count == 0)
            {
                args.Player.SendSuccessMessage("更新了个寂寞~");
                return;
            }
            args.Player.SendSuccessMessage("[更新完成]\n" + string.Join("\n", success.Select(i => $"[{i.Name}] V{i.OldVersion} >>> V{i.NewVersion}")));
            args.Player.SendSuccessMessage("重启服务器后插件生效!");
        }
        catch (Exception ex)
        {
            args.Player.SendErrorMessage("自动更新出现错误:" + ex.Message);
            return;
        }
    }

    private void CheckCmd(CommandArgs args)
    {
        try
        {
            var updates = GetUpdate();
            if (updates.Count == 0)
            {
                args.Player.SendSuccessMessage("你的插件全是最新版本，无需更新哦~");
                return;
            }
            args.Player.SendInfoMessage("[以下插件有新的版本更新]\n" + string.Join("\n", updates.Select(i => $"[{i.Name}] V{i.OldVersion} >>> V{i.NewVersion}")));
        }
        catch (Exception ex)
        {
            args.Player.SendErrorMessage("无法获取更新:" + ex.Message);
            return;
        }
    }

    #region 工具方法
    private static List<PluginUpdateInfo> GetUpdate()
    {
        var plugins = GetPlugins();
        HttpClient httpClient = new();
        var response = httpClient.GetAsync(PUrl + PluginsUrl).Result;

        if (!response.IsSuccessStatusCode)
            throw new Exception("无法连接服务器");
        var json = response.Content.ReadAsStringAsync().Result;
        var latestPluginList = JsonConvert.DeserializeObject<List<PluginVersionInfo>>(json) ?? new();
        List<PluginUpdateInfo> pluginUpdateList = new();
        foreach (var latestPluginInfo in latestPluginList)
            foreach (var plugin in plugins)
                if (plugin.Name == latestPluginInfo.Name && plugin.Version != latestPluginInfo.Version)
                    pluginUpdateList.Add(new PluginUpdateInfo(plugin.Name, plugin.Author, latestPluginInfo.Version, plugin.Version, plugin.Path, latestPluginInfo.Path));
        return pluginUpdateList;
    }

    private static List<PluginVersionInfo> GetPlugins()
    {
        List<PluginVersionInfo> plugins = new();
        //获取已安装的插件，并且读取插件信息和AssemblyName
        foreach (var plugin in ServerApi.Plugins) 
        {
            plugins.Add(new PluginVersionInfo()
            {
                AssemblyName = plugin.Plugin.GetType().Assembly.GetName().Name!,
                Author = plugin.Plugin.Author,
                Name = plugin.Plugin.Name,
                Description = plugin.Plugin.Description,
                Version = plugin.Plugin.Version.ToString()
            });
        }
        //反射拯救了TSAPI
        var type = typeof(ServerApi);
        var field = type.GetField("loadedAssemblies", BindingFlags.NonPublic | BindingFlags.Static)!;
        var loadedAssemblies = (Dictionary<string, Assembly>)field.GetValue(null)!;
        foreach (var loadedAssembly in loadedAssemblies)
            for (int i = 0; i < plugins.Count; i++)
                if (plugins[i].AssemblyName == loadedAssembly.Value.GetName().Name)
                    plugins[i].Path = loadedAssembly.Key + ".dll"; 
        return plugins;
    }


    private static void DownLoadPlugin()
    {
        DirectoryInfo directoryInfo = new(TempSaveDir);
        if (!directoryInfo.Exists)
            directoryInfo.Create();
        HttpClient httpClient = new();
        var zipBytes = httpClient.GetByteArrayAsync(PUrl + ReleaseUrl).Result;
        File.WriteAllBytes(Path.Combine(directoryInfo.FullName, TempZipName), zipBytes);
    }

    private static void ExtractDirectoryZip()
    {
        DirectoryInfo directoryInfo = new(TempSaveDir);
        ZipFile.ExtractToDirectory(Path.Combine(directoryInfo.FullName, TempZipName), Path.Combine(directoryInfo.FullName, "Plugins"), true);
    }

    private static List<PluginUpdateInfo> UpdatePlugin(List<PluginUpdateInfo> pluginUpdateInfos)
    {
        for (int i = pluginUpdateInfos.Count - 1; i >= 0; i--)
        {
            var pluginUpdateInfo = pluginUpdateInfos[i];
            string sourcePath = Path.Combine(TempSaveDir, "Plugins", pluginUpdateInfo.RemotePath);
            string destinationPath = Path.Combine(ServerApi.ServerPluginsDirectoryPath, pluginUpdateInfo.LocalPath);
            // 确保目标目录存在
            string destinationDirectory = Path.GetDirectoryName(destinationPath)!;
            if (File.Exists(destinationPath))
            {
                File.Copy(sourcePath, destinationPath, true);
            }
            else
            {
                TShock.Log.ConsoleWarn($"[跳过更新]无法在本地找到插件{pluginUpdateInfo.Name}({destinationPath}),可能是云加载或使用-additionalplugins加载");
                pluginUpdateInfos.RemoveAt(i);  // 移除元素
            }
        }

        if (Directory.Exists(TempSaveDir))
            Directory.Delete(TempSaveDir, true);

        return pluginUpdateInfos;
    }
    #endregion
}
