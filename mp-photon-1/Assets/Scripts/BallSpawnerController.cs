using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnerController : MonoBehaviour
{
    public int BallCount;
    public GameObject BallPrefab;

    private GameObject[] balls;
    private int BallsSpawned = 0;

    // Start is called before the first frame update
    void Start()
    {
        balls = new GameObject[BallCount];
        for (int i = 0; i < BallCount; i++)
        {
            var ball = Instantiate(BallPrefab, this.transform);
            ball.SetActive(false);
            balls[i] = ball;
        }

        StartCoroutine(SpawnBallEverySecond());
    }

    IEnumerator SpawnBallEverySecond()
    {
        while (BallCount > BallsSpawned)
        {
            var ball = balls[BallsSpawned];
            ball.SetActive(true);
            BallsSpawned = BallsSpawned + 1;
            yield return new WaitForSeconds(1f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
