using UnityEngine;

public static class SkinManager
{
    // returns how many skins player has unlocked
    public static int GetUnlockedSkins() {
        return PlayerProgress.GetHighestLevel();
    }
    // checks if a specific skin is unlocked based on level 
    public static bool IsSkinUnlocked(int skinIndex) {
        // Skins 1-3 level progress
        if (skinIndex <= 3) {
            return skinIndex <= GetUnlockedSkins();
        }
        // Skin 4 boss skin unlock
        if (skinIndex == 4) {
            return PlayerProgress.IsBossSkinUnlocked();
        }
        return false;
    }
}
