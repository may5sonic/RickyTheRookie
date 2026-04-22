using UnityEngine;

    // helper class to save and check player progress
    public static class PlayerProgress {
        // Return highest level the player has unlocked
        public static int GetHighestLevel() {
            return PlayerPrefs.GetInt("HighestLevel", 1);
        }
        // called when player completes a level
        public static void CompleteLevel (int level) {
            int current = GetHighestLevel();
            //updates when player reached new high level
            if (level >= current) {
                // unlocks next level
                PlayerPrefs.SetInt("HighestLevel", level + 1);
                PlayerPrefs.Save();
            }
        }
    }
