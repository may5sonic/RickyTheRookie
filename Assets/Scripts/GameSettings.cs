using UnityEngine;

public static class GameSettings
{
    // Default speed if nothing saved yet
    public static float PlayerSpeed = 5f;

    private const string SpeedKey = "PlayerSpeed";

    // Bullet Speed
    public static float BulletSpeed = 20f; // Default value
    private const string BulletSpeedKey = "BulletSpeed";

    // NEW: CAMPAIGN MEMORY
    public static string SavedLevel = ""; 
    public static int SavedRound = 1;
    public static int SavedLives = 3;
    public static int SavedScore = 0;

    // Load from PlayerPrefs (disk)
    public static void Load()
    {
        PlayerSpeed = PlayerPrefs.GetFloat(SpeedKey, PlayerSpeed);

        BulletSpeed = PlayerPrefs.GetFloat(BulletSpeedKey, BulletSpeed);
    }

    // Save to PlayerPrefs (disk)
    public static void Save()
    {
        PlayerPrefs.SetFloat(SpeedKey, PlayerSpeed);

        PlayerPrefs.SetFloat(BulletSpeedKey, BulletSpeed);

        PlayerPrefs.Save();
    }

    public static void LoadProgress()
    {
        SavedLevel = PlayerPrefs.GetString("SavedLevel", ""); 
        SavedRound = PlayerPrefs.GetInt("SavedRound", 1);
        SavedLives = PlayerPrefs.GetInt("SavedLives", 3);
        SavedScore = PlayerPrefs.GetInt("SavedScore", 0);
    }

    // Save Campaign Progress
    public static void SaveProgress(string level, int round, int lives, int score)
    {
        PlayerPrefs.SetString("SavedLevel", level);
        PlayerPrefs.SetInt("SavedRound", round);
        PlayerPrefs.SetInt("SavedLives", lives);
        PlayerPrefs.SetInt("SavedScore", score);
        PlayerPrefs.Save();
    }

    // Wipe Campaign Progress (For a New Game)
    public static void ClearProgress()
    {
        PlayerPrefs.SetString("SavedLevel", ""); 
        PlayerPrefs.Save();
    }
}