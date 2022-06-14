using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionScript : MonoBehaviour
{
    [Header("Прикрепить анимацию подлета камеры к пьедесталу")]
    [SerializeField] private Animator PlayerFollowCamera;

    [Header("Прикрепить анимацию опускания кнопки Play")]
    [SerializeField] private Animator ButtonPlay;

    [Header("Прикрепить элементы выбора персонажа")]
    [SerializeField] private GameObject CharacterSelect;
    [Header("Прикрепить панель мужчины")]
    [SerializeField] private GameObject PanelsMale;
    [Header("Прикрепить панель женщины")]
    [SerializeField] private GameObject PanelsFemale;

    [Header("Прикрепить персонажа мужчину")]
    [SerializeField] private GameObject CharacterMale;

    [Header("Прикрепить персонажа женщину")]
    [SerializeField] private GameObject CharacterFemale;

    public bool playbutton = false;

    #region PLAY
    public void OnClickPlayButton()
    {
        playbutton = true;
        PlayerFollowCamera.SetTrigger("PlayButtonClick");
        ButtonPlay.SetTrigger("PlayButtonClick");
        StartCoroutine(timer());
    }
    public IEnumerator timer()
    {
        yield return new WaitForSeconds(1.3f);
            CharacterSelect.SetActive(true);
            CharacterMale.SetActive(true);
            PanelsMale.SetActive(true);
    }
    #endregion
    #region ARROW
    public void onClickArrowRight()
    {
            CharacterMale.SetActive(false);
            PanelsMale.SetActive(false);
            PanelsFemale.SetActive(true);
            CharacterFemale.SetActive(true);
    }
    public void onClickArrowLeft()
    {
            CharacterMale.SetActive(true);
            PanelsMale.SetActive(true);
            PanelsFemale.SetActive(false);
            CharacterFemale.SetActive(false);
    }
    #endregion
}

