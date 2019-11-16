using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentController : MonoBehaviour
{
    public RealtimePlayer player;
    Transform _transform;

    private void Start()
    {
        _transform = transform;
        name = player.NickName;
    }

    private void Update()
    {
        if(_transform.position.x != player.XPosition || _transform.position.y != player.YPosition)
        {
            _transform.position = new Vector3(player.XPosition, player.YPosition);
        }
    }
}
