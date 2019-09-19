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
        RectTransform rectTransform;

        // Canvas
        myGO = new GameObject();
        myGO.name = "My Score";
        myGO.AddComponent<Canvas>();
        myGO.transform.SetParent(this.transform);

        myCanvas = myGO.GetComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        myGO.AddComponent<CanvasScaler>();
        myGO.AddComponent<GraphicRaycaster>();

        // Text
        myText = new GameObject();
        myText.transform.parent = myGO.transform;
        myText.name = "wibble";

        text = myText.AddComponent<Text>();
        text.font = Font.CreateDynamicFontFromOSFont("Arial", 100);
        text.text = "Score: " + score;
        text.fontSize = 40;

        // Text position
        rectTransform = text.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(1000, 200);
        rectTransform.localPosition = new Vector3(500, -100, 0);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
    }

    public void IncrementScore()
    {
        score += 1;
        text.text = "Score: " + score;
    }
    public void DisplayWinMessage()
    {
        text.text = "You WIN MOTHERFUCKER";
    }
}