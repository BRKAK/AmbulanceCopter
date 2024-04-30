using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public Camera camera;
    public SettingsMenuHandler settings;

    private bool settingsFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        Time.timeScale = 1;
        if (settingsFlag)
            settingsFlag = false;
        SceneManager.GetActiveScene().GetRootGameObjects()[0].gameObject.SetActive(true);           //camera.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (!settingsFlag)
            SceneManager.GetActiveScene().GetRootGameObjects()[0].gameObject.SetActive(false);       //camera.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayBtnClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        camera.GetComponent<AudioSource>().Stop();
    }

    public void OnSettingsBtnClicked()
    {
        settingsFlag = true;
        settings.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnQuitBtnClicked()
    {
        camera.GetComponent<AudioSource>().Stop();        
        Application.Quit();
    }
}
