using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    public DialogueObject dialogues;
    public Canvas timer, liftBar;
    public MissionHandler missionHandler;
    public GameObject DialogueWindow, menu;
    public Text dialogue;
    private int dialogueCounter = 0;
    private bool dialogueOver = false;
    private SFXHandler sFX;

    // Update is called once per frame
    void Update()
    {
        if (!dialogueOver)
        {
            DisplayDialogue();
        }
        else
        {
            DialogueWindow.SetActive(false);
            missionHandler.givePlayerControls = true;
            menu.SetActive(true);
        }
    }

    void DisplayDialogue()
    {
        switch (dialogueCounter)
        {
            case 2:
                timer.gameObject.SetActive(true);
                break;
            case 3:
                liftBar.gameObject.SetActive(true);
                break;
        }
        dialogue.text = dialogues.Dialogue[dialogueCounter];
        if (Input.GetKeyDown(KeyCode.Space) && dialogueCounter < 7)
        {
            dialogueCounter++;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && dialogueCounter == 7)
        {
            dialogueCounter++;
            dialogueOver = true;
            missionHandler.missionTime += Time.timeSinceLevelLoad;
        }
    }
}
