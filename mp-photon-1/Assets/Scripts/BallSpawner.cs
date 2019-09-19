﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Launcher))]
public class BallSpawner : MonoBehaviour
{
    public int BallCount;
    public int SecondsBetweenSpawn;
    public GameObject BallPrefab;

    private GameObject[] balls;
    private Launcher launcher;
    private int BallsSpawned = 0;

    void Awake()
    {
        launcher = GetComponent<Launcher>();
        balls = new GameObject[BallCount];
        for (int i = 0; i < BallCount; i++)
        {
            var ball = Instantiate(BallPrefab, transform);
            ball.SetActive(false);
            balls[i] = ball;
        }

    }

    public void Go()
    {
        StartCoroutine(SpawnBalls());
    }

    IEnumerator SpawnBalls()
    {
        while (BallCount > BallsSpawned)
        {
            var ball = balls[BallsSpawned];
            ball.SetActive(true);
            ball.transform.position = transform.position;
            launcher.LaunchBall(ball);
            BallsSpawned = BallsSpawned + 1;
            yield return new WaitForSeconds(SecondsBetweenSpawn);
        }
    }
}