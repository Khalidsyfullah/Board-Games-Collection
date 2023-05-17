using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Guti_3_Game : MonoBehaviour
{
    int sound = 1, vibration = 1, soundSettings = 1;
    public AudioSource audioSource;
    public AudioClip audioClip1, audioClip2, audioClip3, audioClip4, audioClip5;

    //change
    GameObject[] grid_board = new GameObject[9];
    int[] grid_position = new int[9];


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

    //change
    int[,] grid_connector_secondary = { {-100, -1, 1, -1, -1, -100, 3, -100, 4 },
                               {-1, -100, -1, -100, -1, -100, -100, 4, -100 },
                                {1, -1, -100, -100, -1, -1, 4, -100, 5 },
                                {-1, -100, -100, -100, -1, 4, -1, -100, -100},
                                {-1, -1, -1, -1, -100, -1, -1,-1,-1},
                                {-100, -100, -1, 4, -1, -100, -100, -100, -1 },
                                {3, -100, 4, -1, -1, -100, -100, -1, 7},
                                {-100, 4, -100, -100, -1, -100, -1, -100, -1},
                                {4, -100, 5, -100, -1, -1, 7, -1, -100}};



    List<GameObject> secondaryMove = new List<GameObject>();
    List<int> secondaryPossible = new List<int> ();
    bool isSecondaryMove = false;

    int settings = 0;
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
        cancelButtonGameover.onClick.AddListener (onGameOverCancel);
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


        movesCount = PlayerPrefs.GetInt("movesCount", 20);
        total_moves_count = movesCount;
        moveLeftText.text = movesCount.ToString();

        //change
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
        grid_position[5] = 1;
        grid_position[7] = 1;
        grid_position[8] = 1;

        //Red Guti
        grid_position[0] = 2;
        grid_position[1] = 2;
        grid_position[3] = 2;

        settings = PlayerPrefs.GetInt("TinGuti", 3);
        current_player = UnityEngine.Random.Range(1, 3);

        if (settings == 0)
        {
            if (current_player == 2)
            {
                turning_text.text = "RED's Turn";
                turning_text.color = Color.red;
            }
            else
            {
                turning_text.text = "Blue's Turn";
                turning_text.color = Color.blue;
            }

            scorePlayer1Text.text = "Blue's Score:\n"+score1;
            scorePlayer2Text.text = "Red's Score:\n"+score2;
        }
        else
        {
            if (current_player == 2)
            {
                turning_text.text = "AI's Turn";
                turning_text.color = Color.red;
                if (settings == 1)
                {
                    Invoke("AI_Turn_Easy", 1f);
                }
                else if (settings == 2)
                {
                    Invoke("AI_Turn_Medium", 1f);
                }
                else
                {
                    Invoke("AI_Turn_Hard", 1f);
                }
            }
            else
            {
                turning_text.text = "Your Turn";
                turning_text.color = Color.blue;
            }

            scorePlayer1Text.text = "Your Score:\n"+score1;
            scorePlayer2Text.text = "AI's Score:\n"+score2;
        }


    }

    void onGameOverCancel()
    {
        resumeMenu.SetActive(false);
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
        SceneManager.LoadScene("ParentPage");
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
            onPauseGame();
        }

        if (gameFinish) return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit.collider != null)
            {
                GameObject gamer = hit.collider.gameObject;
                SpriteRenderer spriteRenderer = gamer.GetComponent<SpriteRenderer>();

                if(settings != 0 && current_player == 2)
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
                        return;
                    }
                    else if (isSelected && current_selected != -1 && grid_board[i] == gamer && grid_position[i] == current_player && i == current_selected && !isSecondaryMove)
                    {
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
                            if(settings == 0)
                            {
                                if (current_player == 1)
                                {
                                    score1 += 1;
                                    scorePlayer1Text.text = "Blue's Score:\n" + score1;
                                }
                                else
                                {
                                    score2 += 1;
                                    scorePlayer2Text.text = "Red's Score:\n" + score2;
                                }
                            }

                            else
                            {
                                score1 += 1;
                                scorePlayer1Text.text = "Your Score:\n" + score1;
                            }

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
                            if(settings == 0)
                            {
                                if (current_player == 2)
                                {
                                    turning_text.color = Color.red;
                                    turning_text.text = "Red's Turn";
                                }
                                else
                                {
                                    turning_text.color = Color.blue;
                                    turning_text.text = "Blue's Turn";
                                }
                            }
                            else
                            {
                                turning_text.color = Color.red;
                                turning_text.text = "AI's Turn";

                                if (settings == 1)
                                {
                                    Invoke("AI_Turn_Easy", 1f);
                                }
                                else if (settings == 2)
                                {
                                    Invoke("AI_Turn_Medium", 1f);
                                }
                                else
                                {
                                    Invoke("AI_Turn_Hard", 1f);
                                }
                            }
                        }
                        return;

                    }

                    else if (isSelected && current_selected != -1 && grid_board[i] == gamer && secondaryMove.Count > 0 && secondaryMove.Contains(gamer) && isSecondaryMove)
                    {
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

                        if (settings == 0)
                        {
                            if (current_player == 1)
                            {
                                score1 += 1;
                                scorePlayer1Text.text = "Blue's Score:\n" + score1;
                            }
                            else
                            {
                                score2 += 1;
                                scorePlayer2Text.text = "Red's Score:\n" + score2;
                            }
                        }

                        else
                        {
                            score1 += 1;
                            scorePlayer1Text.text = "Your Score:\n" + score1;
                        }

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
                            if (settings == 0)
                            {
                                if (current_player == 2)
                                {
                                    turning_text.color = Color.red;
                                    turning_text.text = "Red's Turn";
                                }
                                else
                                {
                                    turning_text.color = Color.blue;
                                    turning_text.text = "Blue's Turn";
                                }
                            }
                            else
                            {
                                turning_text.color = Color.red;
                                turning_text.text = "AI's Turn";

                                if (settings == 1)
                                {
                                    Invoke("AI_Turn_Easy", 1f);
                                }
                                else if (settings == 2)
                                {
                                    Invoke("AI_Turn_Medium", 1f);
                                }
                                else
                                {
                                    Invoke("AI_Turn_Hard", 1f);
                                }
                            }
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
        if(possibleMove.Count > 0)
        {
            possibleMove.Clear();
            possibleAttack.Clear();
        }

        int num_opponemt = (current_player == 2) ? 1 : 2;

        for(int i=0; i<grid_board.Length; i++)
        {
            if (grid_connector_secondary[number, i] == -1 && grid_position[i] == 0)
            {
                possibleMove.Add(grid_board[i]);
                possibleAttack.Add(-1);
            }

            else if(grid_connector_secondary[number, i] == -1 && grid_position[i] == num_opponemt)
            {
                for(int j =0; j<grid_board.Length; j++)
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

        foreach(GameObject g in possibleMove)
        {
            if(current_player == 1)
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

        turning_text.text = "Game Over";
        turning_text.color = Color.yellow;

        if (ridoy == 1)
        {
            if(settings == 0)
            {
                string val = "Moves Played: " + mv + "\nBlue's Score: " + score1 + "\nRed's Score: " + score2 + "\nBlue Won by Destruction!";
                text_description.text = val;
                text_pop.text = "Blue is Winner!";
            }
            else
            {
                string val = "Moves Played: " + mv + "\nYour Score: " + score1 + "\nAI's Score: " + score2 + "\nYou've Won by Destruction!";
                text_description.text = val;
                text_pop.text = "You're Winner!";
                updateStatistics(0);

            }
        }
        else if(ridoy == 2)
        {
            if (settings == 0)
            {
                string val = "Moves Played: " + mv + "\nBlue's Score: " + score1 + "\nRed's Score: " + score2 + "\nRed Won by Destruction!";
                text_description.text = val;
                text_pop.text = "Red is Winner!";
            }
            else
            {
                string val = "Moves Played: " + mv + "\nYour Score: " + score1 + "\nAI's Score: " + score2 + "\nAI Won by Destruction!";
                text_description.text = val;
                text_pop.text = "AI is Winner!";
                updateStatistics(1);
            }
        }


        else if (ridoy == 3)
        {
            
            if (settings == 0)
            {
                string val = "Moves Played: " + mv + "\nBlue's Score: " + score1 + "\nRed's Score: " + score2;
                if(score1 > score2)
                {
                    val += "\nBlue Won by More Points!";
                    text_pop.text = "Blue is Winner!";
                }
                else if(score1 < score2)
                {
                    val += "\nRed Won by More Points!";
                    text_pop.text = "Red is Winner!";
                }
                else
                {
                    val += "\nMatch Draw for Same Points!";
                    text_pop.text = "Match Draw!";
                }

                text_description.text = val;

            }
            else
            {
                string val = "Moves Played: " + mv + "\nYour Score: " + score1 + "\nAI's Score: " + score2;
                if (score1 > score2)
                {
                    updateStatistics(0);
                    val += "\nYou've Won by More Points!";
                    text_pop.text = "You're Winner!";
                }
                else if (score1 < score2)
                {
                    updateStatistics(1);
                    val += "\nAI Won by More Points!";
                    text_pop.text = "AI is Winner!";
                }
                else
                {
                    updateStatistics(2);
                    val += "\nMatch Draw for Same Points!";
                    text_pop.text = "Match Draw!";
                }

                text_description.text = val;
            }
        }

        else if (ridoy == 4)
        {
            resumeMenu.SetActive(true);
            
            if (settings == 0)
            {
                string val = "Moves Played: " + mv+ "\nBlue's Score: "+score1+ "\nRed's Score: "+score2 +"\nWin by move blocking!";
                text_description.text = val;
                text_pop.text = "Blue is Winner!";
            }
            else
            {
                updateStatistics(0);
                string val = "Moves Played: " + mv + "\nYour Score: " + score1 + "\nAI's Score: " + score2 + "\nWin by move blocking!";
                text_description.text = val;
                text_pop.text = "You're Winner!";
            }
        }

        else
        {
            resumeMenu.SetActive(true);
            
            if (settings == 0)
            {
                string val = "Moves Played: " + mv + "\nBlue's Score: " + score1 + "\nRed's Score: " + score2 + "\nRed Won by move blocking!";
                text_description.text = val;
                text_pop.text = "Red is Winner!";
            }
            else
            {
                updateStatistics(1);
                string val = "Moves Played: " + mv + "\nYour Score: " + score1 + "\nAI's Score: " + score2 + "\nAI Won by move blocking!";
                text_description.text = val;
                text_pop.text = "AI is Winner!";
            }
        }

        
    }



    bool isSecondAttackPossible(int current_position)
    {
        int opponent = (current_player == 2) ? 1 : 2;
        bool flag = false;
        if(secondaryMove.Count > 0)
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
        for(int i=0; i<grid_board.Length; i++)
        {
            if(gameObject == grid_board[i])
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



    void updateStatistics(int number)
    {
        if (settings == 0) return;
        string[] strings = {"tingutistateasy", "tingutistatmedium", "tingutistathard" };
        int total_game = PlayerPrefs.GetInt(strings[settings - 1] + "total", 0);
        int total_won = PlayerPrefs.GetInt(strings[settings - 1] + "won", 0);
        int total_lost = PlayerPrefs.GetInt(strings[settings - 1] + "lost", 0);
        

        total_game++;
        if(number == 0)
        {
            total_won++;
        }
        else if(number == 1)
        {
            total_lost++;
        }

        PlayerPrefs.SetInt(strings[settings - 1] + "total", total_game);
        PlayerPrefs.SetInt(strings[settings - 1] + "won", total_won);
        PlayerPrefs.SetInt(strings[settings - 1] + "lost", total_lost);
    }



    int checkifWin()
    {

        if(movesCount <= 0)
        {
            return 3;
        }


        int count1 = 0, count2 = 0;
        for(int i=0; i < grid_position.Length; i++)
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

        if(count1 == 0)
        {
            return 2;
        }

        if(count2 == 0)
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
        int current_position_move = -1;
        int position_move = -1;
        bool isAttackDone = false;


        int num_opponent = (current_player == 2) ? 1 : 2;

        for (int il = 0; il < grid_board.Length; il++)
        {
            if (grid_position[il] == current_player)
            {
                current_position_move = il;

                for (int i = 0; i < grid_board.Length; i++)
                {
                    if (grid_connector_secondary[il, i] == -1 && grid_position[i] == 0)
                    {
                        possibleValues.Add(grid_board[i]);
                        possibleAttackValues.Add(-1);
                    }

                    else if (grid_connector_secondary[il, i] == -1 && grid_position[i] == num_opponent)
                    {
                        for (int j = 0; j < grid_board.Length; j++)
                        {
                            if (grid_connector_secondary[il, j] == i && grid_position[j] == 0)
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
                    break;
                }
            }
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
                grid_board[possibleAttackValues[position_move]].GetComponent<SpriteRenderer>().sprite = null;
                grid_position[possibleAttackValues[position_move]] = 0;
                score2 += 1;
                scorePlayer2Text.text = "AI's Score:\n"+score2;
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


    void AI_Turn_Hard()
    {
        int length_gameboard = grid_position.Length;
        List<int> position_own = new List<int>();

        for(int i=0; i<length_gameboard; i++)
        {
            if (grid_position[i] == current_player)
            {
                position_own.Add(i);
            }
        }

        List<DataFormat> dataFormats = getScoreForCurrentMove(grid_position, position_own);

        if(dataFormats.Count == 0)
        {
            gameEndSound();
            gameFinish = true;
            StartCoroutine(showWinner(4));
            return;
        }
        else
        {
            movesCount--;
            moveLeftText.text = movesCount.ToString();
            soundManagerOperation();
            grid_position[dataFormats[0].initialPosition] = 0;
            grid_position[dataFormats[0].movesList] = current_player;
            grid_board[dataFormats[0].initialPosition].GetComponent<SpriteRenderer>().sprite = null;
            grid_board[dataFormats[0].movesList].GetComponent<SpriteRenderer>().sprite = gutis[current_player - 1];
            if (dataFormats[0].attackIndex != -1)
            {
                grid_position[dataFormats[0].attackIndex] = 0;
                grid_board[dataFormats[0].attackIndex].GetComponent<SpriteRenderer>().sprite = null;
                score2++;
                scorePlayer2Text.text = "AI's Score:\n" + score2;
            }

            if(dataFormats.Count == 1)
            {
                int num = checkifWin();

                if (num != 0)
                {
                    gameEndSound();
                    gameFinish = true;
                    StartCoroutine(showWinner(num));
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
            else
            {
                StartCoroutine(hardmodeMoveController(dataFormats, 1));
            }
        }

    }

    IEnumerator hardmodeMoveController(List<DataFormat> datas, int number)
    {
        yield return new WaitForSeconds(1f);

        movesCount--;
        moveLeftText.text = movesCount.ToString();
        soundManagerOperation();
        grid_position[datas[number].initialPosition] = 0;
        grid_position[datas[number].movesList] = current_player;
        grid_board[datas[number].initialPosition].GetComponent<SpriteRenderer>().sprite = null;
        grid_board[datas[number].movesList].GetComponent<SpriteRenderer>().sprite = gutis[current_player - 1];

        if(datas[number].attackIndex != -1)
        {
            grid_position[datas[number].attackIndex] = 0;
            grid_board[datas[number].attackIndex].GetComponent<SpriteRenderer>().sprite = null;
            score2++;
            scorePlayer2Text.text = "AI's Score:\n" + score2;
        }

        if (datas.Count == number+1)
        {
            int num = checkifWin();

            if (num != 0)
            {
                gameEndSound();
                gameFinish = true;
                StartCoroutine(showWinner(num));
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
        else
        {
            StartCoroutine(hardmodeMoveController(datas, number+1));

        }

    }



    List<DataFormat> getScoreForCurrentMove(int[] move_calculation, List<int> move_items)
    {
        List<DataFormat> score_toreturn = new List<DataFormat>();
        int currentNetScore = -100;
        int opponent = (current_player == 2) ? 1 : 2;
        int length_gameboard = move_calculation.Length;

        foreach (int currentPosition in move_items)
        {
            for (int i = 0; i < length_gameboard; i++)
            {
                if (grid_connector_secondary[currentPosition, i] == -1 && move_calculation[i] == 0)
                {
                    int[] temp_move_calculation = new int[length_gameboard];
                    for (int t = 0; t < length_gameboard; t++)
                    {
                        temp_move_calculation[t] = move_calculation[t];
                    }

                    temp_move_calculation[currentPosition] = 0;
                    temp_move_calculation[i] = current_player;
                    int num = opponentDepthCalculator(opponent, temp_move_calculation);
                    int netPoint = 0 - num;

                    //Debug.Log("Move: "+ currentPosition+" To: "+i+"\nOwn Depth: 0 Opponent Depth: "+num);

                    int tempUp = UnityEngine.Random.Range(0, 3);
                    if (netPoint > currentNetScore || (netPoint == currentNetScore && tempUp!= 2))
                    {
                        currentNetScore = netPoint;
                        score_toreturn.Clear();
                        DataFormat score = new DataFormat();
                        score.initialPosition = currentPosition;
                        score.attackIndex = -1;
                        score.movesList = i;
                        score_toreturn.Add(score);
                    }

                }

                else if (grid_connector_secondary[currentPosition, i] == -1 && move_calculation[i] == opponent)
                {
                    for (int j = 0; j < length_gameboard; j++)
                    {
                        if (grid_connector_secondary[currentPosition, j] == i && grid_position[j] == 0)
                        {
                            List<DataFormat> datas = currentBestDepthForMoves(current_player, opponent, move_calculation, currentPosition);
                            int[] temp_move_calculation = new int[length_gameboard];

                            for (int t = 0; t < length_gameboard; t++)
                            {
                                temp_move_calculation[t] = move_calculation[t];
                            }

                            foreach (DataFormat data in datas)
                            {
                                temp_move_calculation[data.initialPosition] = 0;
                                temp_move_calculation[data.attackIndex] = 0;
                                temp_move_calculation[data.movesList] = current_player;

                            }
                            int val = opponentDepthCalculator(opponent, temp_move_calculation);
                            //Debug.Log("Move By Attack: " + currentPosition + " To: " + datas[datas.Count-1].movesList + "\nOwn Depth: "+datas.Count +"Opponent Depth: " + val);
                            int net_val = datas.Count - val;
                            int tempUp = UnityEngine.Random.Range(0, 3);
                            if (net_val > currentNetScore || (net_val == currentNetScore && tempUp!= 0))
                            {
                                currentNetScore = net_val;
                                score_toreturn.Clear();
                                score_toreturn.AddRange(datas);
                            }

                        }
                    }
                }
            }
        }


        return score_toreturn;
    }


    List<DataFormat> currentBestDepthForMoves(int currentPlayer, int opponent, int[] moves, int cur_pos)
    {
        List<DataFormat> bestDepthForMoves = new List<DataFormat>();
        for (int i = 0; i < moves.Length; i++)
        {
            if (moves[i] == opponent && grid_connector_secondary[cur_pos, i] == -1)
            {
                for (int j = 0; j < moves.Length; j++)
                {
                    if (moves[j] == 0 && grid_connector_secondary[cur_pos, j] == i)
                    {
                        List<DataFormat> newList = new List<DataFormat>();
                        DataFormat df = new DataFormat();
                        df.initialPosition = cur_pos;
                        df.movesList = j;
                        df.attackIndex = i;

                        int[] tempValues = new int[moves.Length];

                        for(int t=0; t<moves.Length; t++)
                        {
                            tempValues[t] = moves[t];
                        }

                        tempValues[i] = 0;
                        tempValues[j] = current_player;
                        tempValues[cur_pos] = 0;
                        newList.Add(df);
                        newList.AddRange(currentBestDepthForMoves(currentPlayer, opponent, tempValues, j));
                        if (newList.Count > bestDepthForMoves.Count)
                        {
                            bestDepthForMoves.Clear();
                            bestDepthForMoves.AddRange(newList);
                        }
                    }
                }
            }
        }


        return bestDepthForMoves;
    }


    int opponentDepthCalculator(int curPlayer, int[] move_grid)
    {
        int deptLevel = 0;
        int opponent = (curPlayer == 2) ? 1 : 2;
        int length_gameboard = move_grid.Length;

        for (int il = 0; il<length_gameboard; il++)
        {
            if (move_grid[il] == curPlayer)
            {
                int depth = currentBestDepth(curPlayer, opponent, move_grid, il);
                if (depth > deptLevel)
                {
                    deptLevel = depth;
                }
            }
        }


        return deptLevel;
    }


    int currentBestDepth(int currentPlayer, int opponent, int[] moves, int cur_pos)
    {
        int depth = 0;
        for(int i=0; i<moves.Length; i++)
        {
            if (moves[i] == opponent && grid_connector_secondary[cur_pos, i] == -1)
            {
                for(int j=0; j<moves.Length; j++)
                {
                    if (moves[j] == 0 && grid_connector_secondary[cur_pos,j]== i)
                    {
                        int[] tempValues = new int[moves.Length];

                        for(int t= 0; t<moves.Length; t++)
                        {
                            tempValues[t] = moves[t];
                        }

                        tempValues[i] = 0;
                        tempValues[j] = current_player;
                        tempValues[cur_pos] = 0;

                        int tempdepth = 1+ currentBestDepth(currentPlayer, opponent, tempValues, j);
                        if (tempdepth > depth)
                        {
                            depth = tempdepth;
                        }
                    }
                }
            }
        }


        return depth;
    }


    struct DataFormat
    {
        public int initialPosition;
        public int movesList;
        public int attackIndex;
    }


    void AI_Turn_Medium()
    {
        int number = UnityEngine.Random.Range(0, 4);
        if(number == 0)
        {
            AI_Turn_Easy();
        }
        else
        {
            AI_Turn_Hard();
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
