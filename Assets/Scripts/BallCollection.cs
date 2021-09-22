using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallCollection : MonoBehaviour
{
    public float ballSpeed = 0;
    public float ballDistance = 0;
    List<Ball> Balls = new List<Ball>();
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public GameObject ballPrefab;
    BallCollection forwardCollection;
    BallCollection backwardCollection;
    void CreateBall()
    {
        GameObject prefab = Instantiate(ballPrefab);
        Ball ball = prefab.GetComponent<Ball>();
        ball.distanceTravelled = 0;
        AddBall(ball, 0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateBall();
        }
        foreach (Ball ball in Balls)
        {
            MoveBall(ball);
        }
    }

    void MoveBall(Ball ball)
    {
        if (ball.backwardBall == null)
        {
            if (!IsLastBall(ball))
            {
                // ball has no ball behind it and is not the ball furthest back
                // that means we should set the last ball because it is null
                ball.backwardBall = FindBackBall(ball);

            }
        }

        // if we are too far away from the ball behind us we should move back
        // we should not go back if this is the last ball
        if (!IsLastBall(ball) && BallTooFar(ball, ball.backwardBall))
        {
            GoBackward(ball);
        }
        // otherwise move forward
        else
        {
            // probably go forward
            GoForward(ball);
        }

    }

    void AddBall(Ball ball, int index)
    {
        Balls.Insert(index, ball);
        ball.forwardBall = FindForwardBall(ball);
        ball.backwardBall = FindBackBall(ball);
    }

    void GoForward(Ball ball)
    {
        ball.distanceTravelled += ballSpeed * Time.deltaTime;
        ball.transform.position = pathCreator.path.GetPointAtDistance(ball.distanceTravelled, endOfPathInstruction);
        ball.transform.rotation = pathCreator.path.GetRotationAtDistance(ball.distanceTravelled, endOfPathInstruction);
    }
    void GoBackward(Ball ball)
    {
        ball.distanceTravelled -= ballSpeed * 10 * Time.deltaTime;
        ball.transform.position = pathCreator.path.GetPointAtDistance(ball.distanceTravelled, endOfPathInstruction);
        ball.transform.rotation = pathCreator.path.GetRotationAtDistance(ball.distanceTravelled, endOfPathInstruction);
    }

    Ball FindBackBall(Ball ball)
    {
        int index = Balls.IndexOf(ball);
        if (index <= 0)
        {
            return null;
        }
        return Balls[index - 1];
    }
    Ball FindForwardBall(Ball ball)
    {
        int index = Balls.IndexOf(ball);
        if (Balls.ElementAtOrDefault(index + 1) == null)
        {
            return null;
        }
        return Balls[index + 1];
    }
    bool IsLastBall(Ball ball)
    {
        if (Balls[0] == ball)
        {
            return true;
        }
        return false;
    }

    bool BallTooFar(Ball ball1, Ball ball2)
    {
        float difference = ball1.distanceTravelled - ball2.distanceTravelled;
        if (difference > ballDistance)
        {
            return true;
        }
        return false;
    }
}
