using UnityEngine;

public static class SelectedSkin 
{
  // The Jet uses this to read the skin
  public static int GetSkin()
  {
    return PlayerPrefs.GetInt("SelectedSkin", 1); // Default to 1 if nothing saved yet
  }

  // The Button uses this to save the skin
  public static void SetSkin(int index)
  {
    PlayerPrefs.SetInt("SelectedSkin", index);
    PlayerPrefs.Save();
  }
}
