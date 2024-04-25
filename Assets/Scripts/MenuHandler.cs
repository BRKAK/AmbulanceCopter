using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    private bool menuOn = false;
    public GameObject settings;
    public GameObject uiInterface;
    public PlayerController player;
    public float timeScaleDelay = .5f;
    private bool innerMenuDisplayed = false;
    // Start is called before the first frame update
    void Start()
    {
        uiInterface.SetActive(false);
        settings.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ReadInput();
    }

    private void ReadInput()
    {
        if (!innerMenuDisplayed)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuOn = !menuOn;
                if (menuOn)
                {
                    player.SetScriptTimeInterval(0);
                    IsGameOver();
                    uiInterface.SetActive(true);
                }
                else
                {
                    OnResumeBtnClicked();
                    return;
                }
            }
        }
    }

    public void SetInnerMenuIsDisplayed(bool flag) //Sets whether the inner menu is displayed or not
    {
        innerMenuDisplayed = flag;
    }

    public void OnRestartBtnClicked()
    {
        SceneManager.LoadScene("Playground");
    }

    public void OnSettingsBtnClicked()
    {
        innerMenuDisplayed = true;
        uiInterface.SetActive(false);
        settings.SetActive(true);

    }

    private void IsGameOver()
    {
        if (player.CheckGameState() != PlayerController.GAME.OVER)
        {
            player.SetGameState(PlayerController.GAME.START);

        }
    }

    public void OnResumeBtnClicked()
    {
        IsGameOver();
        uiInterface.SetActive(false);
        menuOn = false;
        player.SetScriptTimeInterval(0.5f);
        StartCoroutine(IncreaseTimeScale(timeScaleDelay));
    }
    private IEnumerator IncreaseTimeScale(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!menuOn)
            player.SetScriptTimeInterval(1f);
    }
}
