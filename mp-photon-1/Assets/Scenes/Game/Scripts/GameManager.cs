using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int NumberOfBalls;
    public float SecondsBetweenBallSpawn;
    public GameObject PlayerSpawnerPrefab;
    public GameObject BallSpawnerPrefab;
    public GameObject ScoreUIPrefab;
    private BallSpawner ballSpawner;
    private BallSpawner playerSpawner;
    private CanvasController canvasController;
    private Dictionary<string, int> scores = new Dictionary<string, int>();    
    private int numberOfPlayers;    

    private int ScoreToWin => ballSpawner.BallCount / scores.Count;

    internal void UpdateScore(string playerName)
    {
        scores[playerName] += 1;
        canvasController.UpdateScore(playerName, scores[playerName]);
        if (scores[playerName] == ScoreToWin)
        {
            canvasController.DisplayWinMessage(playerName);
        }
    }

    private void Awake()
    {
        var transform = this.transform;
        ballSpawner = Instantiate(BallSpawnerPrefab).GetComponent<BallSpawner>();
        ballSpawner.name = "Ball Daddy";
        ballSpawner.transform.parent = transform;
        ballSpawner.BallCount = NumberOfBalls;
        ballSpawner.SecondsBetweenSpawn = SecondsBetweenBallSpawn;
        ballSpawner.Initialize();

        playerSpawner = Instantiate(PlayerSpawnerPrefab).GetComponent<BallSpawner>();
        playerSpawner.name = "Player Daddy";
        playerSpawner.transform.parent = transform;
        playerSpawner.SecondsBetweenSpawn = 0;

        canvasController = Instantiate(ScoreUIPrefab).GetComponent<CanvasController>();
        canvasController.transform.parent = transform;
        canvasController.name = "HUD";
    }

    private void InitializeScores()
    {
        foreach (var player in playerSpawner.Balls)
        {
            scores.Add(player.name, 0);
        }
    }

    private void Start()
    {
        StartCoroutine(PressAnyKeyToStart());
    }

    private IEnumerator PressAnyKeyToStart()
    {
        canvasController.PressAnyKeyToStart();
        while (!Input.anyKey)
        {
            yield return null;
        }

        canvasController.Clear();
        yield return new WaitForSeconds(0.5f);

        ////var input = CanvasController.HowManyPlayers();
        ////input.Select();
        ////while (!Input.GetKeyDown(KeyCode.Return) || input.text.Length == 0)
        ////{
        ////    yield return null;
        ////}
        ////numberOfPlayers = int.Parse(input.text);
        numberOfPlayers = 1;
        playerSpawner.BallCount = numberOfPlayers;
        playerSpawner.Initialize();
        InitializeScores();
        canvasController.Clear();

        Run();
    }

    private void Run()
    {
        canvasController.InitializeScores(scores.Keys);
        playerSpawner.Go();
        ballSpawner.Go();
    }
}