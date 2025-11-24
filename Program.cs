using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // 1. Locate the launcher and then the game EXE
        string launcherPath = Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
        string launcherDir = Path.GetDirectoryName(launcherPath) ?? string.Empty;

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

        // 2. Show the custom optimization form
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

        // 3. Inform the user if changes were successfully applied
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

        // 4. Launch the game (always)
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
    }
}
