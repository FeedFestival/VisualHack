using UnityEngine;
using System.Collections;

public class Logic_DeathZone : MonoBehaviour {

    /// <sumary>
    ///     For when the anything enters in contact with the water.
    /// </sumary>

    void OnTriggerEnter(Collider obj)
    {
        if (string.Equals(obj.gameObject.name, "Sphere"))
        {
            var objScript = obj.GetComponent<Logic_ControlObject>();
            objScript.Death();
        }

        if (obj.gameObject.CompareTag("Object_Box"))
        {
            var objScript = obj.transform.parent.gameObject.GetComponent<Logic_PushedObject>();
            objScript.Death();
            DeactivateDeathZone();
        }
    }
    public void DeactivateDeathZone() {
        this.gameObject.GetComponent<Collider>().enabled = false;
    }
}