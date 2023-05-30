using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParentpageMultiplayer : MonoBehaviour
{
    public Button[] gameButton = new Button[5];
    public Button playButton, bluetoothMultiplayerButton, onlineMultiplayerButton;
    public Button cancelButton;
    public RuntimeAnimatorController animatorControllerGame;
    public Button[] moveButton = new Button[3];
    int gameNumber = 0;
    int movesCount = 0;

    int sound = 0;
    public AudioSource audioSource;
    public AudioClip audioClip;

    public Sprite[] buttonSprite = new Sprite[2];
    int[] movesValues = { 30, 50, 100 };
    string[] gamenameList = new string[] { "3_Guti_Game", "6_Guti_Game", "16_Guti_Game", "laukatakati", "Pretwa" };
    string[] gameprefabsList = new string[] { "TinGuti", "SixGuti", "SholoGuti", "Laukatakati", "Laukatakati" };

    void Start()
    {
        moveButton[0].onClick.AddListener(onmove1Clicked);
        moveButton[1].onClick.AddListener(onmove2Clicked);
        moveButton[2].onClick.AddListener(onmove3Clicked);
        gameButton[0].onClick.AddListener(onPlaytinGuti);
        gameButton[1].onClick.AddListener(onPlaysixGuti);
        gameButton[2].onClick.AddListener(onPlaysholoGuti);
        gameButton[3].onClick.AddListener(onPlaylaukatakati);
        gameButton[4].onClick.AddListener(onPlayPretwa);
        cancelButton.onClick.AddListener(onCancelClicked);
        playButton.onClick.AddListener(onPlayClicked);
        bluetoothMultiplayerButton.onClick.AddListener(onBluetoothMultiplayerClicked);
        onlineMultiplayerButton.onClick.AddListener(ononlineMultiplayerClicked);

        movesCount = PlayerPrefs.GetInt("movesCountNum", 0);
        gameNumber = PlayerPrefs.GetInt("gameNumber", 0);

        sound = PlayerPrefs.GetInt("soundSettings", 1);

        moveButton[movesCount].GetComponent<Image>().sprite = buttonSprite[1];
        gameButton[gameNumber].GetComponent<Animator>().runtimeAnimatorController = animatorControllerGame;
    }


    void playButtonClickSound()
    {
        if (sound != 1) return;
        audioSource.PlayOneShot(audioClip);
    }

    void onBluetoothMultiplayerClicked()
    {
        playButtonClickSound();
#if UNITY_ANDROID && !UNITY_EDITOR
        showAndroidToastMessage("Coming Soon!");
#endif
    }


    void ononlineMultiplayerClicked()
    {
        playButtonClickSound();
#if UNITY_ANDROID && !UNITY_EDITOR
        showAndroidToastMessage("Coming Soon!");
#endif
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



    void onCancelClicked()
    {
        playButtonClickSound();
        SceneManager.LoadSceneAsync("Homepage");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onCancelClicked();
        }
    }


    void onPlayClicked()
    {
        playButtonClickSound();
        PlayerPrefs.SetInt("movesCount", movesValues[movesCount]);
        PlayerPrefs.SetInt(gameprefabsList[gameNumber], 0);
        SceneManager.LoadSceneAsync(gamenameList[gameNumber]);
    }


    void onmove1Clicked()
    {
        playButtonClickSound();
        if (movesCount == 0)
        {
            return;
        }

        moveButton[movesCount].GetComponent<Image>().sprite = buttonSprite[0];
        movesCount = 0;
        moveButton[movesCount].GetComponent<Image>().sprite = buttonSprite[1];
        PlayerPrefs.SetInt("movesCountNum", movesCount);
    }

    void onmove2Clicked()
    {
        playButtonClickSound();
        if (movesCount == 1)
        {
            return;
        }

        moveButton[movesCount].GetComponent<Image>().sprite = buttonSprite[0];
        movesCount = 1;
        moveButton[movesCount].GetComponent<Image>().sprite = buttonSprite[1];
        PlayerPrefs.SetInt("movesCountNum", movesCount);
    }

    void onmove3Clicked()
    {
        playButtonClickSound();
        if (movesCount == 2)
        {
            return;
        }

        moveButton[movesCount].GetComponent<Image>().sprite = buttonSprite[0];
        movesCount = 2;
        moveButton[movesCount].GetComponent<Image>().sprite = buttonSprite[1];
        PlayerPrefs.SetInt("movesCountNum", movesCount);
    }


    void onPlaytinGuti()
    {
        playButtonClickSound();
        if (gameNumber == 0)
        {
            return;
        }

        gameButton[gameNumber].GetComponent<Animator>().runtimeAnimatorController = null;
        gameNumber = 0;
        gameButton[gameNumber].GetComponent<Animator>().runtimeAnimatorController = animatorControllerGame;
        PlayerPrefs.SetInt("gameNumber", gameNumber);
    }


    void onPlaysixGuti()
    {
        playButtonClickSound();
        if (gameNumber == 1)
        {
            return;
        }

        gameButton[gameNumber].GetComponent<Animator>().runtimeAnimatorController = null;
        gameNumber = 1;
        gameButton[gameNumber].GetComponent<Animator>().runtimeAnimatorController = animatorControllerGame;
        PlayerPrefs.SetInt("gameNumber", gameNumber);
    }

    void onPlaysholoGuti()
    {
        playButtonClickSound();
        if (gameNumber == 2)
        {
            return;
        }

        gameButton[gameNumber].GetComponent<Animator>().runtimeAnimatorController = null;
        gameNumber = 2;
        gameButton[gameNumber].GetComponent<Animator>().runtimeAnimatorController = animatorControllerGame;
        PlayerPrefs.SetInt("gameNumber", gameNumber);
    }


    void onPlaylaukatakati()
    {
        playButtonClickSound();
        if (gameNumber == 3)
        {
            return;
        }

        gameButton[gameNumber].GetComponent<Animator>().runtimeAnimatorController = null;
        gameNumber = 3;
        gameButton[gameNumber].GetComponent<Animator>().runtimeAnimatorController = animatorControllerGame;
        PlayerPrefs.SetInt("gameNumber", gameNumber);
    }


    void onPlayPretwa()
    {
        playButtonClickSound();
        if (gameNumber == 4)
        {
            return;
        }

        gameButton[gameNumber].GetComponent<Animator>().runtimeAnimatorController = null;
        gameNumber = 4;
        gameButton[gameNumber].GetComponent<Animator>().runtimeAnimatorController = animatorControllerGame;
        PlayerPrefs.SetInt("gameNumber", gameNumber);
    }



}
