using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerSpawnerPrefab;
    public GameObject ScoreUIPrefab;
    public GameObject OpponentPrefab;
    public GameObject TargetPrefab;
    private RealtimeInGamePlayer LocalPlayer;
    private BallSpawner playerSpawner;
    private CanvasController canvasController;
    private TargetController target;
    private bool shutdown = false;
    private int numberOfPlayers = 1;

    private void Awake()
    {
        var transform = this.transform;
        playerSpawner = Instantiate(PlayerSpawnerPrefab).GetComponent<BallSpawner>();
        playerSpawner.name = "Player Spawner";
        playerSpawner.transform.parent = transform;
        playerSpawner.SecondsBetweenSpawn = 0;       

        canvasController = Instantiate(ScoreUIPrefab).GetComponent<CanvasController>();
        canvasController.transform.parent = transform;
        canvasController.name = "HUD";

        target = Instantiate(TargetPrefab).GetComponent<TargetController>();
        target.transform.parent = transform;

        LocalPlayer = FindObjectOfType<RealtimeInGamePlayer>();
    }

    private void Start()
    {
        StartCoroutine(PressAnyKeyToStart());
    }

    private void Update()
    {
        if(shutdown)
            if(target.isActiveAndEnabled)
                target.gameObject.SetActive(false);
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

        if (LocalPlayer != null)
        {
            var opponentParent = new GameObject("Opponents");
            opponentParent.transform.SetParent(this.transform);
            numberOfPlayers = LocalPlayer.client.CurrentRoom.PlayerCount;
            foreach (var player in LocalPlayer.client.CurrentRoom.Players.Values)
            {
                if (player.NickName != LocalPlayer.player.NickName)
                {
                    var opponent = Instantiate(OpponentPrefab).GetComponent<OpponentController>();
                    opponent.transform.SetParent(opponentParent.transform);
                    opponent.player = (RealtimePlayer)player;
                    opponent.name = player.NickName;
                    canvasController.AddHoverCanvas(opponent.name, opponent.transform);
                }
            }
        }

        canvasController.Clear();

        Run();
    }

    private IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(2.0f);
        LocalPlayer.client.ToggleReady();
        SceneManager.LoadScene("Connect");
    }


    private void Run()
    {
        playerSpawner.Go();
        var player = playerSpawner.Balls.First();
        if (LocalPlayer != null)
        {
            player.name = LocalPlayer.player.NickName;
            LocalPlayer.client.OnClaimWin += EndGame;            
        }
        var playerController = player.GetComponent<PlayerController>();
        playerController.RealtimePlayer = LocalPlayer;
        target.TargetNailed.AddListener(() => { EndGame(player.name); });
    }

    private void EndGame(string winner)
    {
        if (!shutdown)
        {
            canvasController.DisplayWinMessage(winner);
            if (LocalPlayer != null)
            {
                if (LocalPlayer.player.NickName == winner)
                {
                    LocalPlayer.client.SendClaimWin();
                }
            }
            shutdown = true;
            StartCoroutine(BackToMenu());
        }        
    }
}