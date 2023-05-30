using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomepageScript : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public Toggle[] toggles = new Toggle[5];
    bool isFocus = false;
    bool isProcessing = false;
    public GameObject exitScreen;
    public GameObject settingsPanel;
    public Button yesButton, noButton;
    public Button profilebutton, settingsbutton, statisticsbutton, settingsQuit, shareButton, moreAppsButton, gameQuitButton, cancelGameQuit;
    public AudioClip audioClipMusic, audioClipSound;
    public AudioSource audioSource;
    int soundStatus = 1, vibrationStatus = 1, soundSettings = 1;
    public Button soundOn, soundOff, vibrationOn, vibrationOff;
    public Button saveButton, onSinglePlayer, onMultiplayer, onTutorial;


    void Start()
    {
        exitScreen.SetActive(false);
        settingsPanel.SetActive(false);
        soundStatus = PlayerPrefs.GetInt("soundStatus", 1);
        vibrationStatus = PlayerPrefs.GetInt("vibrationStatus", 1);
        soundSettings = PlayerPrefs.GetInt("soundSettings", 1);

        yesButton.onClick.AddListener(onYesPressed);
        noButton.onClick.AddListener(onNoPressed);
        profilebutton.onClick.AddListener(onProfilleClicked);
        settingsbutton.onClick.AddListener(onSettingsClicked);
        statisticsbutton.onClick.AddListener(onStatisticsClicked);
        settingsQuit.onClick.AddListener(onSettingsCancel);
        saveButton.onClick.AddListener(onSettingsQuit);
        gameQuitButton.onClick.AddListener(onGameQuit);
        moreAppsButton.onClick.AddListener(onMoreAppsClicked);
        shareButton.onClick.AddListener(onShareAppClicked);
        soundOn.onClick.AddListener(soundOnclicked);
        soundOff.onClick.AddListener(soundOffclicked);
        vibrationOn.onClick.AddListener(vibrationOnclicked);
        vibrationOff.onClick.AddListener(vibratioffOnclicked);
        onSinglePlayer.onClick.AddListener(onSinglePlayerClicked);
        onMultiplayer.onClick.AddListener(onMultiPlayerClicked);
        onTutorial.onClick.AddListener(onTutorialClicked);
        cancelGameQuit.onClick.AddListener(onCancelGameClickQuit);
        playMusicSound();

    }


    void onSinglePlayerClicked()
    {
        playButtonClickSound();
        SceneManager.LoadSceneAsync("ParentPage");
    }

    void onMultiPlayerClicked()
    {
        playButtonClickSound();
        SceneManager.LoadSceneAsync("ParentPage_multiplayer");
    }

    void onTutorialClicked()
    {
        SceneManager.LoadSceneAsync("TutorialPage");
    }


    void onCancelGameClickQuit()
    {
        playButtonClickSound();
        exitScreen.SetActive(false);
    }

    void soundOnclicked()
    {
        soundSettings = 1;
        playButtonClickSound();
        soundOff.GetComponent<Image>().color = Color.white;
        soundOn.GetComponent<Image>().color = Color.green;
    }

    void soundOffclicked()
    {
        soundSettings = 2;
        soundOn.GetComponent<Image>().color = Color.white;
        soundOff.GetComponent<Image>().color = Color.green;
    }

    void vibrationOnclicked()
    {
        playButtonClickSound();
        vibrationStatus = 1;
        vibrationOff.GetComponent<Image>().color = Color.white;
        vibrationOn.GetComponent<Image>().color = Color.green;
    }

    void vibratioffOnclicked()
    {
        playButtonClickSound();
        vibrationStatus = 2;
        vibrationOn.GetComponent<Image>().color = Color.white;
        vibrationOff.GetComponent<Image>().color = Color.green;
    }

    void onMoreAppsClicked()
    {
        playButtonClickSound();
#if UNITY_ANDROID && !UNITY_EDITOR
        showAndroidToastMessage("Follow our Play Store Developer Page for More Apps by us!");
#endif
    }


    void onShareAppClicked()
    {
#if UNITY_ANDROID
        if (!isProcessing)
        {
            StartCoroutine(ShareTextInAnroid());
        }
#else
        //Debug.Log("No sharing set up for this platform.");
#endif
    }




    void playButtonClickSound()
    {
        if (soundSettings == 1)
        {
            audioSource.PlayOneShot(audioClipSound);
        }
    }



    void playMusicSound()
    {
        if (soundSettings == 1)
        {
            audioSource.PlayOneShot(audioClipMusic);
        }
    }




    void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }

