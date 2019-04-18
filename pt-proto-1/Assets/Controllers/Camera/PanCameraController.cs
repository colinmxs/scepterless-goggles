using UnityEngine;

public class PanCameraController : MonoBehaviour
{
    public float mainSpeed = 20.0f; //regular speed
    Vector3 velocity = Vector3.zero;

    void Update()
    {
        UpdateVelocity();
        velocity = velocity * mainSpeed * Time.deltaTime;
        transform.Translate(velocity);
    }
    
    private void UpdateVelocity()
    {
        velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            velocity += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity += new Vector3(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity += new Vector3(1, 0, 0);
        }
    }
}