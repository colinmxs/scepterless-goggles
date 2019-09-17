using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    GameObject myScore;

    void Start()
    {
        gameObject.AddComponent<Canvas>();
        var canvas = gameObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        gameObject.AddComponent<CanvasScaler>();
        gameObject.AddComponent<GraphicRaycaster>();

        // Text
        myScore = new GameObject();
        myScore.transform.parent = gameObject.transform;
        myScore.name = "My Score";

        var text = myScore.AddComponent<Text>();
        text.font = Font.CreateDynamicFontFromOSFont("Arial", 100);
        text.text = "wobble";
        text.fontSize = 100;

        // Text position
        var rectTransform = text.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, 0);
        rectTransform.sizeDelta = new Vector2(400, 200);
    }
}
