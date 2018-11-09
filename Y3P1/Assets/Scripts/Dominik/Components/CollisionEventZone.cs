﻿using UnityEngine;
using UnityEngine.Events;

public class CollisionEventZone : MonoBehaviour
{

    private bool checkForInput;

    private enum CollisionType
    {
        Trigger,
        Collider
    }
    [SerializeField]
    private CollisionType collisionType;

    [SerializeField]
    private string lookForTag;

    public UnityEvent OnZoneEnterEvent;
    public UnityEvent OnZoneExitEvent;
    private Transform eventCaller;
    public Transform EventCaller { get { return eventCaller; } }

    [Space(10)]
    [SerializeField] private KeyCode key;
    public UnityEvent OnKeyDownEvent;

    private void Update()
    {
        if (checkForInput)
        {
            if (Input.GetKeyDown(key))
            {
                OnKeyDownEvent.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collisionType != CollisionType.Trigger)
        {
            return;
        }

        if (other.tag == lookForTag)
        {
            // Specific network check so that other players cannot trigger collision for us.
            if (lookForTag == "Player" && other.gameObject.layer != 9)
            {
                return;
            }

            eventCaller = other.transform;
            OnZoneEnterEvent.Invoke();

            checkForInput = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (collisionType != CollisionType.Trigger)
        {
            return;
        }

        if (other.tag == lookForTag)
        {
            // Specific network check so that other players cannot trigger collision for us.
            if (lookForTag == "Player" && other.gameObject.layer != 9)
            {
                return;
            }

            eventCaller = other.transform;
            OnZoneExitEvent.Invoke();

            checkForInput = false;
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
            OnZoneEnterEvent.Invoke();

            checkForInput = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collisionType != CollisionType.Collider)
        {
            return;
        }

        if (collision.transform.tag == lookForTag)
        {
            eventCaller = collision.transform;
            OnZoneExitEvent.Invoke();

            checkForInput = false;
        }
    }
}
