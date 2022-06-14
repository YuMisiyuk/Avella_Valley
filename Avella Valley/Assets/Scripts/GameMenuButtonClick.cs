using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuButtonClick : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject touchZones;
    [SerializeField] private GameObject menuPanel;

    [SerializeField] private Button homeButton;
    [SerializeField] private Button marketButton;
    [SerializeField] private Button settingsButton;

    bool buttonBlocked;

    void Start()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(false);
        touchZones.SetActive(true);
        buttonBlocked = false;
    }
    #region SETTINGS
    public void OnClickSettingsOpen()
    {
        settingsPanel.SetActive(true);
        touchZones.SetActive(false);
        buttonBlocked = true;

        if (buttonBlocked==true)
        {
            homeButton.interactable = false;
            marketButton.interactable = false;
            settingsButton.interactable = false;
        }
    }
    public void OnClickSettingsClose()
    {
        settingsPanel.SetActive(false);
        touchZones.SetActive(true);
        buttonBlocked = false;

        if (buttonBlocked == false)
        {
            homeButton.interactable = true;
            marketButton.interactable = true;
            settingsButton.interactable = true;
        }
    }
    #endregion

    #region BUTTON HOME
    public void OnClickHomeOpen()
    {
        menuPanel.SetActive(true);
        touchZones.SetActive(false);
        buttonBlocked = true;

        if (buttonBlocked == true)
        {
            homeButton.interactable = false;
            marketButton.interactable = false;
            settingsButton.interactable = false;
        }
    }

    public void OnClickHomeClose()
    {
        menuPanel.SetActive(false);
        touchZones.SetActive(true);
        buttonBlocked = false;

        if (buttonBlocked == false)
        {
            homeButton.interactable = true;
            marketButton.interactable = true;
            settingsButton.interactable = true;
        }
    }
    #endregion

    #region ENTER/EXIT HOUSE
    public void EnterToHome()
    {
        SceneManager.LoadScene(2);
    }
    public void ExitFromHome()
    {
        SceneManager.LoadScene(1);
    }

    #endregion
}
