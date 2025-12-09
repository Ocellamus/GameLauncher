using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using GameLauncher;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // 0. Initialize Steam API (optional - graceful if not available)
        bool steamInitialized = false;
        
        // 1. Locate the launcher and then the game EXE
        string launcherPath = Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
        string launcherDir = Path.GetDirectoryName(launcherPath) ?? string.Empty;
        
        try
        {
            // Try to read steam_appid.txt from the launcher directory or game directory
            // Check for steam_appid.txt in current directory or parent (game) directory
            string steamAppIdFile = Path.Combine(launcherDir, "steam_appid.txt");
            if (!File.Exists(steamAppIdFile))
            {
                DirectoryInfo parentDir = Directory.GetParent(launcherDir);
                if (parentDir != null)
                {
                    steamAppIdFile = Path.Combine(parentDir.FullName, "steam_appid.txt");
                }
            }

            // If steam_appid.txt exists, read the app ID and initialize Steam
            if (File.Exists(steamAppIdFile))
            {
                string appIdText = File.ReadAllText(steamAppIdFile).Trim();
                if (uint.TryParse(appIdText, out uint appId))
                {
                    steamInitialized = SteamManager.Initialize(appId);
                    
                    if (steamInitialized)
                    {
                        // Request current stats for achievements
                        SteamManager.RequestCurrentStats();
                    }
                }
            }
            else
            {
                // Try to initialize without app ID (will use existing steam_appid.txt if present)
                steamInitialized = SteamManager.Initialize();
                
                if (steamInitialized)
                {
                    SteamManager.RequestCurrentStats();
                }
            }
        }
        catch (Exception ex)
        {
            // Steam initialization is optional, so we just log and continue
            Console.WriteLine($"Steam API initialization skipped or failed: {ex.Message}");
        }

        // 2. Locate game directory and executable
        // Launcher is in ...\GameFolder\Launcher\Launcher.exe
        // Game is in   ...\GameFolder\Player.exe   (one folder up)
        DirectoryInfo parentDirInfo = Directory.GetParent(launcherDir);
        if (parentDirInfo == null)
        {
            MessageBox.Show(
                "Could not determine the game directory.",
                "game Launcher",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        string gameDir = parentDirInfo.FullName;
        string gameExe = Path.Combine(gameDir, "Player.exe"); // change name if needed

        if (!File.Exists(gameExe))
        {
            MessageBox.Show(
                "Game executable not found:\n" + gameExe,
                "game Launcher",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        bool changeAttempted = false;
        bool changeSucceeded = false;

        // 3. Show the custom optimization form
        using (var form = new GameLauncher.OprimizationForm())
        {
            DialogResult result = form.ShowDialog();

            if (result == DialogResult.Yes)
            {
                changeAttempted = true;

                try
                {
                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(
                               @"Software\Microsoft\DirectX\UserGpuPreferences"))
                    {
                        if (key == null)
                        {
                            throw new InvalidOperationException(
                                "Could not open or create the UserGpuPreferences registry key.");
                        }

                        string regName = gameExe;             // full path of the game
                        string regData = "GpuPreference=2;";  // high-performance GPU

                        key.SetValue(regName, regData, RegistryValueKind.String);
                        changeSucceeded = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Failed to update Windows graphics settings:\n" + ex.Message,
                        "game Launcher",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
        }

        // 4. Inform the user if changes were successfully applied
        if (changeAttempted && changeSucceeded)
        {
            MessageBox.Show(
                "Windows graphics settings were updated for game and the Merchant of Dreams." +
                Environment.NewLine + Environment.NewLine +
                "The game is now set to use the high-performance GPU where available.",
                "Settings Applied",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // 5. Launch the game (always)
        try
        {
            Process gameProcess = new Process
            {
                StartInfo = { FileName = gameExe }
            };

            gameProcess.Start();

            // Optional: bump CPU priority
            try
            {
                gameProcess.PriorityClass = ProcessPriorityClass.High;
            }
            catch
            {
                // ignore; game still runs
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Failed to start the game:\n" + ex.Message,
                "Game Launcher",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            // Shutdown Steam API if it was initialized
            if (steamInitialized)
            {
                SteamManager.Shutdown();
            }
        }
    }
}
