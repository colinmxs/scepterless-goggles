using UnityEngine;

[RequireComponent(typeof(CharacterCanvasController))]
public class CharacterSelectable : Selectable
{
    private CharacterCanvasController indicatorCanvas;
    
    void Awake()
    {
        indicatorCanvas = GetComponentInChildren<CharacterCanvasController>();
    }
    
    public override void OnSelect()
    {
        Debug.Log("CharacterSelectable::OnSelect::");
        base.OnSelect();
        indicatorCanvas.enabled = true;
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
        indicatorCanvas.enabled = false;
    }
}