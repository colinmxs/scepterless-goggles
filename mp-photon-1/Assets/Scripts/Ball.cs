using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    private GameManager GameManager;    

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        GameManager = gameControllerObject.GetComponent<GameManager>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {        
        if (col.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            GameManager.UpdateScore(col.gameObject.name);
        }
    }
}
