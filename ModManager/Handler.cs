using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;

namespace MonBazou_ModManager
{
    internal class Handler
    {
        public static string InstallLoc { get; set; }

        public static void Init()
        {
            GetInstallLoc();
        }

        public static void GetInstallLoc()
        {
            try
            {
                using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    RegistryKey? registryKey2 = null;
                    registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 1520370");
                    object value = registryKey2.GetValue("InstallLocation");
                    if (!string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        InstallLoc = value.ToString() + "\\";
                    } else
                    {
                        throw new Exception("Couldn't Auto-Locate Install Folder!");
                    }
                    DialogResult result = MessageBox.Show(InstallLoc + "\nIs this the correct location of your game?", "Mod Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(result == DialogResult.No && !string.IsNullOrWhiteSpace(InstallLoc))
                    {
                        throw new Exception("Wrong Folder");
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nPlease select your Game's Install Location in the Folder Browser!", "Mod Manager");
                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Select Mon Bazou Folder!";

                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string[] files = Directory.GetFiles(fbd.SelectedPath);

                        if (File.Exists(fbd.SelectedPath + "\\Mon Bazou.exe"))
                        {
                            InstallLoc = fbd.SelectedPath.ToString();
                            string createText = fbd.SelectedPath.ToString();
                            File.WriteAllText("dir.txt", createText);
                            InstallLoc = fbd.SelectedPath.ToString() + "\\";
                            return;
                        } else
                        {
                            MessageBox.Show("You selected the wrong folder!");
                            Application.Exit();
                            Application.ExitThread();
                            return;
                        }
                    }
                }
            }
        }

        public async static Task<string> GetChangelog()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    return await client.GetStringAsync(Constants.changelogUrl);
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Error while retrieving changelog! \n" + ex.ToString(), "Mod Manager - Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "Couldn't load changelog!";
            }
        }

        public static List<Mod> GetModListData()
        {
            var client = new WebClient();
            string modResponse = client.DownloadString(Constants.dbUrl);

            try
            {
                var modList = JsonConvert.DeserializeObject<List<Mod>>(modResponse);
                return modList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading Mod List \n" + ex.ToString(), "Mod Manager - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                Application.ExitThread();
                return new List<Mod>();
            }
        }
    }
}
