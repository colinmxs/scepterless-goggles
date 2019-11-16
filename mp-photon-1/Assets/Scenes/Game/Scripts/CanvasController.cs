using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public GameObject MidScreenTextPrefab;
    private Canvas midScreenTextCanvas;
    private GameObject hoverCanvasParent;
    private Text midScreenText;
    private Font font;
    private string displayWinMessage = null;

    internal void AddHoverCanvas(string text, Transform hoverTransform)
    {
        var hoverGameObject = new GameObject();
        hoverGameObject.name = "Hover " + text;
        hoverGameObject.transform.SetParent(this.hoverCanvasParent.transform);
        var hoverCanvas = hoverGameObject.AddComponent<Canvas>();
        //hoverCanvas.renderMode = RenderMode.WorldSpace;
        var textObject = new GameObject();
        var textComponent = textObject.AddComponent<Text>();
        textObject.transform.SetParent(hoverGameObject.transform);
        textObject.name = "Text";
        textComponent.text = text;
        textComponent.font = this.font;
        textComponent.alignment = TextAnchor.UpperCenter;
        var hoverFollow = hoverGameObject.AddComponent<HoverFollow>();        
        hoverFollow.target = hoverTransform.transform;
    }

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
        midScreenText.text = "First player to reach the orange square wins! Press Any Key To Start...";
    }

    internal void Clear()
    {
        var childs = midScreenTextCanvas.gameObject.GetComponentsInChildren<Text>();
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
        var myGO = new GameObject();
        myGO.name = "Canvas";
        myGO.AddComponent<Canvas>();
        myGO.transform.SetParent(this.transform);

        midScreenTextCanvas = myGO.GetComponent<Canvas>();
        midScreenTextCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        myGO.AddComponent<CanvasScaler>();
        myGO.AddComponent<GraphicRaycaster>();

        font = Font.CreateDynamicFontFromOSFont("Arial", 100);

        var midScreenTextParent = Instantiate(MidScreenTextPrefab, midScreenTextCanvas.transform);
        midScreenTextParent.name = "Mid Screen Text";
        midScreenText = midScreenTextParent.GetComponent<Text>();

        this.hoverCanvasParent = new GameObject();
        hoverCanvasParent.transform.SetParent(this.transform);
        hoverCanvasParent.name = "Hover Canvas";
        var canvasComponent = hoverCanvasParent.AddComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
    }
}