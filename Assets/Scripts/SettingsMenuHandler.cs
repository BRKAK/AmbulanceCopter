using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuHandler : MonoBehaviour
{
    public GameObject menu;
    public GameObject uiInterface;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBackBtnClicked()
    {
        uiInterface.SetActive(false);
        menu.SetActive(true);
    }
}
