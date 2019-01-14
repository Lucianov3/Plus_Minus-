﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSliderScript : MonoBehaviour
{
    public bool onPosition = false;

    private Transform childObject;

    [SerializeField] private float timeTillMaxLength = 2f;
    [SerializeField] private float count = 0;

    private BreakpointParentScript parent;

    private bool counted = false;

    private void Start()
    {
        childObject = transform.GetChild(0);
        parent = transform.parent.GetComponent<BreakpointParentScript>();
    }

    private void Update()
    {
        if (onPosition)
        {
            if (count <= timeTillMaxLength)
            {
                count += Time.deltaTime;
                childObject.localScale = new Vector3(count / timeTillMaxLength, childObject.localScale.y, childObject.localScale.z);
            }
            else if (!counted)
            {
                counted = true;
                parent.Counter++;
            }
        }
        else if (counted)
        {
            counted = false;
            parent.Counter--;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            onPosition = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            onPosition = false;
            count = 0;
            childObject.localScale = new Vector3(count, childObject.localScale.y, childObject.localScale.z);
        }
    }
}