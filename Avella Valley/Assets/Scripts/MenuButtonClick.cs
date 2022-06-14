using UnityEngine;
using UnityEngine.UI;

public class MenuButtonClick : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject CharacterSelect;
    [SerializeField] private GameObject PanelsMale;
    [SerializeField] private GameObject PanelsFemale;

    [SerializeField] private Button settingsButton;

    bool buttonBlocked;

    [SerializeField] private CharacterSelectionScript _ñharacterSelectionScript;

    void Start()
    {
        settingsPanel.SetActive(false);
        playPanel.SetActive(true);
        buttonBlocked = false;

    }

#region SETTINGS
    public void OnClickSettingsOpen()
    {
        settingsPanel.SetActive(true);
        playPanel.SetActive(false);
        buttonBlocked = true;
        if(_ñharacterSelectionScript.playbutton == true)
        {
            CharacterSelect.SetActive(false);
            PanelsMale.SetActive(false);
            PanelsFemale.SetActive(false);
        }

        if (buttonBlocked == true)
        {
            settingsButton.interactable = false;
        }
    }
    public void OnClickSettingsClose()
    {
        settingsPanel.SetActive(false);
        playPanel.SetActive(true);
        buttonBlocked = false;
        if (_ñharacterSelectionScript.playbutton == true)
        {
            CharacterSelect.SetActive(true);
            PanelsMale.SetActive(true);
            PanelsFemale.SetActive(true);
        }

        if (buttonBlocked == false)
        {
            settingsButton.interactable = true;
        }
    }
#endregion
}
