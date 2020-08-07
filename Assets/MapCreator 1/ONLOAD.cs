using UnityEngine;
using System.Collections;

public class ONLOAD : MonoBehaviour {

    public string OpenMap_filename;

    public string SavedMap_filename;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
