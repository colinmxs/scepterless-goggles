using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverFollow : MonoBehaviour
{
    public Transform target;
    
    private void Update()
    {
        var wantedPos = Camera.main.WorldToScreenPoint(target.position);
        transform.position = new Vector3(wantedPos.x, wantedPos.y);
    }
}
