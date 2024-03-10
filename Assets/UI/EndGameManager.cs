using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] GameObject endMenu;
    [SerializeField] TMP_Text timerScore;

    PlayerBallController player;
    float timer;
    bool timerStarted;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerBallController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStarted)
        {
            BallController[] allBalls = GameObject.FindObjectsOfType<BallController>();
            int activeBallCount = 0;
            foreach (BallController ball in allBalls)
                if (ball.isActiveOnBoard)
                    activeBallCount++;

            if (activeBallCount <= 1)
            {
                //end the game
                PlayerInput input = FindObjectOfType<PlayerInput>();
                input.SwitchCurrentActionMap("Menu");

                endMenu.SetActive(true);

                timerStarted = false;
                player.InternalResetBalls();

                timerScore.text = string.Format("{0:0.00}", timer) + " seconds";

                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            if (player.isActiveOnBoard)
                timerStarted = true;
        }
    }
}
