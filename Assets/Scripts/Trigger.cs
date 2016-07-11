using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Types;

public class Trigger : MonoBehaviour
{
    [SerializeField]
    private ObjectState _triggerState;
    [SerializeField]
    public ObjectState TriggerState
    {
        get { return _triggerState; }
        set
        {
            _triggerState = value;
            if (_triggerState == ObjectState.Activated)
                Bridge.BridgeState = ObjectState.Activated;
            else
                Bridge.BridgeState = ObjectState.Deactivated;
        }
    }

    public Bridge Bridge;

    public void Initialize(Bridge bridge)   // from DB
    {
        Bridge = bridge;
    }

    void OnTriggerEnter(Collider objC)
    {
        if (TriggerState == ObjectState.Activated) return;
        if (string.Equals(objC.gameObject.name, "TriggerCollider"))
            TriggerState = ObjectState.Activated;
    }

    void OnTriggerExit(Collider objC)
    {
        if (TriggerState != ObjectState.Activated) return;
        if (string.Equals(objC.gameObject.name, "TriggerCollider"))
            TriggerState = ObjectState.Deactivated;
    }
}
