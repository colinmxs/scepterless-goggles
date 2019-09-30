using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            gameManager.UpdateScore(col.gameObject.name);
        }
    }
}
