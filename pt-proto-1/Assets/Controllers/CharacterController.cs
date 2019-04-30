using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CharacterController : MonoBehaviour
{
    public float speed;
    Selectable selectable;
    Moveable moveable;
    SelectionManager manager;

    void Start()
    {        
        selectable = GetComponent<Selectable>();
        moveable = GetComponent<Moveable>();
        manager = SelectionManager.instance;
    }

    void OnMouseDown()
    {
        Debug.Log("CharacterController::OnMouseDown::");
        selectable.OnSelect();
        manager.OnSelect(this);
    }

    public void Deselect()
    {
        selectable.OnDeselect();
    }

    public void Move(Vector3 destination)
    {
        moveable.MoveAsync(destination, speed);
    }
}
