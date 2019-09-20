using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    Canvas Canvas;
    Font font;

    void Awake()
    {
        // Canvas
        var myGO = new GameObject();
        myGO.name = "My Score";
        myGO.AddComponent<Canvas>();
        myGO.transform.SetParent(this.transform);

        Canvas = myGO.GetComponent<Canvas>();
        Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        myGO.AddComponent<CanvasScaler>();
        myGO.AddComponent<GraphicRaycaster>();

        font = Font.CreateDynamicFontFromOSFont("Arial", 100);        
    }

    internal void Initialize(IEnumerable<string> playerNames)
    {
        foreach (var playerName in playerNames)
        {
            AddPlayerToScoreBoard(playerName);
        }
    }

    Dictionary<string, Text> ScoreBoardEntries = new Dictionary<string, Text>();
    private void AddPlayerToScoreBoard(string name)
    {
        var ScoreBoard = new GameObject();
        ScoreBoard.transform.parent = Canvas.transform;
        ScoreBoard.name = "Scoreboard";

        var scoreText = ScoreBoard.AddComponent<Text>();
        scoreText.font = this.font;
        scoreText.text = name + "'s Score: 0";
        scoreText.fontSize = 20;

        // Text position
        var rectTransform = scoreText.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(400, 26);
        rectTransform.localPosition = new Vector3(200, (-26) * (ScoreBoardEntries.Count + 1), 0);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        var scoreBoardEntry = scoreText;
        ScoreBoardEntries.Add(name, scoreBoardEntry);
    }

    public void UpdateScore(string playerName, int score)
    {
        var scoreBoardEntry = ScoreBoardEntries[playerName];
        scoreBoardEntry.text = playerName + "'s Score: " + score;
    }

    public void DisplayWinMessage(string playerName)
    {
        var scoreBoardEntry = ScoreBoardEntries[playerName];
        scoreBoardEntry.text = "You WIN MOTHERFUCKER";
        throw null;
    }
}