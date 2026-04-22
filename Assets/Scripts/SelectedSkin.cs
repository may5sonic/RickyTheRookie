using UnityEngine;

public static class SelectedSkin {
    // saves the skin index based on what the player selected on menu
  public static void SetSkin(int index) {
    PlayerPrefs.SetInt("SelectedSkin", index);
  }
  // gets the skin currently selected
  // 1 is default
  public static int GetSkin() {
    return PlayerPrefs.GetInt("SelectedSkin", 1);
  }
}
