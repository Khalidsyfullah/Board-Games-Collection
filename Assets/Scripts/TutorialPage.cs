using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialPage : MonoBehaviour
{
    int sound = 1, vibration = 1, soundSettings = 1;
    public AudioSource audioSource;
    public AudioClip audioClip1, audioClip2, audioClip3, audioClip4, audioClip5;

    //change
    GameObject[] grid_board = new GameObject[15];
    int[] grid_position = new int[15];

    public GameObject textTutorial;
    public GameObject tutorialPopup, tutorialSecond;
    public TextMeshProUGUI tutorialText, secondTutorialText;
    public Button tutorialNext, secondTutorialNext, secondTutorialPrev;
    public Button tutorialExit, playNewGame, showTextRules;
    bool isTutorialTextrunning = false;
    public Button tutorialTextCancel;

    public Sprite[] gutis = new Sprite[2];
    public GameObject mainParent;
    public GameObject parentObject;
    public GameObject bground;

    public GameObject pauseMenu;
    public GameObject resumeMenu;
    public Button resumeButton, restartButton, exitButton, cancelButton, soundOn, soundOff, vibrationOn, vibrationOff;
    public Button restartBtn, exButton, cancelButtonGameover;
    public TextMeshProUGUI text_pop, text_description;
    TextMeshPro turning_text, scorePlayer1Text, scorePlayer2Text, moveLeftText;
    public GameObject turning_object, scorePlayer1, scorePlayer2, moveLeft;
    public Button pause_object;
    bool isPaused = false;

    bool gameFinish = false;
    bool isSelected = false;

    int current_selected = -1;
    List<GameObject> possibleMove = new List<GameObject>();
    List<int> possibleAttack = new List<int>();

    int total_moves_count = 40;
    int movesCount = 40;
    int score1 = 0;
    int score2 = 0;

    public RuntimeAnimatorController animatorControllerBlue, animatorControllerRed;

    bool isTutorialRunning = true;
    int currentTutorialNumber = 0;

    string valf1 = "Good Job! You've destroyed opponent's guti! Keep it up and best of luck.......";
    string valf2 = "Oh no! Opponent has destroyed one of your guti!!!! Play carefully, you can still win this one.";

    string fggfinish = "Congratulations! You've successfully completed the tutorial. Now play a real game or play tutorial again. Remember, rules are same for every games, only rules are different....";
    int isfinish = 0;

    int istrigger1 = 0, istrigger2 = 0;

    int[] tutorialNumberlist = { 0, 0, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1};
    string[] tutorialTextList = {"Welcome! Here you'll learn how to play guti games. There are five different maps, but the rules are all same.",
                                 "Here you'll learn how to play six guti game. First let's get introduced with the User Interface.",
                                  "This game will be played between two players, one is Blue, other is RED. Here you are Blue team and Computer is RED team.", 
                                   "There are Six Blue Gutis and Six Red Gutis. At top there are score, initially it is zero. By destroying opponent's Guti you'll get score.", 
                                    "At bottom, it is the indication of whose turn it is. At right side, there is pause menu, from where you can adjust the settings.", 
                                    "At top there is the total moves count for the game. If it becomes zero, the match will end and player with highest point will win.", 
                                    "Now it's time to play. Press on any Blue Guti to make a move.", 
                                    "You've successfully selected a guti for moving! You can move it to any empty adjacent cells or select any other guti to move.", 
                                    "You've made your first move, now the computer will move.", 
                                    "The computer had moved. Now keep playing. Now learn the most important rule of the game: attacking opponent's guti.", 
                                     "You can attack opponent's guti if there is an empty cell next to opponent's guti. The cell must be connected by a direct line from your position.",
                                      "Once you attack your opponent's guti, that guti will be destroyed and your guti will be moved to that empty square. Also you'll get one point",
                                      "But also be sure to protect your gutis from opponent!",
                                      "The game will end when one player's all guti got destroyed or the move ended. The player with more score will win.", 
                                    "Now continue playing and try to win!"};

    //change
    int[,] grid_connector_secondary = {{-100,-1,1,-100,-1,-100, -100, -100, 4, -100, -100, -100, -100, -100, 100},
                        {-1, -100, -1, -100, -1, -100, -100, 4, -100, -100, -100, -100, -100, -100, -100},
                        {1,-1, -100, -100, -1, -100, 4, -100, -100, -100, -100, -100, -100, -100, -100},
                        {-100, -100, -100, -100, -1, 4, -1, -100, -100, 6, -100, -100, -100, -100, -100},
                        {-1, -1, - 1, -1, -100, -1, -1, -1, -1, -100, 7 ,-100, -100, -100, -100},
                        {-100, -100, -100, 4, -1, -100, -100, -100, -1, -100, -100, 8, -100, -100, -100},
                        {-100, -100, 4, -1, -1, -100, -100, -1, 7, -1, -1, -100, -100, -100, 10},
                        {-100, 4, -100, -100, -1, -100, -1, -100, -1, -100, -1, -100, -100, 10, -100},
                        {4, -100, -100, -100, -1, -1, 7, -1, -100, -100, -1, -1, 10, -100, -100},
                        {-100, -100, -100, 6, -100, -100, -1, -100, -100, -100, -1, 10, -100, -100, -100},
                        {-100, -100, -100, -100, 7, -100, -1, -1, -1, -1, -100, -1, -1, -1, -1},
                        {-100, -100, -100, -100, -100, 8, -100, -100, -1, 10, -1, -100, -100, -100, -100},
                        {-100, -100, -100, -100, -100, -100, -100, -100, 10, -100, -1, -100, -100, -1, 13},
                        {-100, -100, -100, -100, -100, -100, -100, 10, -100, -100, -1, -100, -1, -100, -1},
                        {-100, -100, -100, -100, -100, -100, 10, -100, -100, -100, -1, -100, 13, -1,-100}};



    List<GameObject> secondaryMove = new List<GameObject>();
    List<int> secondaryPossible = new List<int>();
    bool isSecondaryMove = false;
    int current_player = 1;

    void Start()
    {
        resizeScreen();


        pauseMenu.SetActive(false);
        resumeMenu.SetActive(false);

        resumeButton.onClick.AddListener(onResumeClicked);
        cancelButton.onClick.AddListener(onResumeClicked);
        restartButton.onClick.AddListener(onRestartClicked);
        restartBtn.onClick.AddListener(onRestartClicked);
        exButton.onClick.AddListener(onExitClicked);
        exitButton.onClick.AddListener(onExitClicked);
        pause_object.onClick.AddListener(onPauseGame);
        cancelButtonGameover.onClick.AddListener(onGameOverCancel);
        turning_text = turning_object.GetComponent<TextMeshPro>();
        scorePlayer1Text = scorePlayer1.GetComponent<TextMeshPro>();
        scorePlayer2Text = scorePlayer2.GetComponent<TextMeshPro>();
        moveLeftText = moveLeft.GetComponent<TextMeshPro>();

        soundOn.onClick.AddListener(soundOnclicked);
        soundOff.onClick.AddListener(soundOffclicked);
        vibrationOn.onClick.AddListener(vibrationOnclicked);
        vibrationOff.onClick.AddListener(vibratioffOnclicked);
        soundSettings = PlayerPrefs.GetInt("soundStatus", 2);
        vibration = PlayerPrefs.GetInt("vibrationStatus", 1);
        sound = PlayerPrefs.GetInt("soundSettings", 1);

        playSound(0);

        showTextRules.onClick.AddListener(onShowTextrules);
        tutorialTextCancel.onClick.AddListener(onShowtextCancel);
        tutorialNext.onClick.AddListener(ontutorialNextClicked);
        tutorialExit.onClick.AddListener(onExitTutorialClicked);
        playNewGame.onClick.AddListener(onPlayRealGame);
        tutorialSecond.SetActive(false);
        tutorialPopup.SetActive(true);
        textTutorial.SetActive(false);
        tutorialText.text = tutorialTextList[currentTutorialNumber];
        secondTutorialNext.onClick.AddListener(onSecondTutorialNextClicked);
        secondTutorialPrev.onClick.AddListener(onSecondTutorialPrevClicked);


        movesCount = PlayerPrefs.GetInt("movesCount", 20);
        total_moves_count = movesCount;
        moveLeftText.text = movesCount.ToString();


        string name = "Guti";
        for (int i = 0; i < grid_board.Length; i++)
        {
            string temp = name + " (" + i + ")";
            Transform childTransform = mainParent.transform.Find(temp);
            grid_board[i] = childTransform.gameObject;
        }


        for (int i = 0; i < grid_position.Length; i++)
        {
            grid_position[i] = 0;
        }


        //Initial Position
        //Blue Guti
        grid_position[9] = 1;
        grid_position[10] = 1;
        grid_position[11] = 1;
        grid_position[12] = 1;
        grid_position[13] = 1;
        grid_position[14] = 1;

        //Red Guti
        grid_position[0] = 2;
        grid_position[1] = 2;
        grid_position[2] = 2;
        grid_position[3] = 2;
        grid_position[4] = 2;
        grid_position[5] = 2;

        
        current_player = 1;

        turning_text.text = "Your Turn";
        turning_text.color = Color.blue;
        scorePlayer1Text.text = "Your Score:\n" + score1;
        scorePlayer2Text.text = "AI's Score:\n" + score2;
    }

    void onShowTextrules()
    {
        isTutorialTextrunning = true;
        textTutorial.SetActive(true);
    }


    void onShowtextCancel()
    {
        isTutorialTextrunning = false;
        textTutorial.SetActive(false);
    }


    void onGameOverCancel()
    {
        resumeMenu.SetActive(false);
    }


    void ontutorialNextClicked()
    {
        playButtonClickSound();
        if(isfinish == 1)
        {
            tutorialPopup.SetActive(false);
            return;
        }


        currentTutorialNumber++;
        if (tutorialNumberlist[currentTutorialNumber] == 0)
        {
            tutorialText.text = tutorialTextList[currentTutorialNumber];
        }
        else 
        {
            tutorialPopup.SetActive(false);
            tutorialSecond.SetActive(true);
            secondTutorialText.text =tutorialTextList[currentTutorialNumber];

            if(tutorialNumberlist[currentTutorialNumber] == 2)
            {
                isTutorialRunning = false;
                secondTutorialNext.gameObject.SetActive(false);
            }

        }
    }


    void onSecondTutorialNextClicked()
    {
        playButtonClickSound();
        if (istrigger1 == 1)
        {
            istrigger1 = 2;
            secondTutorialNext.gameObject.SetActive(true);
            secondTutorialPrev.gameObject.SetActive(false);
            secondTutorialText.text = valf1;
            isTutorialRunning = false;
            return;
        }


        if (istrigger2 == 1)
        {
            istrigger2 = 2;
            secondTutorialNext.gameObject.SetActive(true);
            secondTutorialPrev.gameObject.SetActive(false);
            secondTutorialText.text = valf2;
            isTutorialRunning = false;
            return;
        }


        currentTutorialNumber++;

        if(currentTutorialNumber >= 15)
        {
            tutorialSecond.SetActive(false);
            isTutorialRunning = false;
            return;
        }

        if (tutorialNumberlist[currentTutorialNumber] != 0)
        {
            secondTutorialText.text = tutorialTextList[currentTutorialNumber];
            if (tutorialNumberlist[currentTutorialNumber] == 2)
            {
                isTutorialRunning = false;
                secondTutorialNext.gameObject.SetActive(false);
                secondTutorialPrev.gameObject.SetActive(false);
            }
            else
            {
                secondTutorialNext.gameObject.SetActive(true);
                secondTutorialPrev.gameObject.SetActive(true);
            }
        }
        else
        {
            tutorialPopup.SetActive(true);
            tutorialSecond.SetActive(false);
            tutorialText.text = tutorialTextList[currentTutorialNumber];
        }
    }

    void onSecondTutorialPrevClicked()
    {
        playButtonClickSound();
        if (tutorialNumberlist[currentTutorialNumber-1] == 2)
        {
            return;
        }


        currentTutorialNumber--;
        if (tutorialNumberlist[currentTutorialNumber] != 0)
        {
            secondTutorialText.text = tutorialTextList[currentTutorialNumber];
        }
        else
        {
            tutorialPopup.SetActive(true);
            tutorialSecond.SetActive(false);
            tutorialText.text = tutorialTextList[currentTutorialNumber];
        }
    }



    void onExitTutorialClicked()
    {
        SceneManager.LoadSceneAsync("Homepage");
    }

    void onPlayRealGame()
    {
        playButtonClickSound();
        PlayerPrefs.SetInt("movesCount", 50);
        PlayerPrefs.SetInt("SixGuti", 1);
        SceneManager.LoadSceneAsync("6_Guti_Game");
    }



    void onPauseGame()
    {
        if (isPaused) return;
        playButtonClickSound();
        if (gameFinish)
        {
            resumeMenu.SetActive(true);
        }
        else
        {
            isPaused = true;
            updatePopupPanel();
            pauseMenu.SetActive(true);
        }
    }


    void gameEndSound()
    {
        if (sound == 1)
        {
            audioSource.PlayOneShot(audioClip5);
        }
        else if (vibration == 1)
        {
            VibrationManager.Vibrate();
        }
    }



    void playSound(int num)
    {
        if (sound != 1) return;
        if (num == 0)
        {
            audioSource.PlayOneShot(audioClip1);
        }
        else if (num == 1)
        {
            audioSource.PlayOneShot(audioClip2);
        }
        else
        {
            audioSource.PlayOneShot(audioClip3);
        }
    }

    void playVibration()
    {
        if (vibration != 1) return;
        VibrationManager.Vibrate(vibration);
    }


    void soundOnclicked()
    {
        sound = 1;
        playButtonClickSound();
        soundOff.GetComponent<Image>().color = Color.white;
        soundOn.GetComponent<Image>().color = Color.green;
    }

    void soundOffclicked()
    {
        sound = 2;
        soundOn.GetComponent<Image>().color = Color.white;
        soundOff.GetComponent<Image>().color = Color.green;
    }

    void vibrationOnclicked()
    {
        playButtonClickSound();
        vibration = 1;
        vibrationOff.GetComponent<Image>().color = Color.white;
        vibrationOn.GetComponent<Image>().color = Color.green;
    }

    void vibratioffOnclicked()
    {
        playButtonClickSound();
        vibration = 2;
        vibrationOn.GetComponent<Image>().color = Color.white;
        vibrationOff.GetComponent<Image>().color = Color.green;
    }


    void playButtonClickSound()
    {
        if (sound != 1) return;
        audioSource.PlayOneShot(audioClip4);
    }


    void soundManagerOperation()
    {
        if (current_player == 1)
        {
            if (soundSettings == 1)
            {
                playSound(1);
            }
            else if (soundSettings == 2)
            {
                playVibration();
            }
            else if (soundSettings == 3)
            {
                playVibration();
            }
            else if (soundSettings == 4)
            {
                playSound(1);
            }
            else
            {
                playVibration();
                playSound(1);
            }
        }

        else
        {
            if (soundSettings == 1)
            {
                playVibration();
            }
            else if (soundSettings == 2)
            {
                playSound(1);
            }
            else if (soundSettings == 3)
            {
                playVibration();
            }
            else if (soundSettings == 4)
            {
                playSound(2);
            }
            else
            {
                playVibration();
                playSound(2);
            }
        }
    }

    void updatePopupPrefs()
    {
        PlayerPrefs.SetInt("soundSettings", sound);
        PlayerPrefs.SetInt("vibrationStatus", vibration);
    }


    void updatePopupPanel()
    {
        if (sound == 1)
        {
            soundOff.GetComponent<Image>().color = Color.white;
            soundOn.GetComponent<Image>().color = Color.green;
        }
        else
        {
            soundOn.GetComponent<Image>().color = Color.white;
            soundOff.GetComponent<Image>().color = Color.green;
        }

        if (vibration == 1)
        {
            vibrationOff.GetComponent<Image>().color = Color.white;
            vibrationOn.GetComponent<Image>().color = Color.green;
        }
        else
        {
            vibrationOn.GetComponent<Image>().color = Color.white;
            vibrationOff.GetComponent<Image>().color = Color.green;
        }
    }



    void onExitClicked()
    {
        playButtonClickSound();
        SceneManager.LoadScene("Homepage");
    }

    void onRestartClicked()
    {
        playButtonClickSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void onResumeClicked()
    {
        playButtonClickSound();
        updatePopupPrefs();
        pauseMenu.SetActive(false);
        isPaused = false;
    }




    void Update()
    {
        if (isPaused) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isTutorialTextrunning)
            {
                isTutorialTextrunning = false;
                textTutorial.SetActive(false);
                return;
            }

            onPauseGame();
        }

        if(isTutorialRunning || isTutorialTextrunning) return;

        if (gameFinish) return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit.collider != null)
            {
                GameObject gamer = hit.collider.gameObject;
                SpriteRenderer spriteRenderer = gamer.GetComponent<SpriteRenderer>();

                if (current_player == 2)
                {
                    return;
                }

                for (int i = 0; i < grid_board.Length; i++)
                {
                    if (!isSelected && grid_board[i] == gamer && grid_position[i] == current_player && !isSecondaryMove)
                    {
                        current_selected = i;
                        spriteRenderer.color = Color.gray;
                        soundManagerOperation();
                        findMovesForSelectedGuti(i);
                        isSelected = true;
                        if(currentTutorialNumber == 6)
                        {
                            onSecondTutorialNextClicked();
                        }


                        return;
                    }
                    else if (isSelected && current_selected != -1 && grid_board[i] == gamer && grid_position[i] == current_player && i == current_selected && !isSecondaryMove)
                    {
                        if(currentTutorialNumber == 7)
                        {
                            return;
                        }
                        clearCurrentAnimation();
                        grid_board[current_selected].GetComponent<SpriteRenderer>().color = Color.white;
                        current_selected = -1;
                        isSelected = false;
                        return;
                    }

                    else if (isSelected && current_selected != -1 && grid_board[i] == gamer && grid_position[i] == current_player && i != current_selected && !isSecondaryMove)
                    {
                        clearCurrentAnimation();
                        grid_board[current_selected].GetComponent<SpriteRenderer>().color = Color.white;
                        current_selected = i;
                        spriteRenderer.color = Color.gray;
                        soundManagerOperation();
                        findMovesForSelectedGuti(i);
                        isSelected = true;
                        return;
                    }
                    else if (isSelected && current_selected != -1 && grid_board[i] == gamer && possibleMove.Count > 0 && possibleMove.Contains(gamer) && !isSecondaryMove)
                    {
                        if(currentTutorialNumber == 7)
                        {
                            onSecondTutorialNextClicked();
                        }

                        int index = possibleMove.IndexOf(gamer);
                        bool secondAttack = false;
                        if (index == -1)
                        {
                            return;
                        }
                        clearCurrentAnimation();

                        soundManagerOperation();
                        grid_board[current_selected].GetComponent<SpriteRenderer>().color = Color.white;
                        grid_board[current_selected].GetComponent<SpriteRenderer>().sprite = null;
                        grid_position[current_selected] = 0;
                        spriteRenderer.sprite = gutis[current_player - 1];
                        grid_position[i] = current_player;

                        if (possibleAttack[index] != -1)
                        {
                            grid_board[possibleAttack[index]].GetComponent<SpriteRenderer>().sprite = null;
                            grid_position[possibleAttack[index]] = 0;
                            score1 += 1;
                            scorePlayer1Text.text = "Your Score:\n" + score1;

                            if (isSecondAttackPossible(i))
                            {
                                secondAttack = true;
                            }

                        }


                        movesCount--;
                        moveLeftText.text = movesCount.ToString();

                        int num = checkifWin();

                        if (num != 0)
                        {
                            gameEndSound();
                            gameFinish = true;
                            StartCoroutine(showWinner(num));
                        }
                        else if (secondAttack && secondaryMove.Count > 0)
                        {
                            isSecondaryMove = true;
                            addAnimation(secondaryMove);
                            isSelected = true;
                            current_selected = i;
                            grid_board[i].GetComponent<SpriteRenderer>().color = Color.gray;
                        }

                        else
                        {
                            current_player = (current_player == 2) ? 1 : 2;
                            isSelected = false;
                            current_selected = -1;

                            turning_text.color = Color.red;
                            turning_text.text = "AI's Turn";
                            Invoke("AI_Turn_Easy", 1f);
                        }
                        return;

                    }

                    else if (isSelected && current_selected != -1 && grid_board[i] == gamer && secondaryMove.Count > 0 && secondaryMove.Contains(gamer) && isSecondaryMove)
                    {
                        if(istrigger1 == 0)
                        {
                            istrigger1 = 1;
                            isTutorialRunning = true;
                            tutorialSecond.SetActive(true);
                            secondTutorialText.text = valf1;
                            secondTutorialPrev.gameObject.SetActive(false);
                            secondTutorialNext.gameObject.SetActive(true);
                        }


                        bool secondAttack = false;
                        removeAnimation(secondaryMove);
                        soundManagerOperation();
                        grid_board[current_selected].GetComponent<SpriteRenderer>().color = Color.white;
                        grid_board[current_selected].GetComponent<SpriteRenderer>().sprite = null;
                        grid_position[current_selected] = 0;
                        spriteRenderer.sprite = gutis[current_player - 1];
                        grid_position[i] = current_player;

                        int index = secondaryMove.IndexOf(gamer);
                        grid_board[secondaryPossible[index]].GetComponent<SpriteRenderer>().sprite = null;
                        grid_position[secondaryPossible[index]] = 0;

                        score1 += 1;
                        scorePlayer1Text.text = "Your Score:\n" + score1;

                        movesCount--;
                        moveLeftText.text = movesCount.ToString();

                        if (isSecondAttackPossible(i))
                        {
                            secondAttack = true;
                        }

                        isSecondaryMove = false;
                        int num = checkifWin();

                        if (num != 0)
                        {
                            gameEndSound();
                            gameFinish = true;
                            StartCoroutine(showWinner(num));
                        }
                        else if (secondAttack && secondaryMove.Count > 0)
                        {
                            isSecondaryMove = true;
                            addAnimation(secondaryMove);
                            isSelected = true;
                            current_selected = i;
                            grid_board[i].GetComponent<SpriteRenderer>().color = Color.gray;
                        }

                        else
                        {
                            current_player = (current_player == 2) ? 1 : 2;
                            isSelected = false;
                            current_selected = -1;
                            turning_text.color = Color.red;
                            turning_text.text = "AI's Turn";
                            Invoke("AI_Turn_Easy", 1f);
                        }

                        return;

                    }

                }

            }
        }

    }


    void clearCurrentAnimation()
    {
        if (possibleMove.Count > 0)
        {
            foreach (GameObject g in possibleMove)
            {
                g.GetComponent<Animator>().runtimeAnimatorController = null;
            }
        }
    }


    void findMovesForSelectedGuti(int number)
    {
        if (possibleMove.Count > 0)
        {
            possibleMove.Clear();
            possibleAttack.Clear();
        }

        int num_opponemt = (current_player == 2) ? 1 : 2;

        for (int i = 0; i < grid_board.Length; i++)
        {
            if (grid_connector_secondary[number, i] == -1 && grid_position[i] == 0)
            {
                possibleMove.Add(grid_board[i]);
                possibleAttack.Add(-1);
            }

            else if (grid_connector_secondary[number, i] == -1 && grid_position[i] == num_opponemt)
            {
                for (int j = 0; j < grid_board.Length; j++)
                {
                    if (grid_connector_secondary[number, j] == i && grid_position[j] == 0)
                    {
                        possibleMove.Add(grid_board[j]);
                        possibleAttack.Add(i);
                    }

                }
            }
        }


        if (possibleMove.Count == 0)
        {
            return;
        }

        foreach (GameObject g in possibleMove)
        {
            if (current_player == 1)
            {
                g.GetComponent<Animator>().runtimeAnimatorController = animatorControllerBlue;
            }
            else
            {
                g.GetComponent<Animator>().runtimeAnimatorController = animatorControllerRed;
            }
        }

    }


    void addAnimation(List<GameObject> gg)
    {
        foreach (GameObject g in gg)
        {
            if (current_player == 1)
            {
                g.GetComponent<Animator>().runtimeAnimatorController = animatorControllerBlue;
            }
            else
            {
                g.GetComponent<Animator>().runtimeAnimatorController = animatorControllerRed;
            }
        }
    }

    void removeAnimation(List<GameObject> gg)
    {
        foreach (GameObject g in gg)
        {
            g.GetComponent<Animator>().runtimeAnimatorController = null;
        }
    }



    IEnumerator secondMoveCount(int prevPos)
    {
        yield return new WaitForSeconds(1f);

        int rand = UnityEngine.Random.Range(0, secondaryMove.Count);
        int currentPosition = getIndexofGO(secondaryMove[rand]);

        grid_board[prevPos].GetComponent<SpriteRenderer>().sprite = null;
        grid_position[prevPos] = 0;
        grid_board[currentPosition].GetComponent<SpriteRenderer>().sprite = gutis[current_player - 1];
        grid_position[currentPosition] = current_player;


        grid_board[secondaryPossible[rand]].GetComponent<SpriteRenderer>().sprite = null;
        grid_position[secondaryPossible[rand]] = 0;

        soundManagerOperation();

        movesCount--;
        moveLeftText.text = movesCount.ToString();

        int num = checkifWin();

        if (num != 0)
        {
            gameEndSound();
            gameFinish = true;
            StartCoroutine(showWinner(num));
        }

        else if (isSecondAttackPossible(currentPosition) && secondaryMove.Count > 0)
        {
            StartCoroutine(secondMoveCount(currentPosition));
        }

        else
        {
            current_player = (current_player == 2) ? 1 : 2;
            if (current_player == 2)
            {
                turning_text.color = Color.red;
                turning_text.text = "AI's Turn";
            }
            else
            {
                turning_text.color = Color.blue;
                turning_text.text = "Your Turn";
            }
        }

    }




    IEnumerator showWinner(int ridoy)
    {
        yield return new WaitForSeconds(2.5f);
        resumeMenu.SetActive(true);
        int mv = total_moves_count - movesCount;

        tutorialPopup.SetActive(true);
        isfinish = 1;
        tutorialText.text = fggfinish;



        turning_text.text = "Game Over";
        turning_text.color = Color.yellow;

        if (ridoy == 1)
        {
            string val = "Moves Played: " + mv + "\nYour Score: " + score1 + "\nAI's Score: " + score2 + "\nYou've Won by Destruction!";
            text_description.text = val;
            text_pop.text = "You're Winner!";
        }
        else if (ridoy == 2)
        {
            string val = "Moves Played: " + mv + "\nYour Score: " + score1 + "\nAI's Score: " + score2 + "\nAI Won by Destruction!";
            text_description.text = val;
            text_pop.text = "AI is Winner!";
        }


        else if (ridoy == 3)
        {

            string val = "Moves Played: " + mv + "\nYour Score: " + score1 + "\nAI's Score: " + score2;
            if (score1 > score2)
            {
                val += "\nYou've Won by More Points!";
                text_pop.text = "You're Winner!";
            }
            else if (score1 < score2)
            {
                val += "\nAI Won by More Points!";
                text_pop.text = "AI is Winner!";
            }
            else
            {
                val += "\nMatch Draw for Same Points!";
                text_pop.text = "Match Draw!";
            }

            text_description.text = val;
        }

        else if (ridoy == 4)
        {
            string val = "Moves Played: " + mv + "\nYour Score: " + score1 + "\nAI's Score: " + score2 + "\nWin by move blocking!";
            text_description.text = val;
            text_pop.text = "You're Winner!";
        }

        else
        {
            string val = "Moves Played: " + mv + "\nYour Score: " + score1 + "\nAI's Score: " + score2 + "\nAI Won by move blocking!";
            text_description.text = val;
            text_pop.text = "AI is Winner!";
        }


    }



    bool isSecondAttackPossible(int current_position)
    {
        int opponent = (current_player == 2) ? 1 : 2;
        bool flag = false;
        if (secondaryMove.Count > 0)
        {
            secondaryMove.Clear();
            secondaryPossible.Clear();
        }

        for (int i = 0; i < grid_board.Length; i++)
        {
            if (grid_connector_secondary[current_position, i] == -1 && grid_position[i] == opponent)
            {
                for (int j = 0; j < grid_board.Length; j++)
                {
                    if (grid_connector_secondary[current_position, j] == i && grid_position[j] == 0)
                    {
                        flag = true;
                        secondaryMove.Add(grid_board[j]);
                        secondaryPossible.Add(i);
                    }

                }
            }
        }

        return flag;
    }


    int getIndexofGO(GameObject gameObject)
    {
        for (int i = 0; i < grid_board.Length; i++)
        {
            if (gameObject == grid_board[i])
            {
                return i;
            }
        }

        return 0;
    }


    bool checkifMoveBlocked(int number)
    {
        int opponent = (number == 2) ? 1 : 2;

        for (int i = 0; i < grid_position.Length; i++)
        {
            if (grid_position[i] == number)
            {
                for (int j = 0; j < grid_position.Length; j++)
                {
                    if (grid_connector_secondary[i, j] == -1 && grid_position[j] == 0)
                    {
                        return false;
                    }
                    else if (grid_connector_secondary[i, j] == -1 && grid_position[j] == opponent)
                    {
                        for (int k = 0; k < grid_board.Length; k++)
                        {
                            if (grid_connector_secondary[i, k] == j && grid_position[k] == 0)
                            {
                                return false;
                            }

                        }
                    }

                }
            }

        }

        return true;
    }



    int checkifWin()
    {

        if (movesCount <= 0)
        {
            return 3;
        }


        int count1 = 0, count2 = 0;
        for (int i = 0; i < grid_position.Length; i++)
        {
            if (grid_position[i] == 1)
            {
                count1++;
            }

            if (grid_position[i] == 2)
            {
                count2++;
            }
        }

        if (count1 == 0)
        {
            return 2;
        }

        if (count2 == 0)
        {
            return 1;
        }

        if (checkifMoveBlocked(1))
        {
            return 4;
        }

        if (checkifMoveBlocked(2))
        {
            return 5;
        }

        return 0;

    }


    void AI_Turn_Easy()
    {
        List<GameObject> possibleValues = new List<GameObject>();
        List<int> possibleAttackValues = new List<int>();
        int current_position_move;
        int position_move = -1;
        bool isAttackDone = false;

        List<int> possibleMovesCollector = new List<int>();

        int indexofFirst = -1;

        int num_opponent = (current_player == 2) ? 1 : 2;

        for (int il = 0; il < grid_board.Length; il++)
        {
            if (grid_position[il] == current_player)
            {
                if (indexofFirst == -1)
                {
                    indexofFirst = il;
                }

                for (int i = 0; i < grid_board.Length; i++)
                {
                    if (grid_connector_secondary[il, i] == -1 && grid_position[i] == 0)
                    {
                        possibleMovesCollector.Add(il);
                        break;
                    }

                    else if (grid_connector_secondary[il, i] == -1 && grid_position[i] == num_opponent)
                    {
                        bool indicator = false;
                        for (int j = 0; j < grid_board.Length; j++)
                        {
                            if (grid_connector_secondary[il, j] == i && grid_position[j] == 0)
                            {
                                indicator = true;
                                possibleMovesCollector.Add(il);
                                break;
                            }

                        }
                        if (indicator)
                        {
                            break;
                        }
                    }
                }
            }
        }


        if (possibleMovesCollector.Count > 0)
        {
            int ranredX = UnityEngine.Random.Range(0, possibleMovesCollector.Count);
            current_position_move = possibleMovesCollector[ranredX];
        }
        else
        {
            current_position_move = indexofFirst;
        }



        for (int i = 0; i < grid_board.Length; i++)
        {
            if (grid_connector_secondary[current_position_move, i] == -1 && grid_position[i] == 0)
            {
                possibleValues.Add(grid_board[i]);
                possibleAttackValues.Add(-1);
            }

            else if (grid_connector_secondary[current_position_move, i] == -1 && grid_position[i] == num_opponent)
            {
                for (int j = 0; j < grid_board.Length; j++)
                {
                    if (grid_connector_secondary[current_position_move, j] == i && grid_position[j] == 0)
                    {
                        possibleValues.Add(grid_board[j]);
                        possibleAttackValues.Add(i);
                    }

                }
            }
        }


        if (possibleValues.Count > 0)
        {
            position_move = UnityEngine.Random.Range(0, possibleValues.Count);
        }

        if (currentTutorialNumber == 8)
        {
            isTutorialRunning = true;
            onSecondTutorialNextClicked();
        }


        int cur_pos = getIndexofGO(possibleValues[position_move]);

        if (position_move != -1 && current_position_move != -1)
        {
            grid_board[current_position_move].GetComponent<SpriteRenderer>().sprite = null;
            grid_position[current_position_move] = 0;
            possibleValues[position_move].GetComponent<SpriteRenderer>().sprite = gutis[current_player - 1];
            grid_position[cur_pos] = current_player;

            soundManagerOperation();
            if (possibleAttackValues[position_move] != -1)
            {

                if (istrigger2 == 0)
                {
                    istrigger2 = 1;
                    isTutorialRunning = true;
                    tutorialSecond.SetActive(true);
                    secondTutorialText.text = valf2;
                    secondTutorialPrev.gameObject.SetActive(false);
                    secondTutorialNext.gameObject.SetActive(true);
                }

                grid_board[possibleAttackValues[position_move]].GetComponent<SpriteRenderer>().sprite = null;
                grid_position[possibleAttackValues[position_move]] = 0;
                score2 += 1;
                scorePlayer2Text.text = "AI's Score:\n" + score2;
                if (isSecondAttackPossible(cur_pos))
                {
                    isAttackDone = true;
                }
            }
        }
        else
        {
            gameEndSound();
            gameFinish = true;
            StartCoroutine(showWinner(4));
            return;
        }

        movesCount--;
        moveLeftText.text = movesCount.ToString();

        int num = checkifWin();

        if (num != 0)
        {
            gameEndSound();
            gameFinish = true;
            StartCoroutine(showWinner(num));
        }
        else if (isAttackDone && secondaryMove.Count > 0)
        {
            StartCoroutine(secondMoveCount(cur_pos));
        }

        else
        {
            current_player = (current_player == 2) ? 1 : 2;
            if (current_player == 2)
            {
                turning_text.color = Color.red;
                turning_text.text = "AI's Turn";
            }
            else
            {
                turning_text.color = Color.blue;
                turning_text.text = "Your Turn";
            }
        }

    }

    void resizeScreen()
    {
        Renderer renderer = parentObject.GetComponent<Renderer>();
        Bounds bounds = renderer.bounds;
        Bounds bounds1 = bground.GetComponent<Renderer>().bounds;
        Vector3 sizeInScreenSpace = Camera.main.WorldToScreenPoint(bounds.size) - Camera.main.WorldToScreenPoint(Vector3.zero);
        Vector3 sizeInScreenSpace1 = Camera.main.WorldToScreenPoint(bounds1.size) - Camera.main.WorldToScreenPoint(Vector3.zero);
        float sizeInScreenUnitswidth = Screen.width / sizeInScreenSpace.x;
        float sizeInScreenUnitsheight = Screen.height / sizeInScreenSpace.y;
        float sizeInScreenUnitswidth1 = Screen.width / sizeInScreenSpace1.x;
        float sizeInScreenUnitsheight1 = Screen.height / sizeInScreenSpace1.y;

        if (sizeInScreenUnitswidth < sizeInScreenUnitsheight)
        {
            sizeInScreenUnitsheight = sizeInScreenUnitswidth;
        }
        else
        {
            sizeInScreenUnitswidth = sizeInScreenUnitsheight;
        }
        parentObject.transform.localScale = new Vector3(sizeInScreenUnitswidth, sizeInScreenUnitsheight, 1);
        bground.transform.localScale = new Vector3(sizeInScreenUnitswidth1, sizeInScreenUnitsheight1, 1);
    }
}

