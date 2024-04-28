using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public GameObject settings;
    public GameObject uiInterface;
    public PlayerController player;
    public float timeScaleDelay = .5f;
    public MissionHandler missionHandler;

    private bool menuOn = false;
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
        //Debug.LogWarning(player == null);
    }

    private void ReadInput()
    {
        if(player.CheckGameState() != PlayerController.GAME.ERROR)
            if (!innerMenuDisplayed)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    menuOn = !menuOn;
                    if (menuOn)
                    {
                        player.SetScriptTimeInterval(0);
                        IsGameOver();
                        player.SetGameState(PlayerController.GAME.STOP);
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
        player.SetGameState(PlayerController.GAME.RESTART);
        missionHandler.Initialize();
        SceneManager.LoadScene("Playground");
        Debug.Log("Time.time at restart: " + Time.time);
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
        player.SetGameState(PlayerController.GAME.START);
        StartCoroutine(IncreaseTimeScale(timeScaleDelay));
    }
    private IEnumerator IncreaseTimeScale(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!menuOn)
            player.SetScriptTimeInterval(1f);
    }

    public bool IsMenuOn()
    {
        return menuOn;
    }
}
