using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject BallSpawnerPrefab;
    public GameObject PlayerSpawnerPrefab;
    public GameObject ScoreUIPrefab;

    BallSpawner BallSpawner;
    BallSpawner PlayerSpawner;
    CanvasController CanvasController;
    Dictionary<string, int> Scores = new Dictionary<string, int>();

    public int NumberOfBalls;
    public float SecondsBetweenBallSpawn;
    public int NumberOfPlayers;

    void Awake()
    {
        var transform = this.transform;
        BallSpawner = Instantiate(BallSpawnerPrefab).GetComponent<BallSpawner>();
        BallSpawner.name = "Ball Daddy";
        BallSpawner.transform.parent = transform;
        BallSpawner.BallCount = NumberOfBalls;
        BallSpawner.SecondsBetweenSpawn = SecondsBetweenBallSpawn;
        BallSpawner.Initialize();

        PlayerSpawner = Instantiate(PlayerSpawnerPrefab).GetComponent<BallSpawner>();
        PlayerSpawner.name = "Player Daddy";
        PlayerSpawner.transform.parent = transform;
        PlayerSpawner.BallCount = NumberOfPlayers;
        PlayerSpawner.SecondsBetweenSpawn = 0;
        PlayerSpawner.Initialize();

        CanvasController = Instantiate(ScoreUIPrefab).GetComponent<CanvasController>();
        CanvasController.transform.parent = transform;
        CanvasController.name = "HUD";

        InitializeScores();
    }
    void InitializeScores()
    {
        foreach (var player in PlayerSpawner.balls)
        {
            Scores.Add(player.name, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PressAnyKeyToStart());        
    }
    int ScoreToWin => BallSpawner.BallCount / Scores.Count;
    public void UpdateScore(string playerName)
    {
        Scores[playerName] += 1;
        CanvasController.UpdateScore(playerName, Scores[playerName]);
        if(Scores[playerName] == ScoreToWin)
        {
            CanvasController.DisplayWinMessage(playerName);
        }
    }

    IEnumerator PressAnyKeyToStart()
    {
        CanvasController.PressAnyKeyToStart();
        while (!Input.anyKey)
        {
            yield return null;
        }
        CanvasController.Clear();

        var input = CanvasController.HowManyPlayers();
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        Debug.Log(input.text);
        CanvasController.Clear();

        Run();
    }

    void Run()
    {
        CanvasController.InitializeScores(Scores.Keys);
        PlayerSpawner.Go();
        BallSpawner.Go();
    }
}