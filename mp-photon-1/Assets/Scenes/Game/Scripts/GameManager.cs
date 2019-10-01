using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int NumberOfBalls;
    public float SecondsBetweenBallSpawn;
    public GameObject PlayerSpawnerPrefab;
    public GameObject BallSpawnerPrefab;
    public GameObject ScoreUIPrefab;
    public GameObject OpponentPrefab;
    private RealtimeInGamePlayer LocalPlayer;
    private BallSpawner ballSpawner;
    private BallSpawner playerSpawner;
    private CanvasController canvasController;
    private Dictionary<string, int> scores = new Dictionary<string, int>();
    private List<(string, int)> scoresToProcess;
    private int numberOfPlayers = 1;
    private readonly List<OpponentController> OpponentBalls = new List<OpponentController>();

    private int ScoreToWin => ballSpawner.BallCount / scores.Count;

    internal void UpdateScore(string playerName)
    {
        if(LocalPlayer.player.NickName == playerName)
        {
            LocalPlayer.client.SendUpdateScore(playerName);
        }

        scores[playerName] += 1;
        StartCoroutine(canvasController.UpdateScore(playerName, scores[playerName]));
        if (scores[playerName] == ScoreToWin)
        {
            canvasController.DisplayWinMessage(playerName);
        }
    }

    private void Awake()
    {
        var transform = this.transform;
        ballSpawner = Instantiate(BallSpawnerPrefab).GetComponent<BallSpawner>();
        ballSpawner.name = "Ball Spawner";
        ballSpawner.transform.parent = transform;
        ballSpawner.BallCount = NumberOfBalls;
        ballSpawner.SecondsBetweenSpawn = SecondsBetweenBallSpawn;
        ballSpawner.Initialize();

        playerSpawner = Instantiate(PlayerSpawnerPrefab).GetComponent<BallSpawner>();
        playerSpawner.name = "Player Spawner";
        playerSpawner.transform.parent = transform;
        playerSpawner.SecondsBetweenSpawn = 0;

        canvasController = Instantiate(ScoreUIPrefab).GetComponent<CanvasController>();
        canvasController.transform.parent = transform;
        canvasController.name = "HUD";

        LocalPlayer = FindObjectOfType<RealtimeInGamePlayer>();
    }

    private void InitializeScores()
    {
        foreach (var player in playerSpawner.Balls)
        {
            scores.Add(player.name, 0);
        }
        foreach (var player in OpponentBalls)
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

        canvasController.AwaitingOthers();       

        playerSpawner.BallCount = 1;
        playerSpawner.Initialize();

        if(LocalPlayer != null)
        {
            var opponentParent = new GameObject("Opponents");
            opponentParent.transform.SetParent(this.transform);
            numberOfPlayers = LocalPlayer.client.CurrentRoom.PlayerCount;
            foreach (var player in LocalPlayer.client.CurrentRoom.Players.Values)
            {
                if(player.NickName != LocalPlayer.player.NickName)
                {
                    var opponent = Instantiate(OpponentPrefab).GetComponent<OpponentController>();
                    opponent.transform.SetParent(opponentParent.transform);
                    opponent.player = (RealtimePlayer)player;
                    opponent.name = player.NickName;
                    OpponentBalls.Add(opponent);
                }                
            }            
        }
        
        canvasController.Clear();

        Run();
    }

    private void Run()
    {        
        playerSpawner.Go();
        var player = playerSpawner.Balls.First();
        player.name = LocalPlayer.player.NickName;
        var playerController = player.GetComponent<PlayerController>();
        playerController.RealtimePlayer = LocalPlayer;
        LocalPlayer.client.OnScore += UpdateScore;
        InitializeScores();
        canvasController.InitializeScores(scores.Keys);        
        ballSpawner.Go();
    }
}