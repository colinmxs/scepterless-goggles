using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Launcher))]
public class BallSpawner : MonoBehaviour
{
    public GameObject BallPrefab;
    public GameObject[] balls { get; private set; }

    internal int BallCount;
    internal float SecondsBetweenSpawn;

    private Launcher launcher;
    private int BallsSpawned = 0;
    bool initialized = false;

    void Awake()
    {
        launcher = GetComponent<Launcher>();        
    }

    public void Initialize()
    {
        balls = new GameObject[BallCount];
        for (int i = 0; i < BallCount; i++)
        {
            var ball = Instantiate(BallPrefab, transform);
            ball.name = "Ball" + i;
            ball.SetActive(false);
            balls[i] = ball;
        }
        initialized = true;
    }

    void Start()
    {

    }
    public void Go()
    {
        if (!initialized) Initialize();
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
