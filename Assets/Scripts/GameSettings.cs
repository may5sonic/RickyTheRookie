using UnityEngine;

public static class GameSettings
{
    // Default speed if nothing saved yet
    public static float PlayerSpeed = 5f;

    private const string SpeedKey = "PlayerSpeed";

    // Bullet Speed
    public static float BulletSpeed = 20f; // Default value
    private const string BulletSpeedKey = "BulletSpeed";

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
}