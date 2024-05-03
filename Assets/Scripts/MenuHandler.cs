using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public GameObject uiInterface;
    public PlayerController player;
    public float timeScaleDelay = .5f;
    public MissionHandler missionHandler;

    private bool menuOn = false;
    // Start is called before the first frame update
    void Awake()
    {
        uiInterface.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HeliCrashed();
        ReadInput();
    }

    private void HeliCrashed()
    {
        if(player.CheckGameState() == PlayerController.GAME.HELI_CRASHED)
        {
            transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            Canvas.ForceUpdateCanvases();
            uiInterface.SetActive(true);
            menuOn = true;
        }
    }

    private void ReadInput()
    {
        if (player.CheckGameState() != PlayerController.GAME.ERROR && player.CheckGameState() != PlayerController.GAME.OVER)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuOn = !menuOn;
                if (IsMenuOn())
                {
                    Time.timeScale = 0;
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
        if (player.CheckGameState() == PlayerController.GAME.OVER)
        {
            transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            Canvas.ForceUpdateCanvases();
            uiInterface.SetActive(true);
            menuOn = true;
        }
    }

    public void OnRestartBtnClicked()
    {
        player.SetGameState(PlayerController.GAME.RESTART);
        missionHandler.Initialize();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        DebugWarning("Time.time at restart: " + Time.time);
        Time.timeScale = 1;
    }

    private bool IsGameOver()
    {
        if (player.CheckGameState() == PlayerController.GAME.OVER || player.CheckGameState() == PlayerController.GAME.ERROR)
        {
            return true;
        }
        player.SetGameState(PlayerController.GAME.PLAYING);
        return false;
    }

    public void OnResumeBtnClicked()
    {
        if(!IsGameOver())
            Time.timeScale = 1;
        uiInterface.SetActive(false);
        menuOn = false;
        player.SetScriptTimeInterval(0.5f);
        StartCoroutine(IncreaseTimeScale(timeScaleDelay));
    }

    public void OnMainMenuBtnClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1, LoadSceneMode.Single);
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


    //DEBUG
    private void DebugWarning(string msg)
    {
        Debug.LogWarning("MenuHandler::" + msg);
    }
}
