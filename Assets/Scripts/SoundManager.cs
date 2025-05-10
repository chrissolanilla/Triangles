using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField]
    private AudioSource BGMSource;
    [SerializeField]
    private AudioSource BGMSourceAlternate;
    [SerializeField]
    private AudioSource SFXSource;
    [SerializeField]
    private AudioSource OverlapSource;
    [SerializeField]
    private List<string> alternateBGMSceneNames = new List<string>();
    [SerializeField]
    private Button muteButton;
    [SerializeField]
    Sprite unmuttedButtonSprite;
    [SerializeField]
    Sprite muttedButtonSprite;

    private AudioSource currentBGM;
    private bool BGMPlaying = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        BGMSource.loop = true;
        BGMSourceAlternate.loop = true;
        SetActiveBGMSource(SceneManager.GetActiveScene().name);
        SetMuteButton();
    }

    public void PlayButtonClick()
    {
        if (BGMPlaying) //only play when sounds are enabled as a whole
        {
            SFXSource.loop = false;
            SFXSource.Play();
        }        
    }

    public void ToggleBGM()
    {
        if (BGMPlaying) currentBGM.Pause();
        else currentBGM.Play();
        BGMPlaying = !BGMPlaying;
        SetMuteButton();
    }

    public void PlayOverlap()
    {
        if (BGMPlaying) OverlapSource.Play();
    }

    public void SetActiveBGMSource(string sceneName)
    {
        AudioSource newBGM;
        if (alternateBGMSceneNames.Contains(sceneName)) newBGM = BGMSourceAlternate;
        else newBGM = BGMSource;

        if(newBGM != currentBGM)
        {
            if(currentBGM) currentBGM.Pause();
            currentBGM = newBGM;
            if (BGMPlaying) currentBGM.Play();
        }
    }

    private void SetMuteButton()
    {
        if (!muteButton || !muttedButtonSprite || !unmuttedButtonSprite) return;

        if (BGMPlaying) muteButton.GetComponent<Image>().sprite = unmuttedButtonSprite;
        else muteButton.GetComponent<Image>().sprite = muttedButtonSprite;
    }
}
