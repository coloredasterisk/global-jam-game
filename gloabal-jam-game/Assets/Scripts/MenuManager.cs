using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    [Header("MainMenu")]
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;

    [Header("\nSettingsMenu")]
    public GameObject settingsMenu;
    public Button returnButton;

    [Header("\nGridOptions")]
    public Toggle visibleToggle;
    public Slider redSlider;
    public Slider greenSlider;
    public Slider blueSlider;
    public Button defaultButton;
    public Image colorDisplay;

    // Start is called before the first frame update
    void Start()
    {
        //MainMenu stuff
        playButton.onClick.AddListener(PlayGameButton);
        settingsButton.onClick.AddListener(OpenSettingsMenu);
        quitButton.onClick.AddListener(QuitTheGame);

        //settings menu stuff
        returnButton.onClick.AddListener(CloseSettingsMenu);

        //GridOptions
        visibleToggle.isOn = SettingsManager.isGridVisible;
        visibleToggle.onValueChanged.AddListener(ToggleGridVisibility);
        redSlider.onValueChanged.AddListener(RedColorSlider);
        greenSlider.onValueChanged.AddListener(GreenColorSlider);
        blueSlider.onValueChanged.AddListener(BlueColorSlider);
        defaultButton.onClick.AddListener(DefaultColor);
        CopySettingsToSliders();
        UpdateColorDisplay();
    }


    //MainMenu stuff
    void PlayGameButton()
    {
        SceneManager.LoadScene("GameScene");
    }
    void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
    }
    void QuitTheGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    //End of mainMenu stuff



    //Settings Menu stuff
    void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }




    //GridOptions stuff
    void ToggleGridVisibility(bool value)
    {
        SettingsManager.isGridVisible = value;
        UpdateColorDisplay();
    }
    void RedColorSlider(float value)
    {
        SettingsManager.gridRed = value;
        UpdateColorDisplay();
    }
    void GreenColorSlider(float value)
    {
        SettingsManager.gridGreen = value;
        UpdateColorDisplay();
    }
    void BlueColorSlider(float value)
    {
        SettingsManager.gridBlue = value;
        UpdateColorDisplay();
    }

    void DefaultColor()
    {
        SettingsManager.gridRed = 50f / 255f;
        SettingsManager.gridGreen = 100f / 255f;
        SettingsManager.gridBlue = 160f / 255f;
        CopySettingsToSliders();
        UpdateColorDisplay();
    }
    void CopySettingsToSliders()
    {
        redSlider.value = SettingsManager.gridRed;
        greenSlider.value = SettingsManager.gridGreen;
        blueSlider.value = SettingsManager.gridBlue;
    }
    void UpdateColorDisplay()
    {
        colorDisplay.color = new Color(SettingsManager.gridRed, SettingsManager.gridGreen, SettingsManager.gridBlue);
    }

    //End of grid options stuff
}
