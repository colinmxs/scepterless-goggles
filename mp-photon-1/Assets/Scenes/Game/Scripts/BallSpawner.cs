using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Launcher))]
public class BallSpawner : MonoBehaviour
{
    public GameObject BallPrefab;
    private Launcher launcher;
    private int ballsSpawned = 0;
    private bool initialized = false;

    public GameObject[] Balls { get; private set; }

    internal int BallCount { get; set; }

    internal float SecondsBetweenSpawn { get; set; }

    internal void Initialize()
    {
        Balls = new GameObject[BallCount];
        for (int i = 0; i < BallCount; i++)
        {
            GameObject ball = Instantiate(BallPrefab, transform);
            ball.name = "Ball" + i;
            ball.SetActive(false);
            Balls[i] = ball;
        }

        initialized = true;
    }

    internal void Go()
    {
        if (!initialized)
        {
            Initialize();
        }

        StartCoroutine(SpawnBalls());
    }

    private void Awake()
    {
        launcher = GetComponent<Launcher>();
    }

    private IEnumerator SpawnBalls()
    {
        while (BallCount > ballsSpawned)
        {
            GameObject ball = Balls[ballsSpawned];
            ball.SetActive(true);
            ball.transform.position = transform.position;
            launcher.LaunchBall(ball);
            ballsSpawned = ballsSpawned + 1;
            yield return new WaitForSeconds(SecondsBetweenSpawn);
        }

        Destroy(this);
    }
}
