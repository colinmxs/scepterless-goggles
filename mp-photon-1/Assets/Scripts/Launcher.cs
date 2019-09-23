using UnityEngine;

public class Launcher : MonoBehaviour
{
    public float Force;
    public int MinHorizontal;
    public int MaxHorizontal;
    public int MaxVertical;
    public int MinVertical;
    private System.Random random;   

    internal void LaunchBall(GameObject ball)
    {
        var ballRb = ball.GetComponent<Rigidbody2D>();
        var x = random.Next(MinHorizontal, MaxHorizontal);
        var y = random.Next(MinVertical, MaxVertical);
        var targetVelocity = new Vector2(x, y);
        var moveVector = Vector3.zero;
        ballRb.velocity = Vector3.SmoothDamp(ballRb.velocity, targetVelocity * Force, ref moveVector, 0.05f);
    }

    private void Awake()
    {
        random = new System.Random();
    }   
}