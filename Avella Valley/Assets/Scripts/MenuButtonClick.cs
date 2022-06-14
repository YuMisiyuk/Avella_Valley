using UnityEngine;
using UnityEngine.UI;

public class MenuButtonClick : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject playPanel;

    [SerializeField] private Button settingsButton;

    bool buttonBlocked;

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

        if (buttonBlocked == false)
        {
            settingsButton.interactable = true;
        }
    }
#endregion
}
