using System;
using System.Collections;
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
    private string displayWinMessage = null;

    internal void DisplayWinMessage(string playerName)
    {
        displayWinMessage = playerName + " WINS, MOTHERFUCKER!";
    }

    internal void AwaitingOthers()
    {
        midScreenText.fontSize = 50;
        midScreenText.text = "Awaiting other players...";
    }

    internal void PressAnyKeyToStart()
    {
        midScreenText.fontSize = 50;
        midScreenText.text = "Press Any Key To Start...";
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

    private void Update()
    {
        if (displayWinMessage != null)
            midScreenText.text = displayWinMessage;
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
}