using Microsoft.Win32;
using Newtonsoft.Json;
using MonBazou_ModManager.Model;

namespace MonBazou_ModManager;

internal static class Handler
{
    private static string InstallLoc { get; set; } = null!;

    public static void Init()
    {
        GetInstallLoc();
    }

    private static void GetInstallLoc()
    {
        try
        {
            using var registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 1520370");
            if (registryKey2 != null)
            {
                var value = registryKey2.GetValue("InstallLocation");
                if (!string.IsNullOrWhiteSpace(value?.ToString()))
                {
                    InstallLoc = value + "\\";
                }
                else
                {
                    throw new Exception(Resources.Handler.Could_Not_Locate_Install_Folder);
                }
            }
            else
            {
                throw new Exception(Resources.Handler.Could_Not_Locate_Install_Folder);
            }

            var result = MessageBox.Show(InstallLoc + Resources.Handler.Correct_Location, Resources.Handler.Mod_Manager, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.No && !string.IsNullOrWhiteSpace(InstallLoc))
            {
                throw new Exception(Resources.Handler.Wrong_Folder);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message + "\n" + Resources.Handler.Select_Game_Install_Location, Resources.Handler.Mod_Manager);
            using var fbd = new FolderBrowserDialog();
            fbd.Description = Resources.Handler.Select_Mon_Bazou_Folder;

            var result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                if (File.Exists(fbd.SelectedPath + "\\Mon Bazou.exe"))
                {
                    InstallLoc = fbd.SelectedPath;
                    var createText = fbd.SelectedPath;
                    File.WriteAllText("dir.txt", createText);
                    InstallLoc = fbd.SelectedPath + "\\";
                }
                else
                {
                    MessageBox.Show(Resources.Handler.You_Have_Selected_The_Wrong_Folder);
                    Application.Exit();
                    Application.ExitThread();
                }
            }
        }
    }

    public static async Task<string> GetChangelog()
    {
        try
        {
            using var client = new HttpClient();
            return await client.GetStringAsync(Constants.ChangelogUrl);
        }
        catch (Exception ex)
        {
            MessageBox.Show(Resources.Handler.Error_While_Retrieving_Changelog + "\n" + ex, Resources.Handler.Mod_Manager + " - " + Resources.Handler.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return Resources.Handler.Error_While_Retrieving_Changelog;
        }
    }

    public static async Task<List<Mod>> GetModListData()
    {
        var client = new HttpClient();
        var modResponse = await client.GetStringAsync(Constants.DbUrl);

        try
        {
            var modList = JsonConvert.DeserializeObject<List<Mod>>(modResponse);
            return modList ?? new List<Mod>();
        }
        catch (Exception ex)
        {
            MessageBox.Show(Resources.Handler.Error_While_Loading_Mod_List + "\n" + ex, Resources.Handler.Mod_Manager + " - " + Resources.Handler.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
            Application.ExitThread();
            return new List<Mod>();
        }
    }
}