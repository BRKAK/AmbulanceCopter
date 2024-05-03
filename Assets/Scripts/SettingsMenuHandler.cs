using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using System;


public class SettingsMenuHandler : MonoBehaviour
{
    public GameObject brightness;
    public AudioSource audioSource;
    public MainMenuHandler mainMenuHandler;
    public Slider audioSlider, brightnessSlider;
    public Font consola;

    private float brightnessSliderVal, audioSliderVal;
    private PostProcessVolume volume;
    private AutoExposure _ae;
    // Start is called before the first frame update
    void Awake()
    {
        
        try
        {
            audioSliderVal = PlayerPrefs.GetFloat("AudioLevel");
            brightnessSliderVal = PlayerPrefs.GetFloat("BrightnessLevel");
        }
        catch(Exception e)
        {
            Debug.LogWarning(e);
            audioSliderVal = transform.GetChild(0).GetChild(2).GetComponent<Slider>().value;
            brightnessSliderVal = transform.GetChild(0).GetChild(3).GetComponent<Slider>().value;
            return;
        }
        audioSlider.value = audioSliderVal;
        brightnessSlider.value = brightnessSliderVal * 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBackBtnClicked()
    {
        mainMenuHandler.gameObject.SetActive(true);
        gameObject.SetActive(false);
        //gameObject.SetActive(false);
    }

    public void OnSaveBtnClicked()
    {
        try
        {
            PlayerPrefs.SetFloat("AudioLevel", audioSliderVal);
            PlayerPrefs.SetFloat("BrightnessLevel", brightnessSliderVal / 3);
            PlayerPrefs.Save();
            StartCoroutine(ShowPopUpMessageToUser("Save Successful!"));
        }catch(Exception e)
        {
            Debug.LogWarning(e);
            StartCoroutine(ShowPopUpMessageToUser("Save Failed!"));
        }

        Debug.LogWarning("AudioLevel_PlayerPrefs: " + PlayerPrefs.GetFloat("AudioLevel"));
        Debug.LogWarning("BrightnessLevel_PlayerPrefs: " + PlayerPrefs.GetFloat("BrightnessLevel"));
    }

    private IEnumerator ShowPopUpMessageToUser(string msg)
    {
        GameObject popUpMsg = new GameObject("PopUpMessage");
        popUpMsg.transform.SetParent(transform);
        popUpMsg.SetActive(true);

        Text message = popUpMsg.AddComponent<Text>();
        message.gameObject.SetActive(true);

        message.rectTransform.anchoredPosition = Vector2.zero;
        message.alignment = TextAnchor.MiddleCenter;
        message.alignByGeometry = true;

        message.rectTransform.anchorMin = new Vector2(.5f, .5f);
        message.rectTransform.anchorMax = new Vector2(.5f, .5f);
        message.rectTransform.pivot = new Vector2(.5f, .5f);
        message.rectTransform.sizeDelta = new Vector2(480, 200);

        message.color = Color.red;
        message.fontSize = 50;

        //message.font = Resources.Load<Font>("Packages/com.unity.mobile.android-logcat/Editor/Fonts/consola.ttf");
        message.font = consola;
        message.text = msg;
        yield return new WaitForSeconds(.2f);
        Destroy(popUpMsg);
    }

    public void ChangeVolume()
    {
        audioSliderVal = transform.GetChild(0).GetChild(2).GetComponent<Slider>().value;
        audioSource.volume = audioSliderVal;
    }

    public void ChangeBrightness()
    {
        brightnessSliderVal = transform.GetChild(0).GetChild(3).GetComponent<Slider>().value;
        Debug.Log("Slider Val: " + brightnessSliderVal);
        volume = brightness.GetComponent<PostProcessVolume>();
        Debug.Log(volume.profile.TryGetSettings(out _ae));
        _ae.keyValue.value = brightnessSliderVal / 3;
        Debug.Log(_ae.keyValue.value);
        //light.intensity = transform.GetChild(0).GetChild(2).GetComponent<Slider>().value;
    }
}
