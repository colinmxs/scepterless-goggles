using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Selectable : MonoBehaviour
{
    SelectionManager manager;
    protected bool selected;

    void Start()
    {
        manager = SelectionManager.instance;
    }

    void OnMouseDown()
    {
        OnSelect();
        manager.OnSelect(this);
    }

    protected virtual void OnSelect()
    {
        selected = true;
    }
    public virtual void OnDeselect()
    {
        selected = false;
    }
}
