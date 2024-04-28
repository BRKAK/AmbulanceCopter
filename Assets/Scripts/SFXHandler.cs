using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXHandler : MonoBehaviour
{
    public AudioClip[] audioClip;//[0]:MenuTheme, [1]:YouBetterHurry, [2]:ImPanicking
    public AudioSource audioSource;
    public PlayerController player;
    public RawImage GameOverText;
    public MenuHandler menuHandler;
    public MissionHandler missionHandler;//remove later

    private int missionTime = 0, minutes = 0, seconds = 0;
    private Text timerText;
    private string[] MinNSec;
    private int secondCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Main();
        if (player.CheckGameState() == PlayerController.GAME.RESTART)
        {
            PrepareVariablesToRestart();
        }
    }

    public void Initialize()
    {
        timerText = transform.GetComponent<Text>();
        timerText.text = missionHandler.missionTime;
        ValidateTextInput();
        InitializeAudio();
    }

    private void ValidateTextInput() //Validates the input given to the timer text                                                  PURPOSE:DEBUG
    {
        Debug.LogWarning("SFXHandler:Timer Text Not Null");
        try
        {
            MinNSec = timerText.text.Split(':');
            minutes = int.Parse(MinNSec[0]);
            seconds = int.Parse(MinNSec[1]);
            missionTime = minutes * 60 + seconds;
            Debug.Log(missionTime);
            if (MinNSec[0].Length > 1 || seconds > 59)
                throw new FormatException("Limits are exceeded");
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            player.SetGameState(PlayerController.GAME.ERROR);
            Debug.LogWarning("Input Mismatch Check Editor For Timer Text");
            Text debugMsg;
            debugMsg = GameOverText.GetComponentInChildren<Text>();
            debugMsg.text = "INPUT ERROR";
            GameOverText.gameObject.SetActive(true);
            audioSource.Stop();
            player.SetScriptTimeInterval(0);
        }

    }

    private void InitializeAudio() //Audio initialization depending on the input validity                                           PURPOSE:DEBUG
    {
        try
        {
            SetAudioClip(0);
            audioSource.playOnAwake = true;
            audioSource.loop = true;
            if (!audioSource.isPlaying) audioSource.Play();

            if (player.CheckGameState() == PlayerController.GAME.ERROR)
                audioSource.Stop();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }


    void Main()
    {
        DisplayGameOverText();
        if (player.CheckGameState() == PlayerController.GAME.START)
        {
            secondCounter = (int)(Time.timeSinceLevelLoad);
            UpdateTimer();
        }

    }

    public void PrepareVariablesToRestart()
    {
        timerText.text = missionHandler.missionTime;
        audioSource.Stop();
        audioSource.clip = audioClip[0];
        secondCounter = 0;
        
    }

    private void UpdateTimer()
    {
        int remained = missionTime - secondCounter;
        int min, sec;
        min = remained / 60;
        sec = remained % 60;
        if(min == 0) {
            timerText.text = "0:" + sec;
        }
        else
        {
            timerText.text = min + ":" + sec;
            if (sec < 10)
                timerText.text = min + ":0" + sec;

        }
        if (remained == 0)
        {
            player.SetGameState(PlayerController.GAME.OVER);
            GameOverText.GetComponentInChildren<Text>().text = "Out of time";
            player.SetScriptTimeInterval(0);
            return;
        }

    }

    public int SecondsLeft()
    {
        return missionTime - secondCounter;
    }

    public double SecondsLeftAsFloat()
    {
        return Math.Truncate((missionTime - Time.fixedTime) * 100) / 100;
    }

    private void DisplayGameOverText()
    {
        try
        {
            if (player.CheckGameState() == PlayerController.GAME.OVER)
            {
                GameOverText.gameObject.SetActive(true);
                if (audioSource.isPlaying)
                    audioSource.Stop();
            }
        }
        catch(Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    public void SetMissionTimer(string str) //This function validates the input coming from the mission handling script             PURPOSE:DEBUG
    {
        timerText.text = str;
        ValidateTextInput();
    }

    public void SetAudioClip(int index)
    {
        audioSource.Stop();
        audioSource.clip = audioClip[index];
    }
}
