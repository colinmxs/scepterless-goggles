using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    Text text;
    int score = 0;

    void Start()
    {
        GameObject myGO;
        GameObject myText;
        Canvas myCanvas;
        Text text;
        RectTransform rectTransform;

        // Canvas
        myGO = new GameObject();
        myGO.name = "TestCanvas";
        myGO.AddComponent<Canvas>();

        myCanvas = myGO.GetComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        myGO.AddComponent<CanvasScaler>();
        myGO.AddComponent<GraphicRaycaster>();

        // Text
        myText = new GameObject();
        myText.transform.parent = myGO.transform;
        myText.name = "wibble";

        text = myText.AddComponent<Text>();
        text.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        text.text = "wobble";
        text.fontSize = 100;

        // Text position
        //rectTransform = text.GetComponent<RectTransform>();
        //rectTransform.localPosition = new Vector3(0, 0, 0);
        //rectTransform.sizeDelta = new Vector2(1000, 200);
    }

    //void Start()
    //{
    //    GameObject scoreUI;
    //    GameObject myText;
    //    Canvas scoreCanvas;        
    //    RectTransform rectTransform;

    //    // Canvas
    //    scoreUI = new GameObject
    //    {
    //        name = "Score Canvas"
    //    };

    //    scoreCanvas = scoreUI.AddComponent<Canvas>();
    //    scoreCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    //    scoreUI.AddComponent<CanvasScaler>();
    //    scoreUI.AddComponent<GraphicRaycaster>();

    //    // Text
    //    myText = new GameObject();
    //    myText.transform.parent = scoreUI.transform;
    //    myText.name = "Player Score";

    //    text = myText.AddComponent<Text>();
    //    text.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
    //    text.text = "Score: " + score;
    //    text.fontSize = 100;

    //    // Text position
    //    rectTransform = text.GetComponent<RectTransform>();
    //    //rectTransform.anchorMin = new Vector2(0, 1);
    //    //rectTransform.anchorMax = new Vector2(0, 1);
    //    //rectTransform.pivot = new Vector2(0, 1);
    //    rectTransform.localPosition = new Vector3(0, 0, 0);
    //    rectTransform.sizeDelta = new Vector2(1000, 200);
    //}    
    public void IncrementScore()
    {
        //score += 1;
        //text.text = "Score: " + score;
    }
    public void DisplayWinMessage()
    {
        //text.text = "You WIN MOTHERFUCKER";
    }
}