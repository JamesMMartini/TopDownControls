using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRemake : MonoBehaviour
{
    //[SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [SerializeField] float accelerationSpeed;
    [SerializeField] float counterAccelModifier;
    [SerializeField] float friction;
    [SerializeField] List<Transform> wallList = new List<Transform>();
    [SerializeField] float movementIncrement;

    Vector3 velocity;
    Vector3 acceleration;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //velocity = Vector3.zero;
        acceleration = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            acceleration.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            acceleration.y -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            acceleration.x += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            acceleration.x -= 1;
        }

        acceleration = acceleration.normalized * accelerationSpeed * Time.deltaTime;
        float angleVelVsAcc = Vector3.Angle(velocity, acceleration);
        float counterPushRatio = angleVelVsAcc / 180f;

        velocity += acceleration + (acceleration * counterPushRatio * counterAccelModifier);

        if (velocity.magnitude > maxSpeed * Time.deltaTime)
        {
            velocity = velocity.normalized * maxSpeed * Time.deltaTime;
        }

        if (acceleration == Vector3.zero)
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
            if (Mathf.Abs(velocityThisFrame.x) > Mathf.Abs(velocityThisFrame.y))
            {
                velocityThisFrame.x -= MovementIncrementSigned(velXPos);
                microVel = new Vector3(MovementIncrementSigned(velXPos), 0, 0);

                if (IsPositionInWall(nextPos + microVel))
                {
                    velocityThisFrame.x = 0f;
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
                    velocityThisFrame.y = 0f;
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
    }

    float MovementIncrementSigned(bool positiveDir)
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

    bool IsPositionInWall(Vector3 position)
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
}
