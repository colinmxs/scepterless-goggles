using UnityEngine;

public class CharacterSelectable : Selectable
{
    private Canvas indicatorCanvas;
    
    void Awake()
    {
        indicatorCanvas = GetComponentInChildren<Canvas>();
        indicatorCanvas.enabled = false;
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