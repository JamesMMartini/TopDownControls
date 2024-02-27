using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    [SerializeField] List<Transform> ballList;
    [SerializeField] Transform player;

    private void Update()
    {
        foreach (Transform ball in ballList)
        {
            if (CheckBallCollision(ball))
            {
                //Destroy(ball.gameObject);
                ball.position = new Vector3(0, 10, 0);
                ball.GetComponent<BallController>().SetVelocity(Vector3.zero);
                ball.GetComponent<Juice>().enabled = false;

                GetComponent<ParticleCreator>().CreateParticles(100);
            }
        }

        if (CheckBallCollision(player))
        {
            player.position = Vector3.zero;
        }
    }

    bool CheckBallCollision(Transform ball)
    {
        float radius = transform.localScale.x * 0.5f;
        float otherRadius = ball.localScale.x * 0.5f;

        if (Mathf.Abs((transform.position - ball.position).magnitude) < radius + otherRadius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
