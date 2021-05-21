using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Material kartMaterial;
    public Material logoMaterial;
    public GameObject logoObj;
    private int selectedLogo;
    private int selectedColor;

    [Header("Body Textures:")]
    public Texture[] bodyTextures;
    [Header("Logo Textures")]
    public Texture[] logoTexture;

    [Header("Audio")]
    public AudioMixer audioMixer;
    [Header("UI")]
    public Dropdown resDrop;
    public InputField playerNameInputField;

    Resolution[] resolutions;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("SelectedColor"))
        {
            selectedColor = PlayerPrefs.GetInt("SelectedColor");
            kartMaterial.mainTexture = bodyTextures[selectedColor];
        }
        else
        {
            selectedColor = 0;
            kartMaterial.mainTexture = bodyTextures[selectedColor];
            PlayerPrefs.SetInt("SelectedColor", selectedColor);
        }


        if (PlayerPrefs.HasKey("SelectedLogo"))
        {
            selectedLogo = PlayerPrefs.GetInt("SelectedLogo");
            if (selectedLogo == 0)
                logoObj.SetActive(false);
            else
            {
                logoObj.SetActive(true);
                logoMaterial.mainTexture = logoTexture[selectedLogo];
            }
        }
        else
        {
            selectedLogo = 0;
            PlayerPrefs.SetInt("SelectedLogo", selectedLogo);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        resDrop.ClearOptions();

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            playerNameInputField.text = PlayerPrefs.GetString("PlayerName");
        }
        else
        {
            playerNameInputField.text = "Player" + Random.Range(0, 1000);
        }
        List<string> options = new List<string>();
        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResIndex = i;
        }

        resDrop.AddOptions(options);
        resDrop.value = currentResIndex;
        resDrop.RefreshShownValue();
    }



    public void ButtonClick(int index)
    {
        switch (index)
        {
            case 0:
                SceneManager.LoadScene("SingleGame");
                PlayerPrefs.SetString("PlayerName", playerNameInputField.text);
                break;
            case 1:
                SceneManager.LoadScene("MultiMenu");
                break;
            case 2:
                Application.Quit();
                break;
            default:
                break;
        }
    }

    public void PartButtons(int index)
    {
        if (index == 10)
        {
            logoObj.SetActive(false);
            selectedLogo = index;
            PlayerPrefs.SetInt("SelectedLogo", selectedLogo);
        }
        else
        {
            selectedLogo = index;
            logoObj.SetActive(true);
            logoMaterial.mainTexture = logoTexture[selectedLogo];
            PlayerPrefs.SetInt("SelectedLogo", selectedLogo);
        }

    }

    public void ColorButtons(int index)
    {
        kartMaterial.mainTexture = bodyTextures[index];
        PlayerPrefs.SetInt("SelectedColor", index);
    }

    #region Settings
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("music", volume);
    }
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
    public void SetFullScreen(bool screen)
    {
        Screen.fullScreen = screen;
    }
    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    #endregion
}
