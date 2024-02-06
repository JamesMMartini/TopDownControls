using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBallController : BallController
{
    float inputX;
    float inputY;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        //velocity = Vector3.zero;
        acceleration = Vector3.zero;

        acceleration.x = inputX;
        acceleration.y = inputY;

        acceleration = acceleration.normalized * accelerationSpeed * Time.deltaTime;
        float angleVelVsAcc = Vector3.Angle(velocity, acceleration);
        float counterPushRatio = angleVelVsAcc / 180f;

        velocity += acceleration + (acceleration * counterPushRatio * counterAccelModifier);

        if (velocity.magnitude > maxSpeed * Time.deltaTime)
        {
            velocity = velocity.normalized * maxSpeed * Time.deltaTime;
        }

        base.Update();

        //if (acceleration == Vector3.zero)
        //{
        //    if (velocity.magnitude > friction * Time.deltaTime)
        //    {
        //        velocity -= velocity.normalized * friction * Time.deltaTime;
        //    }
        //    else
        //    {
        //        velocity = Vector3.zero;
        //    }
        //}

        //Vector3 velocityThisFrame = velocity;

        //Vector3 microVel;
        //Vector3 nextPos = transform.position;

        //bool velXPos = false;
        //if (velocityThisFrame.x > 0)
        //    velXPos = true;

        //bool velYPos = false;
        //if (velocityThisFrame.y > 0)
        //    velYPos = true;

        //while (velocityThisFrame != Vector3.zero)
        //{
        //    // Run the wall collision checks
        //    if (Mathf.Abs(velocityThisFrame.x) > Mathf.Abs(velocityThisFrame.y))
        //    {
        //        velocityThisFrame.x -= MovementIncrementSigned(velXPos);
        //        microVel = new Vector3(MovementIncrementSigned(velXPos), 0, 0);

        //        if (IsPositionInWall(nextPos + microVel))
        //        {
        //            //velocityThisFrame.x = 0f;

        //            velocityThisFrame.x = -velocityThisFrame.x;
        //            velocity.x = -velocity.x;
        //            velXPos = !velXPos;
        //        }
        //        else
        //        {
        //            nextPos += microVel;
        //        }


        //        if (velocityThisFrame.x < movementIncrement && velocityThisFrame.x > -movementIncrement)
        //        {
        //            velocityThisFrame.x = 0f;
        //        }
        //    }
        //    else
        //    {
        //        velocityThisFrame.y -= MovementIncrementSigned(velYPos);
        //        microVel = new Vector3(0, MovementIncrementSigned(velYPos), 0);

        //        if (IsPositionInWall(nextPos + microVel))
        //        {
        //            //velocityThisFrame.y = 0f;

        //            velocityThisFrame.y = -velocityThisFrame.y;
        //            velocity.y = -velocity.y;
        //            velYPos = !velYPos;
        //        }
        //        else
        //        {
        //            nextPos += microVel;
        //        }

        //        if (velocityThisFrame.y < movementIncrement && velocityThisFrame.y > -movementIncrement)
        //        {
        //            velocityThisFrame.y = 0f;
        //        }
        //    }
        //}

        //transform.position = nextPos;

        //// run the ball collision checks
        //Transform ballCollision = IsPositionInBall();
        //if (ballCollision != null)
        //{
        //    Vector3 angleToBall = (ballCollision.position - transform.position).normalized;
        //    float angleDiff = Vector3.Angle(velocity, angleToBall);

        //    // Add force to the ball
        //    BallController ballCon = ballCollision.GetComponent<BallController>();
        //    if (ballCon != null)
        //    {
        //        ballCon.SetVelocity(angleToBall * velocity.magnitude);
        //        //ballCon.SetVelocity(angleToBall * (velocity.magnitude * (1 - (angleDiff / 90))));
        //    }

        //    // Modify the player's velocity
        //    if (angleDiff < 10)
        //    {
        //        velocity = Vector3.zero;
        //    }
        //    else
        //    {
        //        //Vector3 newVelocity = velocity - (angleToBall * (1 - (angleDiff / 90)));
        //        //newVelocity = newVelocity.normalized * velocity.magnitude;
        //        //velocity = newVelocity;

        //        velocity = -angleToBall * (velocity.magnitude);
        //    }

        //}
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 moveVec = context.ReadValue<Vector2>();

        if (Mathf.Abs(moveVec.x) > 0.1f)
        {
            inputX = moveVec.x;
        }
        else
        {
            inputX = 0f;
        }

        if (Mathf.Abs(moveVec.y) > 0.1f)
        {
            inputY = moveVec.y;
        }
        else
        {
            inputY = 0f;
        }
    }

    public void ResetBalls(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            BallController[] balls = GameObject.FindObjectsOfType<BallController>();
            foreach (BallController ball in balls)
            {
                ball.ResetBall();
            }
        }
    }
}
