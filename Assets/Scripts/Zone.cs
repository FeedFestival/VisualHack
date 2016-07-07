using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class Zone : MonoBehaviour
{
    public ZoneType ZoneType;
    
    void OnTriggerEnter(Collider objC)
    {
        switch (ZoneType)
        {
            case ZoneType.DeathZone:

                if (objC.CompareTag("Sphere"))
                {
                    objC.transform.parent.GetComponent<Sphere>().Main.InitGame();
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
