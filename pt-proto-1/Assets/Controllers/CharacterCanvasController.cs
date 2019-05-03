using UnityEngine;
using UnityEngine.UI;

public class CharacterCanvasController : MonoBehaviour
{
    public Image selectionMarker;

    void Start()
    {
        selectionMarker.enabled = false;
    }

    public void ToggleSelectionMarker()
    {
        selectionMarker.enabled = selectionMarker.enabled ? false : true;
    }
}
