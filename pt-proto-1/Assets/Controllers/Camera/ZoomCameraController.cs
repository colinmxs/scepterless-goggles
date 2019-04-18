using UnityEngine;

public class ZoomCameraController : MonoBehaviour
{
    public float stepSize = 0.5f;
    public float maxZoom;
    public float minZoom;

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            Camera.main.orthographicSize -= stepSize;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            Camera.main.orthographicSize += stepSize;
        }
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
    }
}
