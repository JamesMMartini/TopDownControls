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
    [SerializeField] float projectionStep;
    [SerializeField] float afterImageStep;
    [SerializeField] float projectionDistance;
    [SerializeField] GameObject projectionImagePrefab;
    List<GameObject> afterImages = new List<GameObject>();
    float inputBlockTimer;

    List<Vector3> pastDirections = new List<Vector3>();

    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < 10; i++)
        {
            GameObject newAfterImage = GameObject.Instantiate(projectionImagePrefab);
            newAfterImage.transform.position = transform.position;
            afterImages.Add(newAfterImage);
        }
    }

    protected override void FixedUpdate()
    {
        if (inputBlockTimer > 0)
        {
            inputBlockTimer -= Time.fixedDeltaTime;
        }

        // Set the direction indicator
        Vector3 indicatorDirection = new Vector3(-inputX, -inputY, 0);

        pastDirections.Add(indicatorDirection);
        if (pastDirections.Count > 10)
        {
            pastDirections.RemoveAt(0);
        }

        Vector3 directionSum = pastDirections[0];
        for (int i = 1; i < pastDirections.Count; i++)
        {
            directionSum += pastDirections[i];
        }
        indicatorDirection = directionSum / pastDirections.Count;

        //directionIndicator.transform.up = indicatorDirection;
        //Vector3 newScale = directionIndicator.transform.localScale;
        //newScale.y = maxDirectionScale * indicatorDirection.magnitude;
        //directionIndicator.transform.localScale = newScale;

        if (inputX != 0 || inputY != 0)
        {
            foreach (GameObject image in afterImages)
            {
                image.SetActive(false);
            }

            Vector3 projectionPoint = transform.position;
            float currentProjection = 1f;
            bool foundBall = false;
            float afterImageDistance = 0f;
            int afterImageCount = 0;

            while (currentProjection < projectionDistance && !foundBall)
            {
                if (afterImageDistance > afterImageStep && afterImageCount < afterImages.Count)
                {
                    GameObject projectionImage = afterImages[afterImageCount];
                    projectionImage.SetActive(true);
                    projectionImage.transform.position = projectionPoint;
                    SpriteRenderer sprite = projectionImage.GetComponent<SpriteRenderer>();
                    Color newCol = sprite.color;
                    newCol.a = 1 - (currentProjection / projectionDistance);
                    sprite.color = newCol;

                    afterImageDistance = 0f;
                    afterImageCount++;
                }

                projectionPoint = transform.position + (indicatorDirection.normalized * currentProjection);
                List<Transform> bolCols = IsForcedPositionInBall(projectionPoint);

                foreach (Transform ballCol in bolCols)
                {
                    BallController ball = ballCol.GetComponent<BallController>();
                    if (ball != null)
                    {
                        Vector3 projDir = (ballCol.position - projectionPoint).normalized;

                        ball.SetProjectedDirection(projDir.x, projDir.y);

                        if (afterImageCount < afterImages.Count)
                        {
                            GameObject projectionImage = afterImages[afterImageCount];
                            projectionImage.SetActive(true);
                            projectionImage.transform.position = projectionPoint;
                            SpriteRenderer sprite = projectionImage.GetComponent<SpriteRenderer>();
                            Color newCol = sprite.color;
                            newCol.a = 1 - (currentProjection / projectionDistance);
                            sprite.color = newCol;

                            afterImageDistance = 0f;
                            afterImageCount++;
                        }

                        foundBall = true;
                    }
                }

                afterImageDistance += projectionStep;
                currentProjection += projectionStep;
            }
        }
        else
        {
            if (afterImages[0].activeInHierarchy)
            {
                foreach (GameObject image in afterImages)
                {
                    image.SetActive(false);
                }
            }
        }

        if (fireX == 0 && fireY == 0)
        {
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
        }
        else
        {
            velocity = indicatorDirection;

            if (velocity.magnitude > maxSpeed * Time.fixedDeltaTime)
            {
                velocity = velocity.normalized * maxSpeed * Time.fixedDeltaTime;
            }
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
                Time.timeScale = baseTimescale * 0.05f;
                Time.fixedDeltaTime = baseFixedDeltaTime * Time.timeScale;

                RescaleAllVelocities(Time.fixedDeltaTime, baseFixedDeltaTime);
            }
            else if (context.canceled)
            {
                float oldFixedDeltaTime = Time.fixedDeltaTime;

                Time.timeScale = baseTimescale;
                Time.fixedDeltaTime = baseFixedDeltaTime * Time.timeScale;

                RescaleAllVelocities(Time.fixedDeltaTime, oldFixedDeltaTime);

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

    public void RescaleAllVelocities(float newFixedDeltaTime, float oldFixedDeltaTime)
    {
        velocity = velocity / oldFixedDeltaTime;
        velocity = velocity * newFixedDeltaTime;

        foreach (BallController ball in ballList)
        {
            ball.RescaleVelocity(newFixedDeltaTime, oldFixedDeltaTime);
        }
    }

    //public void ChangeTimeScale(float newTimeScale)
    //{
    //    Time.timeScale = baseTimescale * newTimeScale;
    //    Time.fixedDeltaTime = baseFixedDeltaTime * Time.timeScale;

    //    RescaleAllVelocities(Time.fixedDeltaTime, baseFixedDeltaTime);
    //}
}
