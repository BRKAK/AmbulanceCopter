using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MissionHandler : MonoBehaviour
{
    public string missionTime; 
    public SFXHandler SFXhandler;
    private int musicChanged = 0;
    public double[] musicMilestone = { 32, 16 }; 
    // Start is called before the first frame update
    void Awake()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        //ChangeMusic();
    }

    private void FixedUpdate()
    {
        ChangeMusic();
    }

    public void ChangeMusic()
    {
        if (musicChanged == 2)
            return;
        double x = musicMilestone[musicChanged];
        double counter = x + (1f / 16f);
        counter = Math.Truncate((counter * 100)) / 100;
        if (Math.Truncate(SFXhandler.SecondsLeftAsFloat() * 100) / 100 <= counter + .1f && Math.Truncate(SFXhandler.SecondsLeftAsFloat() * 100) / 100 >= counter - .1f)
        {
            Preload();
            double diff = Math.Truncate((Math.Truncate(SFXhandler.SecondsLeftAsFloat() * 100) / 100 - counter) * 100) / 100;
            Debug.LogError("DIFF: " + diff);
            musicChanged++;
            SFXhandler.SetAudioClip(musicChanged);
            SFXhandler.audioSource.PlayDelayed((float)diff);
        }
    }

    public void Initialize()
    {
        musicChanged = 0;
        DebugWarning("MissionHandler:Mission Time: " + missionTime);
    }

    public void Preload()
    {
        SFXhandler.audioClip[1].LoadAudioData();
        SFXhandler.audioClip[2].LoadAudioData();
    }

    //DEBUG
    private void DebugWarning(string msg)
    {
        Debug.LogWarning("MissionHandler::" + msg);
    }
}
