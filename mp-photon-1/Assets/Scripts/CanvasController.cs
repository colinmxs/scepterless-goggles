using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public GameObject MidScreenTextPrefab;
    public GameObject InputFieldPrefab;
    private readonly Dictionary<string, Text> scoreBoardEntries = new Dictionary<string, Text>();
    private Canvas canvas;
    private Text midScreenText;
    private Font font;    

    internal void UpdateScore(string playerName, int score)
    {
        var scoreBoardEntry = scoreBoardEntries[playerName];
        scoreBoardEntry.text = playerName + "'s Score: " + score;
    }

    internal void DisplayWinMessage(string playerName)
    {
        var scoreBoardEntry = scoreBoardEntries[playerName];
        scoreBoardEntry.text = "You WIN MOTHERFUCKER";
    }

    internal void InitializeScores(IEnumerable<string> playerNames)
    {
        foreach (var playerName in playerNames)
        {
            AddPlayerToScoreBoard(playerName);
        }
    }

    internal void PressAnyKeyToStart()
    {
        midScreenText.fontSize = 50;
        midScreenText.text = "Press Any Key To Start...";
    }

    internal InputField HowManyPlayers()
    {
        midScreenText.fontSize = 32;
        midScreenText.text = "How many players?";

        var inputField = Instantiate(InputFieldPrefab, canvas.transform).GetComponent<InputField>();
        return inputField;
    }

    internal void Clear()
    {
        var childs = GetComponentsInChildren<Text>();
        foreach (var text in childs)
        {
            text.text = string.Empty;
        }

        var inputs = GetComponentsInChildren<InputField>();
        foreach (var input in inputs)
        {
            Destroy(input.gameObject);
        }
    }

    private void Awake()
    {
        // Canvas
        var myGO = new GameObject();
        myGO.name = "Canvas";
        myGO.AddComponent<Canvas>();
        myGO.transform.SetParent(this.transform);

        canvas = myGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        myGO.AddComponent<CanvasScaler>();
        myGO.AddComponent<GraphicRaycaster>();

        font = Font.CreateDynamicFontFromOSFont("Arial", 100);

        var midScreenTextParent = Instantiate(MidScreenTextPrefab, canvas.transform);
        midScreenTextParent.name = "Mid Screen Text";
        midScreenText = midScreenTextParent.GetComponent<Text>();
    }

    private void AddPlayerToScoreBoard(string name)
    {
        var scoreBoard = new GameObject();
        scoreBoard.transform.parent = canvas.transform;
        scoreBoard.name = name + "'s Score";

        var scoreText = scoreBoard.AddComponent<Text>();
        scoreText.font = this.font;
        scoreText.text = name + "'s Score: 0";
        scoreText.fontSize = 20;

        // Text position
        var rectTransform = scoreText.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(400, 26);
        rectTransform.localPosition = new Vector3(200, (-26) * (scoreBoardEntries.Count + 1), 0);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        var scoreBoardEntry = scoreText;
        scoreBoardEntries.Add(name, scoreBoardEntry);
    }    
}