using System;
using System.Linq;

namespace GameLauncher
{
    /// <summary>
    /// Example utility class that can be used to unlock achievements from command line or external calls.
    /// This demonstrates how PGMMV games could trigger achievement unlocks.
    /// </summary>
    public static class AchievementHelper
    {
        /// <summary>
        /// Processes achievement commands from command-line arguments.
        /// Usage examples:
        ///   unlock ACHIEVEMENT_NAME
        ///   check ACHIEVEMENT_NAME
        ///   clear ACHIEVEMENT_NAME
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>Exit code (0 = success, 1 = error)</returns>
        public static int ProcessAchievementCommand(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Achievement Helper Usage:");
                Console.WriteLine("  unlock <achievement_name>  - Unlocks the specified achievement");
                Console.WriteLine("  check <achievement_name>   - Checks if achievement is unlocked");
                Console.WriteLine("  clear <achievement_name>   - Clears the achievement (for testing)");
                Console.WriteLine();
                Console.WriteLine("Example:");
                Console.WriteLine("  AchievementHelper.exe unlock ACHIEVEMENT_WIN_FIRST_GAME");
                return 1;
            }

            string command = args[0].ToLower();
            string achievementName = args[1];

            // Ensure Steam is initialized
            if (!SteamManager.IsInitialized)
            {
                Console.WriteLine("Initializing Steam API...");
                
                // Try to initialize with App ID from args if provided
                uint? appId = null;
                if (args.Length > 2 && uint.TryParse(args[2], out uint parsedAppId))
                {
                    appId = parsedAppId;
                }
                
                bool initialized = SteamManager.Initialize(appId);
                
                if (!initialized)
                {
                    Console.WriteLine("Error: Failed to initialize Steam API.");
                    Console.WriteLine("Make sure:");
                    Console.WriteLine("  - Steam client is running");
                    Console.WriteLine("  - steam_appid.txt exists with valid App ID");
                    return 1;
                }
                
                // Request current stats
                SteamManager.RequestCurrentStats();
            }

            bool success = false;
            
            switch (command)
            {
                case "unlock":
                    success = SteamManager.UnlockAchievement(achievementName);
                    if (success)
                    {
                        Console.WriteLine($"✓ Achievement unlocked: {achievementName}");
                    }
                    else
                    {
                        Console.WriteLine($"✗ Failed to unlock achievement: {achievementName}");
                    }
                    break;

                case "check":
                    bool isUnlocked = SteamManager.IsAchievementUnlocked(achievementName);
                    Console.WriteLine($"Achievement {achievementName}: {(isUnlocked ? "UNLOCKED" : "LOCKED")}");
                    success = true;
                    break;

                case "clear":
                    success = SteamManager.ClearAchievement(achievementName);
                    if (success)
                    {
                        Console.WriteLine($"✓ Achievement cleared: {achievementName}");
                    }
                    else
                    {
                        Console.WriteLine($"✗ Failed to clear achievement: {achievementName}");
                    }
                    break;

                default:
                    Console.WriteLine($"Unknown command: {command}");
                    Console.WriteLine("Valid commands: unlock, check, clear");
                    return 1;
            }

            return success ? 0 : 1;
        }

        /// <summary>
        /// Processes multiple achievements from a file.
        /// File format: one achievement name per line.
        /// </summary>
        /// <param name="filePath">Path to file containing achievement names</param>
        /// <returns>Number of achievements successfully unlocked</returns>
        public static int UnlockAchievementsFromFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine($"Error: File not found: {filePath}");
                return 0;
            }

            try
            {
                string[] achievements = System.IO.File.ReadAllLines(filePath)
                    .Select(line => line.Trim())
                    .Where(line => !string.IsNullOrEmpty(line) && !line.StartsWith("#"))
                    .ToArray();

                if (achievements.Length == 0)
                {
                    Console.WriteLine("No achievements found in file.");
                    return 0;
                }

                Console.WriteLine($"Processing {achievements.Length} achievement(s)...");

                int successCount = 0;
                foreach (string achievement in achievements)
                {
                    if (SteamManager.UnlockAchievement(achievement))
                    {
                        Console.WriteLine($"✓ {achievement}");
                        successCount++;
                    }
                    else
                    {
                        Console.WriteLine($"✗ {achievement}");
                    }
                }

                Console.WriteLine($"\nUnlocked {successCount}/{achievements.Length} achievements.");
                return successCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Example of how to use the achievement system from code.
        /// </summary>
        public static void DemoUsage()
        {
            Console.WriteLine("=== Steam Achievement System Demo ===\n");

            // Initialize Steam
            Console.WriteLine("1. Initializing Steam API...");
            bool initialized = SteamManager.Initialize(480); // Using Spacewar for demo
            
            if (!initialized)
            {
                Console.WriteLine("Failed to initialize Steam. Aborting demo.");
                return;
            }

            // Request stats
            Console.WriteLine("2. Requesting current stats...");
            SteamManager.RequestCurrentStats();

            // Example achievement name (this works with Spacewar - App ID 480)
            string testAchievement = "ACH_WIN_ONE_GAME";

            // Check current status
            Console.WriteLine($"\n3. Checking achievement status: {testAchievement}");
            bool isUnlocked = SteamManager.IsAchievementUnlocked(testAchievement);
            Console.WriteLine($"   Status: {(isUnlocked ? "UNLOCKED" : "LOCKED")}");

            // Try to unlock
            if (!isUnlocked)
            {
                Console.WriteLine($"\n4. Attempting to unlock: {testAchievement}");
                bool unlocked = SteamManager.UnlockAchievement(testAchievement);
                Console.WriteLine($"   Result: {(unlocked ? "SUCCESS" : "FAILED")}");
            }
            else
            {
                Console.WriteLine("\n4. Achievement already unlocked!");
            }

            // Cleanup
            Console.WriteLine("\n5. Shutting down Steam API...");
            SteamManager.Shutdown();
            
            Console.WriteLine("\n=== Demo Complete ===");
        }
    }
}
