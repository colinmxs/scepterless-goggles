using UnityEngine;
using UnityEngine.Events;

public class TargetController : MonoBehaviour
{
    public float maxX;
    public float minX;
    public float speed;
    internal bool dirRight = true;
    internal UnityEvent TargetNailed = new UnityEvent();

    private void Update()
    {
        if (dirRight)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else
            transform.Translate(-Vector2.right * speed * Time.deltaTime);

        if (transform.position.x >= maxX)
        {
            dirRight = false;
        }

        if (transform.position.x <= minX)
        {
            dirRight = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            TargetNailed.Invoke();
        }
    }
}
