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
        if (!selected)
        {
            base.OnSelect();
            indicatorCanvas.ToggleSelectionMarker();
        }        
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
        indicatorCanvas.ToggleSelectionMarker();
    }
}