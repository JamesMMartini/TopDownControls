using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    Vector3 velocity;
    [SerializeField] List<Transform> wallList = new List<Transform>();
    [SerializeField] float movementIncrement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            velocity.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity.y -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity.x -= 1;
        }

        velocity = velocity.normalized * speed * Time.deltaTime;
        Vector3 microVel;
        Vector3 nextPos = transform.position;
        
        bool velXPos = false;
        if (velocity.x > 0)
            velXPos = true;

        bool velYPos = false;
        if (velocity.y > 0)
            velYPos = true;

        while (velocity != Vector3.zero)
        {
            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
            {
                velocity.x -= MovementIncrementSigned(velXPos);
                microVel = new Vector3(MovementIncrementSigned(velXPos), 0, 0);

                if (IsPositionInWall(nextPos + microVel))
                {
                    velocity.x = 0f;
                }
                else
                {
                    nextPos += microVel;
                }

                if (velocity.x < movementIncrement && velocity.x > -movementIncrement)
                {
                    velocity.x = 0f;
                }
            }
            else
            {
                velocity.y -= MovementIncrementSigned(velYPos);
                microVel = new Vector3(0, MovementIncrementSigned(velYPos), 0);

                if (IsPositionInWall(nextPos + microVel))
                {
                    velocity.y = 0f;
                }
                else
                {
                    nextPos += microVel;
                }

                if (velocity.y < movementIncrement && velocity.y > -movementIncrement)
                {
                    velocity.y = 0f;
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
