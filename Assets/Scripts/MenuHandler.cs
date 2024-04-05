using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    private bool menuOn = false;
    public GameObject settings;
    public GameObject uiInterface;
    public PlayerController player;
    public float timeScaleDelay = .5f;
    // Start is called before the first frame update
    void Start()
    {
        uiInterface.SetActive(false);
        settings.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        readInput();
    }

    void readInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuOn = !menuOn;
            if (menuOn)
            {
                Time.timeScale = 0;
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

    public void OnSettingsBtnClicked()
    {
        uiInterface.SetActive(false);
        settings.SetActive(true);

    }

    public void OnResumeBtnClicked()
    {
        player.SetGameState(PlayerController.GAME.START);
        uiInterface.SetActive(false);
        menuOn = false;
        Time.timeScale = .5f;
        StartCoroutine(IncreaseTimeScale(timeScaleDelay));
    }
    private IEnumerator IncreaseTimeScale(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 1;
    }
}
