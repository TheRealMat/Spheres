using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallCollection : MonoBehaviour
{
    public float ballSpeed = 0;
    public float ballDistance = 0;
    LinkedList<Ball> Balls = new LinkedList<Ball>();
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public GameObject ballPrefab;
    void CreateBall()
    {
        GameObject prefab = Instantiate(ballPrefab);
        Ball ball = prefab.GetComponent<Ball>();
        ball.distanceTravelled = Balls.Count * ballDistance;
        AddBall(ball);
    }

    private void Start()
    {
        CreateBall();
        CreateBall();
        CreateBall();
        CreateBall();
        CreateBall();
        CreateBall();
    }

    void Update()
    {
        foreach (Ball ball in Balls)
        {
            MoveBall(ball);
        }
    }

    void MoveBall(Ball ball)
    {
        // ball is the last ball and so should move
        if (IsLastBall(ball))
        {
            GoForward(ball);
        }
        // if we are too far away from the ball behind us we should move back
        else if (BallTooFar(ball, FindBackBall(ball)))
        {
            GoBackward(ball);
        }
        // we are in line, so position should be set accordingly
        else
        {
            SetPosition(ball, ball.distanceTravelled = FindBackBall(ball).distanceTravelled + ballDistance);
        }

    }

    void AddBall(Ball ball)
    {
        Balls.AddFirst(ball);
    }

    public void AddBallAtPosition(Ball newBall, Ball hitBall, Vector3 collisionPoint)
    {
        // point on path closest to impact
        float pathPosition = pathCreator.path.GetClosestDistanceAlongPath(collisionPoint);


        float frontPosDistance = Mathf.Abs(hitBall.distanceTravelled + ballDistance - pathPosition);
        float backPosDistance = Mathf.Abs(hitBall.distanceTravelled - ballDistance - pathPosition);
        LinkedListNode<Ball> hitBallNode = Balls.Find(hitBall);
        if (frontPosDistance <= backPosDistance)
        {
            // go in front of hit ball

            newBall.distanceTravelled = hitBall.distanceTravelled + ballDistance;
            Balls.AddAfter(hitBallNode, newBall);
        }
        else
        {
            // take position of current ball (push other balls up)

            newBall.distanceTravelled = hitBall.distanceTravelled - ballDistance;

            Balls.AddBefore(hitBallNode, newBall);
        }

        // love magic numbers
        newBall.gameObject.GetComponentInChildren<Collider>().gameObject.layer = 6;
        newBall.enabled = true;
        Destroy(newBall.gameObject.GetComponent<BallProjectile>());
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
    void SetPosition(Ball ball, float distanceTravelled)
    {
        ball.transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
        ball.transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
    }

    Ball FindBackBall(Ball ball)
    {
        LinkedListNode<Ball> ballNode = Balls.Find(ball);
        if (ballNode.Previous != null)
        {
            return ballNode.Previous.Value;
        }
        return null;
    }
    Ball FindForwardBall(Ball ball)
    {
        LinkedListNode<Ball> ballNode = Balls.Find(ball);
        if (ballNode.Previous != null)
        {
            return ballNode.Next.Value;
        }
        return null;
    }
    bool IsLastBall(Ball ball)
    {
        return Balls.First.Value == ball;
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
