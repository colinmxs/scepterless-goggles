using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BallSpawner BallSpawner;
    public CanvasController CanvasController;

    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
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
