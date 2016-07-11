using UnityEngine;
using System.Collections;

public class Finish : MonoBehaviour
{
    void OnTriggerEnter(Collider objC)
    {
        if (objC.CompareTag("Sphere"))
            objC.transform.parent.GetComponent<Sphere>().YouWinBro();
    }
}