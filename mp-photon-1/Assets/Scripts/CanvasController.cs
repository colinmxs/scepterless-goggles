using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    Text myText;
    int score = 0;

    void Start()
    {
        gameObject.AddComponent<Canvas>();
        var canvas = gameObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        gameObject.AddComponent<CanvasScaler>();
        gameObject.AddComponent<GraphicRaycaster>();

        // Text
        var myScore = new GameObject();
        myScore.transform.parent = gameObject.transform;
        myScore.name = "Score";

        myText = myScore.AddComponent<Text>();
        myText.font = Font.CreateDynamicFontFromOSFont("Arial", 100);
        myText.text = "Score: " + score;
        myText.fontSize = 100;

        // Text position
        var rectTransform = myText.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, 0);
        rectTransform.sizeDelta = new Vector2(450, 200);
    }    
    public void IncrementScore()
    {
        score += 1;
        myText.text = "Score: " + score;
    }
    public void DisplayWinMessage()
    {
        myText.text = "You WIN MOTHERFUCKER";
    }
}