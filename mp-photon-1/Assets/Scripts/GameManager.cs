using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject BallSpawnerPrefab;
    public GameObject ScoreUIPrefab;
    public GameObject PlayerPrefab;

    BallSpawner BallSpawner;
    CanvasController CanvasController;
    Dictionary<string, int> Scores = new Dictionary<string, int>();

    void Awake()
    {
        var transform = this.transform;
        BallSpawner = Instantiate(BallSpawnerPrefab).GetComponent<BallSpawner>();
        BallSpawner.name = "Ball Daddy";
        BallSpawner.transform.parent = transform;

        //make this scale with multiple players
        var player1 = Instantiate(PlayerPrefab);
        player1.transform.parent = transform;
        player1.name = "Colin";
        Scores[player1.name] = 0;

        //make this scale with multiple players
        var player2 = Instantiate(PlayerPrefab);
        player2.transform.parent = transform;
        player2.transform.position = new Vector3(-8, 0, 0);
        player2.name = "Donkey";
        Scores[player2.name] = 0;

        CanvasController = Instantiate(ScoreUIPrefab).GetComponent<CanvasController>();
        CanvasController.transform.parent = transform;
        CanvasController.name = "Score Board";
        CanvasController.Initialize(Scores.Keys);
    }

    // Start is called before the first frame update
    void Start()
    {
        //BallSpawner.Initialize(Players);
        BallSpawner.Go();        
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
}
