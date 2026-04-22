using UnityEngine;
using UnityEngine.UI;

public class SkinButtonUI : MonoBehaviour
{

    public int skinIndex;
    public GameObject lockIcon;
    public Button button;

    void Awake() {
        button = GetComponent<Button>();
    }
    
    void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        bool unlocked = SkinManager.IsSkinUnlocked(skinIndex);

        // show locked if not unlocked
        lockIcon.SetActive(!unlocked);

        // disable click if locked
        button.interactable = unlocked;
    }
}
