using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ParentPageScript : MonoBehaviour
{
    public Button[] moveButton = new Button[3];
    public Button[] difficultyButton = new Button[3];
    public Button[] modeButton = new Button[4];
    public Button playButton;
    public TextMeshProUGUI textMeshPro;

    int movesCount = 0;
    int diifculty = 0;
    int mode = 0;

    int[] movesValues = {20, 40, 80 };
    void Start()
    {
        moveButton[0].onClick.AddListener(onmove1Clicked);
        moveButton[1].onClick.AddListener(onmove2Clicked);
        moveButton[2].onClick.AddListener(onmove3Clicked);
        difficultyButton[0].onClick.AddListener(ondiff1Clicked);
        difficultyButton[1].onClick.AddListener(ondiff2Clicked);
        difficultyButton[2].onClick.AddListener(ondiff3Clicked);
        modeButton[0].onClick.AddListener(ondmode1Clicked);
        modeButton[1].onClick.AddListener (ondmode2Clicked);
        modeButton[2].onClick.AddListener(ondmode3Clicked);
        modeButton[3].onClick.AddListener(ondmode4Clicked);
        playButton.onClick.AddListener(onPlayClicked);
        moveButton[movesCount].GetComponent<Image>().color = Color.gray;
        difficultyButton[diifculty].GetComponent<Image>().color = Color.gray;
        modeButton[mode].GetComponent<Image>().color = Color.gray;
    }

    void onmove1Clicked()
    {
        moveButton[movesCount].GetComponent<Image>().color = Color.white;
        movesCount = 0;
        moveButton[movesCount].GetComponent<Image>().color = Color.gray;
    }

    void onmove2Clicked()
    {
        moveButton[movesCount].GetComponent<Image>().color = Color.white;
        movesCount = 1;
        moveButton[movesCount].GetComponent<Image>().color = Color.gray;
    }

    void onmove3Clicked()
    {
        moveButton[movesCount].GetComponent<Image>().color = Color.white;
        movesCount = 2;
        moveButton[movesCount].GetComponent<Image>().color = Color.gray;
    }

    void ondiff1Clicked()
    {
        difficultyButton[diifculty].GetComponent<Image>().color = Color.white;
        diifculty = 0;
        difficultyButton[diifculty].GetComponent<Image>().color = Color.gray;
    }

    void ondiff2Clicked()
    {
        difficultyButton[diifculty].GetComponent<Image>().color = Color.white;
        diifculty = 1;
        difficultyButton[diifculty].GetComponent<Image>().color = Color.gray;
    }

    void ondiff3Clicked()
    {
        difficultyButton[diifculty].GetComponent<Image>().color = Color.white;
        diifculty = 2;
        difficultyButton[diifculty].GetComponent<Image>().color = Color.gray;
    }


    void ondmode1Clicked()
    {
        modeButton[mode].GetComponent<Image>().color = Color.white;
        mode = 0;
        modeButton[mode].GetComponent<Image>().color = Color.gray;
    }

    void ondmode2Clicked()
    {
        modeButton[mode].GetComponent<Image>().color = Color.white;
        mode = 1;
        modeButton[mode].GetComponent<Image>().color = Color.gray;
    }

    void ondmode3Clicked()
    {
        modeButton[mode].GetComponent<Image>().color = Color.white;
        mode = 2;
        modeButton[mode].GetComponent<Image>().color = Color.gray;
    }

    void ondmode4Clicked()
    {
        modeButton[mode].GetComponent<Image>().color = Color.white;
        mode = 3;
        modeButton[mode].GetComponent<Image>().color = Color.gray;
    }

    void onPlayClicked()
    {
        PlayerPrefs.SetInt("movesCount", movesValues[movesCount]);

        if(mode != 0)
        {
            PlayerPrefs.SetInt("TinGuti", 0);
        }
        else
        {
            PlayerPrefs.SetInt("TinGuti", diifculty+1);
        }
        SceneManager.LoadSceneAsync("3_Guti_Game");
    }
}
