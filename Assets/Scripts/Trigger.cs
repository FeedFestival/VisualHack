using UnityEngine;
using System.Collections;
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

    // Use this for initialization
    void Start()
    {

    }

    void OnTriggerEnter(Collider objC)
    {
        if (TriggerState != ObjectState.Activated)
            if (objC.tag.Contains("Box"))
                TriggerState = ObjectState.Activated;
    }
    void OnTriggerExit(Collider objC)
    {
        if (TriggerState == ObjectState.Activated)
            if (objC.tag.Contains("Box"))
                TriggerState = ObjectState.Deactivated;
    }
}
