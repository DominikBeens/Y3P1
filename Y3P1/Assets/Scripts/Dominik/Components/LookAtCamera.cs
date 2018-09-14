﻿using UnityEngine;
using Y3P1;

public class LookAtCamera : MonoBehaviour 
{

    private Transform target;
    private Vector3 targetFixedPos;

    private void LateUpdate()
    {
        if (Player.localPlayer)
        {
            if (!target)
            {
                target = Player.localPlayer.playerCam.camLookAtPoint;
            }

            targetFixedPos = target.position;
            targetFixedPos.x = transform.position.x;

            transform.LookAt(targetFixedPos);
            transform.Rotate(0, 180, 0);
        }
    }
}
