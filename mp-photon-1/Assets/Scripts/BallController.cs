using UnityEngine;

public class BallController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name == "Player")
        {
            Destroy(gameObject);
        }
    }
}
