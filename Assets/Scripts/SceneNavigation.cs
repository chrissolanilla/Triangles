using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{    public void LoadScene(string sceneName)
    {
        GameObject manager = GameObject.FindWithTag("SoundManager");
        if (manager)
        {
            SoundManager soundManager = manager.GetComponent<SoundManager>();
            if (soundManager) SoundManager.instance.SetActiveBGMSource(sceneName);
        }
        SceneManager.LoadScene(sceneName);
    }

    public void EnterLevel(string levelName)
    {
        PlayerStatistics.EnterLevel(levelName);
    }

    public void LeaveLevel()
    {
        PlayerStatistics.StopLevelTimer();
    }

    public void OpenInfoPage()
    {
        //hard coded because we don't want links to be variables for security reasons
        Application.OpenURL("https://www.researchgate.net/publication/366311257_Supporting_All_Students_Development_of_Geometric_Understanding_2021_NCTM_Virtual_Conference_Activities_for_the_Triangle_Puzzle");
    }

    public void RequestButtonClickSound()
    {
        GameObject manager = GameObject.FindWithTag("SoundManager");
        if (manager)
        {
            SoundManager soundManager = manager.GetComponent<SoundManager>();
            if (soundManager) SoundManager.instance.PlayButtonClick();
        }
    }
    public void RequestToggleMute()
    {
        GameObject manager = GameObject.FindWithTag("SoundManager");
        if (manager)
        {
            SoundManager soundManager = manager.GetComponent<SoundManager>();
            if (soundManager) SoundManager.instance.ToggleBGM();
        }
    }
}
