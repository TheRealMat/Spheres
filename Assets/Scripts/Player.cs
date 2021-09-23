using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject ballPrefab;
    private void Update()
    {
        Look();
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    void Shoot()
    {
        GameObject ball = Instantiate(ballPrefab, this.transform.position, this.transform.rotation);

        ball.AddComponent<BallProjectile>();
        ball.GetComponent<Ball>().enabled = false;
    }
    void Look()
    {
        Plane plane = new Plane(Vector3.up, 0);

        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            transform.LookAt(ray.GetPoint(distance));
        }
    }
}
