﻿using UnityEngine;
using UnityEngine.Events;

public class CollisionEventZone : MonoBehaviour
{

    private enum CollisionType
    {
        Trigger,
        Collider
    }
    [SerializeField]
    private CollisionType collisionType;

    [SerializeField]
    private string lookForTag;

    public UnityEvent OnCollisionEvent;
    [HideInInspector] public Transform eventCaller;

    [Space(10)]
    [SerializeField] private KeyCode key;
    public UnityEvent OnKeyDownEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (collisionType != CollisionType.Trigger)
        {
            return;
        }

        if (other.tag == lookForTag)
        {
            eventCaller = other.transform;
            OnCollisionEvent.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (collisionType != CollisionType.Trigger)
        {
            return;
        }

        if (other.tag == lookForTag)
        {
            if (Input.GetKeyDown(key))
            {
                eventCaller = other.transform;
                OnKeyDownEvent.Invoke();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collisionType != CollisionType.Collider)
        {
            return;
        }

        if (collision.transform.tag == lookForTag)
        {
            eventCaller = collision.transform;
            OnCollisionEvent.Invoke();
        }
    }
}
