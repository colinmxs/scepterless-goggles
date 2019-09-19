using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject BallSpawnerPrefab;
    public GameObject ScoreUIPrefab;
    public GameObject PlayerPrefab;

    BallSpawner BallSpawner;
    CanvasController CanvasController;
    PlayerController[] Players = new PlayerController[1];

    int score = 0;

    void Awake()
    {
        BallSpawner = Instantiate(BallSpawnerPrefab).GetComponent<BallSpawner>();
        this.BallSpawner.name = "Ball Daddy";
        this.BallSpawner.transform.parent = this.transform;
        CanvasController = Instantiate(ScoreUIPrefab).GetComponent<CanvasController>();
        this.CanvasController.transform.parent = this.transform;
        this.CanvasController.name = "Score Board";
        var player1 = Instantiate(PlayerPrefab);
        player1.name = "Colin";
        Players[0] = player1.GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        BallSpawner.Initialize(Players);
        BallSpawner.Go();        
    }

    public void UpdateScore()
    {
        score += 1;
        CanvasController.IncrementScore();
        if(score == BallSpawner.BallCount)
        {
            CanvasController.DisplayWinMessage();
        }
    }
}
