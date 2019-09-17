using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnerController : MonoBehaviour
{
    public int BallCount;
    public int SecondsBetweenSpawn;
    public GameObject BallPrefab;

    private GameObject[] balls;
    private int BallsSpawned = 0;
    private System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        balls = new GameObject[BallCount];
        for (int i = 0; i < BallCount; i++)
        {
            var ball = Instantiate(BallPrefab);
            ball.SetActive(false);
            balls[i] = ball;
        }

        StartCoroutine(SpawnBalls());
    }
    private void LaunchBall(GameObject ball)
    {
        var ballRb = ball.GetComponent<Rigidbody2D>();
        var x = random.Next(-100, 100);
        var y = random.Next(-100, 100);
        var targetVelocity = new Vector2(x, y);
        var moveVector = Vector3.zero;
        ballRb.velocity = Vector3.SmoothDamp(ballRb.velocity, targetVelocity, ref moveVector, 0.05f);
    }
    IEnumerator SpawnBalls()
    {
        while (BallCount > BallsSpawned)
        {
            var ball = balls[BallsSpawned];
            ball.SetActive(true);
            ball.transform.position = transform.position;
            LaunchBall(ball);
            BallsSpawned = BallsSpawned + 1;
            yield return new WaitForSeconds(SecondsBetweenSpawn);
        }
    }
}
