﻿using UnityEngine;
using Y3P1;

public class PlayerCamera : MonoBehaviour 
{

    private Transform target;
    private Vector3 offset;

    [HideInInspector] public Camera cameraComponent;
    [HideInInspector] public Transform camLookAtPoint;

    [SerializeField] private bool lerp = true;
    [SerializeField] private float moveSpeed = 1f;

    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();
        camLookAtPoint = transform.GetChild(0);
    }

    public void Initialize()
    {
        target = Player.localPlayerObject.transform;
        offset = target.position - transform.position;

        transform.SetParent(lerp ? null : transform);
    }

    private void LateUpdate()
    {
        if (target)
        {
            transform.position = lerp ? Vector3.Lerp(transform.position, target.position - offset, Time.deltaTime * moveSpeed) : target.position - offset;
        }
    }
}
