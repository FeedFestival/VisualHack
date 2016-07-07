using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class Zone : MonoBehaviour
{
    public ZoneType ZoneType;

    // Use this for initialization
    void Start()
    {
        //if Type Solid
    }

    void OnTriggerEnter(Collider objC)
    {
        switch (ZoneType)
        {
            case ZoneType.DeathZone:

                if (objC.CompareTag("Sphere"))
                {
                    Debug.Log("Dead");
                }
                break;

            case ZoneType.Walkable:


                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void OnTriggerExit(Collider objC)
    {
    }
}