#if UNITY_ANDROID
    public IEnumerator ShareTextInAnroid()
    {
        var shareSubject = "I am in Love with this game";
        var shareMessage = "I think you should try this game. " +
                           "Download it from: " +
                           "https://play.google.com/store/apps/details?id=com.akapps.pnpgames";
        isProcessing = true;
        if (!Application.isEditor)
        {

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), shareSubject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage);
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share your high score");


            currentActivity.Call("startActivity", chooser);


        }
        yield return new WaitUntil(() => isFocus);
        isProcessing = false;
    }


#endif


    void onGameQuit()
    {
        playButtonClickSound();
        exitScreen.SetActive(true);
    }



#if UNITY_ANDROID && !UNITY_EDITOR
    private void showAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
#endif


    void vibrationM()
    {
        if (vibrationStatus == 1)
        {
            VibrationManager.Vibrate();
        }
    }


    void onSettingsCancel()
    {
        playButtonClickSound();
        soundStatus = PlayerPrefs.GetInt("soundStatus", 1);
        vibrationStatus = PlayerPrefs.GetInt("vibrationStatus", 1);
        soundSettings = PlayerPrefs.GetInt("soundSettings", 1);
        settingsPanel.SetActive(false);
    }


    void onSettingsQuit()
    {
        playButtonClickSound();
        Toggle activeToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (activeToggle != null)
        {
            if (activeToggle.name == "Toggle (0)")
            {
                soundStatus = 1;
            }
            else if (activeToggle.name == "Toggle (1)")
            {
                soundStatus = 2;
            }
            else if (activeToggle.name == "Toggle (2)")
            {
                soundStatus = 3;
            }
            else if (activeToggle.name == "Toggle (3)")
            {
                soundStatus = 4;
            }
            else
            {
                soundStatus = 5;
            }
        }

        PlayerPrefs.SetInt("soundStatus", soundStatus);
        PlayerPrefs.SetInt("soundSettings", soundSettings);
        PlayerPrefs.SetInt("vibrationStatus", vibrationStatus);

        settingsPanel.SetActive(false);
    }


    void onSettingsClicked()
    {
        playButtonClickSound();
        if (soundSettings == 1)
        {
            soundOff.GetComponent<Image>().color = Color.white;
            soundOn.GetComponent<Image>().color = Color.green;
        }
        else
        {
            soundOn.GetComponent<Image>().color = Color.white;
            soundOff.GetComponent<Image>().color = Color.green;
        }

        if (vibrationStatus == 1)
        {
            vibrationOff.GetComponent<Image>().color = Color.white;
            vibrationOn.GetComponent<Image>().color = Color.green;
        }
        else
        {
            vibrationOn.GetComponent<Image>().color = Color.white;
            vibrationOff.GetComponent<Image>().color = Color.green;
        }

        toggles[soundStatus - 1].isOn = true;

        settingsPanel.SetActive(true);
    }

    void onStatisticsClicked()
    {
        playButtonClickSound();
        //statisticsPanel.SetActive(true);
#if UNITY_ANDROID && !UNITY_EDITOR
        showAndroidToastMessage("Coming Soon! Please Wait for the Upcoming Updates!");
#endif
    }


    void onYesPressed()
    {
        Application.Quit();
    }

    void onNoPressed()
    {
        playButtonClickSound();
        exitScreen.SetActive(false);
    }


    void onProfilleClicked()
    {
        playButtonClickSound();
        //statisticsPanel.SetActive(true);
#if UNITY_ANDROID && !UNITY_EDITOR
        showAndroidToastMessage("Coming Soon! Please Wait for the Upcoming Updates!");
#endif
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playButtonClickSound();
            if (settingsPanel.activeSelf)
            {
                onSettingsQuit();
            }
            else
            {
                exitScreen.SetActive(true);
            }
        }
    }
}
