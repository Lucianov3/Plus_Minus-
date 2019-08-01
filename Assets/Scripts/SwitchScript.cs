﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour
{
    public int Channel = 0;
    private int numberOfObjectOnSwitch = 0;

    private void Start()
    {
        TransmitterEventManager.NumberOfTransmitterPerChannel[Channel]++;
        TransmitterEventManager.IsChannelInMulitMode[Channel] = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger || other.CompareTag("Pick Up"))
        {
            numberOfObjectOnSwitch++;
            if (numberOfObjectOnSwitch == 1)
            {
                ActivateSwitch();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger || other.CompareTag("Pick Up"))
        {
            numberOfObjectOnSwitch--;
            if (numberOfObjectOnSwitch == 0)
            {
                DeactivateSwitch();
            }
        }
    }

    private void ActivateSwitch()
    {
        TransmitterEventManager.NumberOfActivatedTransmitterPerChannel[Channel]++;
        if (TransmitterEventManager.OnTransmitterActivation != null)
        {
            TransmitterEventManager.OnTransmitterActivation(Channel);
        }
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void DeactivateSwitch()
    {
        TransmitterEventManager.NumberOfActivatedTransmitterPerChannel[Channel]--;
        if (TransmitterEventManager.OnTransmitterDeactivation != null)
        {
            TransmitterEventManager.OnTransmitterDeactivation(Channel);
        }
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}