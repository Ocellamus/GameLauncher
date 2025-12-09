using System;
using System.IO;
using Steamworks;

namespace GameLauncher
{
    /// <summary>
    /// Manages Steam API initialization and provides Steam functionality including achievements.
    /// </summary>
    public static class SteamManager
    {
        private static bool _isInitialized = false;
        private static bool _isSteamRunning = false;

        /// <summary>
        /// Gets whether Steam API has been successfully initialized.
        /// </summary>
        public static bool IsInitialized => _isInitialized;

        /// <summary>
        /// Gets whether Steam client is running.
        /// </summary>
        public static bool IsSteamRunning => _isSteamRunning;

        /// <summary>
        /// Initializes the Steam API. This should be called before any Steam functionality is used.
        /// </summary>
        /// <param name="appId">Optional Steam App ID. If not provided, reads from steam_appid.txt</param>
        /// <returns>True if initialization was successful, false otherwise.</returns>
        public static bool Initialize(uint? appId = null)
        {
            if (_isInitialized)
            {
                return true;
            }

            try
            {
                // If appId is provided, create steam_appid.txt file
                if (appId.HasValue)
                {
                    CreateSteamAppIdFile(appId.Value);
                }

                // Check if Steam client is running
                if (!Packsize.Test())
                {
                    Console.WriteLine("Steam API: Packsize test failed. Steam may not be running correctly.");
                    return false;
                }

                // Initialize Steam API
                _isSteamRunning = SteamAPI.Init();
                
                if (_isSteamRunning)
                {
                    _isInitialized = true;
                    Console.WriteLine("Steam API initialized successfully.");
                    
                    // Get user info for verification
                    if (SteamUser.BLoggedOn())
                    {
                        string userName = SteamFriends.GetPersonaName();
                        Console.WriteLine($"Steam user logged in: {userName}");
                    }
                    
                    return true;
                }
                else
                {
                    Console.WriteLine("Steam API initialization failed. Steam client may not be running.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Steam API initialization error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Shuts down the Steam API. This should be called when the application exits.
        /// </summary>
        public static void Shutdown()
        {
            if (_isInitialized)
            {
                SteamAPI.Shutdown();
                _isInitialized = false;
                _isSteamRunning = false;
                Console.WriteLine("Steam API shut down.");
            }
        }

        /// <summary>
        /// Unlocks a Steam achievement.
        /// </summary>
        /// <param name="achievementId">The API name of the achievement to unlock.</param>
        /// <returns>True if the achievement was successfully unlocked, false otherwise.</returns>
        public static bool UnlockAchievement(string achievementId)
        {
            if (!_isInitialized || !_isSteamRunning)
            {
                Console.WriteLine("Steam API not initialized. Cannot unlock achievement.");
                return false;
            }

            try
            {
                // Set the achievement
                bool success = SteamUserStats.SetAchievement(achievementId);
                
                if (success)
                {
                    // Store stats to commit the achievement
                    bool stored = SteamUserStats.StoreStats();
                    
                    if (stored)
                    {
                        Console.WriteLine($"Achievement unlocked: {achievementId}");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to store achievement: {achievementId}");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to set achievement: {achievementId}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error unlocking achievement {achievementId}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Checks if a specific achievement has been unlocked.
        /// </summary>
        /// <param name="achievementId">The API name of the achievement to check.</param>
        /// <returns>True if the achievement is unlocked, false otherwise.</returns>
        public static bool IsAchievementUnlocked(string achievementId)
        {
            if (!_isInitialized || !_isSteamRunning)
            {
                return false;
            }

            try
            {
                bool achieved;
                bool success = SteamUserStats.GetAchievement(achievementId, out achieved);
                return success && achieved;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking achievement {achievementId}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Clears (locks) a specific achievement. Useful for testing.
        /// </summary>
        /// <param name="achievementId">The API name of the achievement to clear.</param>
        /// <returns>True if the achievement was successfully cleared, false otherwise.</returns>
        public static bool ClearAchievement(string achievementId)
        {
            if (!_isInitialized || !_isSteamRunning)
            {
                Console.WriteLine("Steam API not initialized. Cannot clear achievement.");
                return false;
            }

            try
            {
                bool success = SteamUserStats.ClearAchievement(achievementId);
                
                if (success)
                {
                    bool stored = SteamUserStats.StoreStats();
                    
                    if (stored)
                    {
                        Console.WriteLine($"Achievement cleared: {achievementId}");
                        return true;
                    }
                }
                
                Console.WriteLine($"Failed to clear achievement: {achievementId}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing achievement {achievementId}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Requests current stats from Steam. This should be called after initialization.
        /// </summary>
        /// <returns>True if stats were successfully requested, false otherwise.</returns>
        public static bool RequestCurrentStats()
        {
            if (!_isInitialized || !_isSteamRunning)
            {
                return false;
            }

            try
            {
                return SteamUserStats.RequestCurrentStats();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error requesting current stats: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Creates a steam_appid.txt file in the current directory.
        /// This file tells Steam which application ID to use.
        /// </summary>
        /// <param name="appId">The Steam App ID to write to the file.</param>
        private static void CreateSteamAppIdFile(uint appId)
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "steam_appid.txt");
                File.WriteAllText(filePath, appId.ToString());
                Console.WriteLine($"Created steam_appid.txt with App ID: {appId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not create steam_appid.txt: {ex.Message}");
            }
        }

        /// <summary>
        /// Runs Steam callbacks. This should be called regularly (e.g., in a game loop).
        /// For a launcher, this might not be necessary as we're not running continuously.
        /// </summary>
        public static void RunCallbacks()
        {
            if (_isInitialized && _isSteamRunning)
            {
                try
                {
                    SteamAPI.RunCallbacks();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error running Steam callbacks: {ex.Message}");
                }
            }
        }
    }
}
