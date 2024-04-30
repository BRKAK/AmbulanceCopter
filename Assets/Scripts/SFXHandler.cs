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
    private int secondCounter = 0, timeAtLeveLoad = 0;
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
        timeAtLeveLoad -= (int)Time.time;
    }

    void Main()
    {
        AdjustAudio();
        DisplayGameOverText();
        if (player.CheckGameState() == PlayerController.GAME.PLAYING)
        {
            secondCounter = (int)(Time.timeSinceLevelLoad) + timeAtLeveLoad;
            UpdateTimer();
        }

    }

    void AdjustAudio() //Plays or stops the audio according to the gamestate
    {
        if (!audioSource.isPlaying && player.CheckGameState() == PlayerController.GAME.PLAYING) 
        {
            audioSource.UnPause();
            return;
        }
        if (player.CheckGameState() == PlayerController.GAME.ERROR || player.CheckGameState() == PlayerController.GAME.OVER)
        {
            audioSource.Stop();
        }
        else if (audioSource.isPlaying && player.CheckGameState() == PlayerController.GAME.STOP)
        {
            audioSource.Pause();
        }
    }


    void DisplayErrorMessage(string msg)
    {
        player.SetGameState(PlayerController.GAME.ERROR);
        Time.timeScale = 0;
        GameOverText.gameObject.SetActive(true);
        audioSource.Stop();
        GameOverText.GetComponentInChildren<Text>().text = msg;
    }




    public void PrepareVariablesToRestart()
    {
        timerText.text = missionHandler.missionTime;
        audioSource.Stop();
        audioSource.clip = audioClip[0];
        
    }

    private void UpdateTimer()
    {
        int remained = missionTime - secondCounter;
        int min, sec;
        min = remained / 60;
        sec = remained % 60;
        if(min == 0) {
            timerText.text = "0:" + sec;
            FormatSecond(min, sec);
        }
        else
        {
            timerText.text = min + ":" + sec;
            FormatSecond(min, sec);

        }
        if (remained == 0)
        {
            player.SetGameState(PlayerController.GAME.OVER);
            GameOverText.GetComponentInChildren<Text>().text = "Out of time";
            return;
        }

    }

    private void FormatSecond(int min, int sec)
    {
        if (sec < 10)
        {
            timerText.text = min + ":0" + sec;
        }
    }

    public int SecondsLeft()
    {
        return missionTime - secondCounter;
    }

    public double SecondsLeftAsFloat()
    {
        return Math.Truncate((missionTime - Time.fixedTime + timeAtLeveLoad) * 100) / 100;
    }

    private void DisplayGameOverText()
    {
        try
        {
            if (player.CheckGameState() == PlayerController.GAME.OVER)
            {
                Time.timeScale = 0;
                GameOverText.gameObject.SetActive(true);
                if (audioSource.isPlaying)
                    audioSource.Stop();
            }
        }
        catch(Exception e)
        {
            DebugWarning(e.ToString());
        }
    }


    public void SetAudioClip(int index)
    {
        audioSource.Stop();
        audioSource.clip = audioClip[index];
    }


    //DEBUG
    private void ValidateTextInput() //Validates the input given to the timer text                                                  PURPOSE: DEBUG
    {
        DebugWarning("SFXHandler:Timer Text Not Null");
        try
        {
            MinNSec = timerText.text.Split(':');
            minutes = int.Parse(MinNSec[0]);
            seconds = int.Parse(MinNSec[1]);
            missionTime = minutes * 60 + seconds;
            DebugWarning("Mission Time: " + missionTime);
            if (MinNSec[0].Length > 1 || seconds > 59)
                throw new FormatException("Limits are exceeded");
        }
        catch (Exception e)
        {
            DebugWarning(e.ToString());
            DebugWarning("Input Mismatch Check Editor For Timer Text");
            DisplayErrorMessage("TIME INPUT ERROR");
        }

    }
    public void SetMissionTimer(string str) //This function validates the input coming from the mission handling script             PURPOSE: DEBUG
    {
        timerText.text = str;
        ValidateTextInput();
    }
    private void InitializeAudio() //Audio initialization depending on the input validity                                           PURPOSE: DEBUG
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
            DebugWarning(e.ToString());
            DisplayErrorMessage("Audio File Error");
        }
    }

    private void DebugWarning(string msg)                                                                                         //PURPOSE: DEBUG
    {
        Debug.LogWarning("SFXHandler::" + msg);
    }
}
