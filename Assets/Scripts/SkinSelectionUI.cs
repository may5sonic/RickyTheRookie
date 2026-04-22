using UnityEngine;

public class SkinSelectionUI : MonoBehaviour
{
      // called when player clicks a skin button
    public void SelectSkin(int index) {
        // check if unlocked
        if(!SkinManager.IsSkinUnlocked(index)) {
            Debug.Log("Skin locked"); // skin test
           return;
        }
         // makes sure skin stays kept in between scenes
         SelectedSkin.SetSkin(index);
         Debug.Log("Saved Skin: " + index); // skin test
    }
}
