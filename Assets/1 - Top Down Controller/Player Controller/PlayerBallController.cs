using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBallController : BallController
{
    float inputX;
    float inputY;

    float fireX;
    float fireY;

    [SerializeField] GameObject directionIndicator;
    [SerializeField] float maxDirectionScale;
    [SerializeField] float maxDirectionRotationSpeed;
    [SerializeField] float maxFireValue;

    [SerializeField] float maxInputBlock;
    float inputBlockTimer;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (inputBlockTimer > 0)
        {
            inputBlockTimer -= Time.fixedDeltaTime;
        }

        // Set the direction indicator
        Vector3 indicatorDirection = new Vector3(-inputX, -inputY, 0);

        float angle = Vector3.Angle(directionIndicator.transform.up, indicatorDirection);
        if (angle < maxDirectionRotationSpeed)
        {
            directionIndicator.transform.up = indicatorDirection;
            Vector3 newScale = directionIndicator.transform.localScale;
            newScale.y = maxDirectionScale * indicatorDirection.magnitude;
            directionIndicator.transform.localScale = newScale;
        }
        else
        {
            directionIndicator.transform.up = Vector3.RotateTowards(directionIndicator.transform.up, indicatorDirection, maxDirectionRotationSpeed * Mathf.Deg2Rad, 0.5f);
            Vector3 newScale = directionIndicator.transform.localScale;
            newScale.y = maxDirectionScale * indicatorDirection.magnitude;
            directionIndicator.transform.localScale = newScale;
        }

        

        //velocity = Vector3.zero;
        acceleration = Vector3.zero;

        //acceleration.x = inputX;
        //acceleration.y = inputY;
        acceleration.x = fireX;
        acceleration.y = fireY;

        acceleration = acceleration * accelerationSpeed * Time.fixedDeltaTime;
        float angleVelVsAcc = Vector3.Angle(velocity, acceleration);
        float counterPushRatio = angleVelVsAcc / 180f;

        velocity += acceleration + (acceleration * counterPushRatio * counterAccelModifier);

        if (velocity.magnitude > maxSpeed * Time.fixedDeltaTime)
        {
            velocity = velocity.normalized * maxSpeed * Time.fixedDeltaTime;
        }

        if (fireX != 0 || fireY != 0)
        {
            fireX = 0f;
            fireY = 0f;
            inputX = 0f;
            inputY = 0f;

            inputBlockTimer = maxInputBlock;
        }

        base.FixedUpdate();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (inputBlockTimer <= 0f)
        {
            Vector2 moveVec = context.ReadValue<Vector2>();

            inputX = moveVec.x;
            inputY = moveVec.y;

            //if (Mathf.Abs(moveVec.x) > 0.1f)
            //{
            //    inputX = moveVec.x;
            //}
            //else
            //{
            //    inputX = 0f;
            //}

            //if (Mathf.Abs(moveVec.y) > 0.1f)
            //{
            //    inputY = moveVec.y;
            //}
            //else
            //{
            //    inputY = 0f;
            //}
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (inputBlockTimer <= 0f)
        {
            if (context.performed)
            {
                fireX = -inputX * maxFireValue;
                fireY = -inputY * maxFireValue;
            }
        }
    }

    public void ResetBalls(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            BallController[] balls = GameObject.FindObjectsOfType<BallController>();
            foreach (BallController ball in balls)
            {
                ball.GetComponent<Juice>().enabled = true;
                ball.GetComponent<AudioSource>().enabled = true;
                ball.ResetBall();
            }
        }
    }

    public override void ResetBall()
    {
        base.ResetBall();

        inputX = 0f;
        inputY = 0f;

        fireX = 0f;
        fireY = 0f;

        inputBlockTimer = maxInputBlock;
    }
}
