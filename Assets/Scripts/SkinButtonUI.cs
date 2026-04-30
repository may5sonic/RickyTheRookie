using UnityEngine;
using UnityEngine.UI;

public class SkinButtonUI : MonoBehaviour
{

    public int skinIndex;
    public GameObject lockIcon;
    public Button button;

    void Awake() {
        button = GetComponent<Button>();

        // Tell the button what to do when clicked
        button.onClick.AddListener(EquipThisSkin);
    }
    
    void OnEnable() 
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        bool unlocked = SkinManager.IsSkinUnlocked(skinIndex);

        // show locked if not unlocked
        if (lockIcon != null)
        {
            lockIcon.SetActive(!unlocked);
        }

        // disable click if locked
        if (button != null)
        {
            button.interactable = unlocked;
        }
    }

    // The function that actually saves your choice
    void EquipThisSkin()
    {
        // Tells your SelectedSkin script to save this specific index
        SelectedSkin.SetSkin(skinIndex);

        Debug.Log("Successfully equipped Skin Index: " + skinIndex);
    }
}
