using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    [SerializeField] List<BallController> ballList = new List<BallController>();
    [SerializeField] BallController player;

    private void Start()
    {
        BallController[] allBalls = GameObject.FindObjectsOfType<BallController>();
        foreach (BallController ball in allBalls)
        {
            PlayerBallController playerB = ball.GetComponent<PlayerBallController>();
            if (playerB != null)
            {
                player = ball;
            }
            else
            {
                ballList.Add(ball);
            }
        }
    }

    private void Update()
    {
        foreach (BallController ball in ballList)
        {
            if (CheckBallCollision(ball))
            {
                //Destroy(ball.gameObject);
                ball.transform.position = new Vector3(0, 10, 0);
                ball.GetComponent<BallController>().SetVelocity(Vector3.zero);
                ball.GetComponent<Juice>().enabled = false;
                ball.GetComponent<AudioSource>().enabled = false;
                ball.isActiveOnBoard = false;

                GetComponent<ParticleCreator>().CreateParticles(100);
            }
        }

        if (CheckBallCollision(player))
        {
            //player.GetComponent<PlayerBallController>().ResetBall();
            player.transform.position = Vector3.zero;
        }
    }

    bool CheckBallCollision(BallController ball)
    {
        float radius = transform.localScale.x * 0.5f;
        float otherRadius = ball.baseScale.x * 0.5f;

        if (Mathf.Abs((transform.position - ball.transform.position).magnitude) < radius + otherRadius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
