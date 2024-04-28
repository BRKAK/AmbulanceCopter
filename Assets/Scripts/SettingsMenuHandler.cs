using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;


public class SettingsMenuHandler : MonoBehaviour
{
    public MenuHandler menuHandlerScript;
    public GameObject menu, brightness;
    public GameObject uiInterface;
    public AudioSource audioSource;
    private PostProcessVolume volume;
    private AutoExposure _ae;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Active: " + uiInterface.activeInHierarchy);
        if (uiInterface.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnBackBtnClicked();
            }
        }
    }

    public void OnBackBtnClicked()
    {
        uiInterface.SetActive(false);
        menu.SetActive(true);
        menuHandlerScript.SetInnerMenuIsDisplayed(false);
    }

    public void ChangeVolume()
    {
        float sliderVal = transform.GetChild(0).GetChild(2).GetComponent<Slider>().value;
        audioSource.volume = sliderVal;
    }

    public void ChangeBrightness()
    {
        float sliderVal = transform.GetChild(0).GetChild(3).GetComponent<Slider>().value;
        Debug.Log("Slider Val: " + sliderVal);
        volume = brightness.GetComponent<PostProcessVolume>();
        Debug.Log(volume.profile.TryGetSettings(out _ae));
        _ae.keyValue.value = sliderVal / 3;
        Debug.Log(_ae.keyValue.value);
        //light.intensity = transform.GetChild(0).GetChild(2).GetComponent<Slider>().value;
    }
}
