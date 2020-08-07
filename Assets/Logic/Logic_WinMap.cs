using UnityEngine;
using System.Collections;

public class Logic_WinMap : MonoBehaviour {

    public GameRules gameRules;

    void OnTriggerEnter(Collider obj)
    {
        if (gameRules.isPortalTriggersComplete)
        {
            if (string.Equals(obj.gameObject.name, "Sphere"))
            {
                var objScript = obj.GetComponent<Logic_ControlObject>();
                objScript.Death();
            }
        }
    }
}
