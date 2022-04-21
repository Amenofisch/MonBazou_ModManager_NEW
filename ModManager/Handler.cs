using Microsoft.Win32;
using Newtonsoft.Json;
using MonBazou_ModManager.Model;

namespace MonBazou_ModManager;

internal class Handler
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
                    throw new Exception("Couldn't Auto-Locate Install Folder!");
                }
            }
            else
            {
                throw new Exception("Couldn't Auto-Locate Install Folder!");
            }

            var result = MessageBox.Show(InstallLoc + "\nIs this the correct location of your game?", "Mod Manager", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.No && !string.IsNullOrWhiteSpace(InstallLoc))
            {
                throw new Exception("Wrong Folder");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message + "\nPlease select your Game's Install Location in the Folder Browser!", "Mod Manager");
            using var fbd = new FolderBrowserDialog();
            fbd.Description = "Select Mon Bazou Folder!";

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
                    MessageBox.Show("You selected the wrong folder!");
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
            MessageBox.Show("Error while retrieving changelog! \n" + ex, "Mod Manager - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return "Couldn't load changelog!";
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
            MessageBox.Show("Error while loading Mod List \n" + ex, "Mod Manager - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
            Application.ExitThread();
            return new List<Mod>();
        }
    }
}