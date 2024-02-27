using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float accelerationSpeed;
    [SerializeField] protected float counterAccelModifier;
    [SerializeField] protected float friction;
    [SerializeField] protected List<Transform> ballList = new List<Transform>();
    [SerializeField] protected List<Transform> wallList = new List<Transform>();
    [SerializeField] protected float movementIncrement;

    Vector3 startingPosition;

    protected Vector3 velocity;
    protected Vector3 acceleration;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (velocity != Vector3.zero)
        {
            if (velocity.magnitude > friction * Time.deltaTime)
            {
                velocity -= velocity.normalized * friction * Time.deltaTime;
            }
            else
            {
                velocity = Vector3.zero;
            }
        }

        Vector3 velocityThisFrame = velocity;

        Vector3 microVel;
        Vector3 nextPos = transform.position;

        bool velXPos = false;
        if (velocityThisFrame.x > 0)
            velXPos = true;

        bool velYPos = false;
        if (velocityThisFrame.y > 0)
            velYPos = true;

        while (velocityThisFrame != Vector3.zero)
        {
            // Run the wall collision checks
            if (Mathf.Abs(velocityThisFrame.x) > Mathf.Abs(velocityThisFrame.y))
            {
                velocityThisFrame.x -= MovementIncrementSigned(velXPos);
                microVel = new Vector3(MovementIncrementSigned(velXPos), 0, 0);

                if (IsPositionInWall(nextPos + microVel))
                {
                    //velocityThisFrame.x = 0f;
                    velocityThisFrame.x = -velocityThisFrame.x;
                    velocity.x = -velocity.x;
                    velXPos = !velXPos;
                }
                else
                {
                    nextPos += microVel;
                }


                if (velocityThisFrame.x < movementIncrement && velocityThisFrame.x > -movementIncrement)
                {
                    velocityThisFrame.x = 0f;
                }
            }
            else
            {
                velocityThisFrame.y -= MovementIncrementSigned(velYPos);
                microVel = new Vector3(0, MovementIncrementSigned(velYPos), 0);

                if (IsPositionInWall(nextPos + microVel))
                {
                    //velocityThisFrame.y = 0f;

                    velocityThisFrame.y = -velocityThisFrame.y;
                    velocity.y = -velocity.y;
                    velYPos = !velYPos;
                }
                else
                {
                    nextPos += microVel;
                }

                if (velocityThisFrame.y < movementIncrement && velocityThisFrame.y > -movementIncrement)
                {
                    velocityThisFrame.y = 0f;
                }
            }
        }

        transform.position = nextPos;

        // run the ball collision checks
        Transform ballCollision = IsPositionInBall();
        if (ballCollision != null)
        {
            Vector3 angleToBall = (ballCollision.position - transform.position).normalized;
            float angleDiff = Vector3.Angle(velocity, angleToBall);

            // Add force to the ball
            BallController ballCon = ballCollision.GetComponent<BallController>();
            if (ballCon != null)
            {
                ballCon.SetVelocity(angleToBall * velocity.magnitude);
                //ballCon.SetVelocity(angleToBall * (velocity.magnitude * (1 - (angleDiff / 90))));
            }

            // Modify the player's velocity
            if (angleDiff < 10)
            {
                velocity = Vector3.zero;
            }
            else
            {
                //Vector3 newVelocity = velocity - (angleToBall * (1 - (angleDiff / 90)));
                //newVelocity = newVelocity.normalized * velocity.magnitude;
                //velocity = newVelocity;

                velocity = -angleToBall * (velocity.magnitude);
            }

            Juice juice = GetComponent<Juice>();
            if (juice != null)
            {
                juice.Squish();
                juice.Hit();

            }

            PlayerBallController playerBall = GetComponent<PlayerBallController>();
            if (playerBall != null)
            {
                Camera.main.GetComponent<CameraJuice>().Shake();
            }
        }
    }

    protected float MovementIncrementSigned(bool positiveDir)
    {
        if (positiveDir)
        {
            return movementIncrement;
        }
        else
        {
            return -movementIncrement;
        }
    }

    protected Transform IsPositionInBall()
    {
        foreach (Transform ball in ballList)
        {
            float radius = transform.localScale.x / 2f;
            float otherRadius = ball.localScale.x / 2f;

            if (Mathf.Abs((transform.position - ball.position).magnitude) < radius + otherRadius)
            {
                return ball;
            }
        }

        return null;
    }

    protected bool IsPositionInWall(Vector3 position)
    {
        foreach (Transform wall in wallList)
        {
            float xDist = Mathf.Abs(position.x - wall.position.x);
            float yDist = Mathf.Abs(position.y - wall.position.y);

            float xMax = (transform.localScale.x / 2) + (wall.transform.localScale.x / 2);
            float yMax = (transform.localScale.y / 2) + (wall.transform.localScale.y / 2);

            if (xDist < xMax && yDist < yMax)
            {
                return true;
            }
        }

        return false;
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
    }

    public void ResetBall()
    {
        transform.position = startingPosition;
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
    }
}