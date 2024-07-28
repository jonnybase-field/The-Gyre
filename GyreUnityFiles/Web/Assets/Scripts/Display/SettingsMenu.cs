using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class SettingsMenu : MonoBehaviour
{
    bool menuOpened = false;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject MenuCamera;
    [SerializeField] GameObject StatsCamera;
    [SerializeField] GameObject PortraitStats;
    [SerializeField] GameObject LandscapeStats;
    [SerializeField] Color toggledOff;
    [SerializeField] Color toggledOn;
    [SerializeField] GameObject toggleShapes;
    [SerializeField] GameObject floatingShapes;
    private int mainDisplay = 1;
    private bool floatingShapesEnabled = true;
    private bool landscapeModeStats = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!menuOpened)
            {
                openMenu();
            }
            else
            {
                closeMenu();
            }
        }
    }

    void openMenu()
    {
        menuOpened = true;
        mainMenu.SetActive(true);
    }
    public void closeMenu()
    {
        menuOpened = false;
        mainMenu.SetActive(false);
    }

    public void openSettings()
    {
        settings.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void closeSettings()
    {
        settings.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void toggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void flipStatsMode()
    {
        if (landscapeModeStats)
        {
            PortraitStats.SetActive(true);
            LandscapeStats.SetActive(false);
            landscapeModeStats = false;
        }
        else
        {
            landscapeModeStats = true;
            PortraitStats.SetActive(false);
            LandscapeStats.SetActive(true);
        }
        
    }

    public void toggleFloatingShapes()
    {
        if (floatingShapesEnabled)
        {
            toggleShapes.GetComponent<ProceduralImage>().color = toggledOff;
            floatingShapes.SetActive(false);
            floatingShapesEnabled = false;
        }
        else
        {
            toggleShapes.GetComponent<ProceduralImage>().color = toggledOn;
            floatingShapes.SetActive(true);
            floatingShapesEnabled = true;
        }
        
    }

    public void flipDisplays()
    {
        switch (mainDisplay){
            case 2:
            mainDisplay = 1;
            MenuCamera.GetComponent<Camera>().targetDisplay = 0;
            StatsCamera.GetComponent<Camera>().targetDisplay = 1;
            break;
            default:
            mainDisplay = 2;
            MenuCamera.GetComponent<Camera>().targetDisplay = 1;
            StatsCamera.GetComponent<Camera>().targetDisplay = 0;
            break;
        }
        
    }

    public void exitApp()
    {
        Application.Quit();
    }
}
