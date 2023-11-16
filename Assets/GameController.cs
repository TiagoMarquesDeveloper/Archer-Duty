using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int targetCount;
    public GameObject startScreen;
    public GameObject gameScreen;
    public GameObject endScreen;


    public GameObject bow;
    public GameObject[] targets;


    // Start is called before the first frame update
    void Start()
    {
        openGame();
    }
    public void Reset()
    {
        startScreen.SetActive(false);
        gameScreen.SetActive(false);
        endScreen.SetActive(false);
        targetCount = 0;

        bow.SetActive(false);
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetCount >= 6)
        {
            gameOver();
        }
    }

    public void openGame()
    {
        Reset();
        startScreen.SetActive(true);

    }
    public void startGame()
    {
        restartGame();

    }
    public void restartGame()
    {
        Reset();
        gameScreen.SetActive(true);
        startScreen.SetActive(false);
        endScreen.SetActive(false);

        bow.SetActive(true);
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].SetActive(true);
        }

        ScoreManager.instance.score = 0;
    }
    public void gameOver()
    {
        Reset();
        endScreen.SetActive(true);

    }
}
