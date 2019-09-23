using System;
using System.Collections;
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
        myGO.name = "Canvas";
        myGO.AddComponent<Canvas>();
        myGO.transform.SetParent(this.transform);

        Canvas = myGO.GetComponent<Canvas>();
        Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        myGO.AddComponent<CanvasScaler>();
        myGO.AddComponent<GraphicRaycaster>();

        font = Font.CreateDynamicFontFromOSFont("Arial", 100);

        var midScreenTextParent = new GameObject();
        midScreenTextParent.transform.parent = Canvas.transform;
        midScreenTextParent.name = "Mid Screen Text";

        MidScreenText = midScreenTextParent.AddComponent<Text>();
        MidScreenText.font = this.font;
        var rectTransform = MidScreenText.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(450, 300);
        rectTransform.localPosition = Vector3.zero;
    }

    internal void InitializeScores(IEnumerable<string> playerNames)
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
    }

    Text MidScreenText;
    internal void PressAnyKeyToStart()
    {       
        MidScreenText.fontSize = 50;
        MidScreenText.text = "Press Any Key To Start...";        
    }

    internal InputField HowManyPlayers()
    {
        MidScreenText.fontSize = 32;
        MidScreenText.text = "How many players?";

        var inputParent = new GameObject();
        inputParent.name = "Input Text";
        inputParent.transform.parent = Canvas.transform;
        var inputText = inputParent.AddComponent<Text>();
        inputText.font = this.font;
        inputText.fontSize = 33;
        inputText.text = "";
        var rectTransform = inputText.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(450, 300);
        rectTransform.localPosition = new Vector3(0, -38, 0);
        var inputFieldParent = new GameObject();
        inputFieldParent.name = "Input Field";
        inputFieldParent.transform.parent = Canvas.transform;
        var inputField = inputFieldParent.AddComponent<InputField>();
        inputField.textComponent = inputText;
        var caret = new GameObject();
        //caret.AddComponent<>
        return inputField;
    }    

    internal void Clear()
    {
        var childs = GetComponentsInChildren<Text>();
        foreach (var text in childs)
        {
            text.text = "";
        }
    }
}