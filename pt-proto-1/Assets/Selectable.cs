using UnityEngine;

public class Selectable : MonoBehaviour
{
    [HideInInspector]
    public bool selected;

    public virtual void OnSelect()
    {
        Debug.Log("Selectable::OnSelect::");
        selected = true;
    }
    public virtual void OnDeselect()
    {
        selected = false;
    }
}
