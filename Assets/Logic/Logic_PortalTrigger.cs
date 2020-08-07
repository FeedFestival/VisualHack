using UnityEngine;
using System.Collections;

public class Logic_PortalTrigger : MonoBehaviour
{
    public int index;
    public GameRules gameRules;

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Object_Box"))
        {
            gameRules.activatePortal(index);
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject.CompareTag("Object_Box"))
        {
            gameRules.DEactivatePortal(index);
        }
    }
}
