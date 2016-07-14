using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

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
                    objC.transform.parent.GetComponent<Sphere>().YouDeadBro();
                }
                else if (string.Equals(objC.gameObject.name, "ZoneCollider"))
                {
                    objC.transform.parent.gameObject.GetComponent<Box>().YouDeadBro();
                    ZoneType = ZoneType.Walkable;
                }
                break;

            case ZoneType.Walkable:

                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}