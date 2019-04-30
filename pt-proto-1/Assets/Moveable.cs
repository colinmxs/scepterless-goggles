using System.Collections;
using UnityEngine;

public class Moveable : MonoBehaviour
{

    Transform _transform;
    Vector3 destination;
    float speed;

    void Start()
    {
        _transform = transform;
    }

    public void MoveAsync(Vector3 destination, float speed)
    {
        this.destination = destination;
        this.speed = speed;
    }

    void FixedUpdate()
    {
        float step = (speed / (_transform.position - destination).magnitude) * Time.fixedDeltaTime;
        transform.position = Vector3.Lerp(_transform.position, destination, step);
    }
}
