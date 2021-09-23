using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallProjectile : MonoBehaviour
{
    BallCollection ballCollection;
    Ball thisBall;
    public float speed = 8;
    Vector3 originPosition;
    float despawnDistance = 10;
    private void Start()
    {
        originPosition = transform.position;
        ballCollection = FindObjectOfType<BallCollection>();
        thisBall = GetComponent<Ball>();



        // test
        this.gameObject.GetComponentInChildren<Renderer>().material.color = new Color(100, 255, 255);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        if (Vector3.Distance(originPosition, transform.position) >= despawnDistance)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.root.TryGetComponent<Ball>(out Ball otherBall);
        if (otherBall != null)
        {
            ballCollection.AddBallAtPosition(thisBall, otherBall, collision.contacts[0].point);
        }
    }
}
