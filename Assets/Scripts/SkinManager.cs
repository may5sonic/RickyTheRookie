using UnityEngine;

public static class SkinManager
{
    // returns how many skins player has unlocked
    public static int GetUnlockedSkins() {
        return PlayerProgress.GetHighestLevel();
    }
    // checks if a specific skin is unlocked based on level 
    public static bool IsSkinUnlocked(int skinIndex) {
        return skinIndex <= GetUnlockedSkins();
    }
}
