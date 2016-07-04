using System;
using UnityEngine;
using Assets.Scripts.Types;

public class Box : MonoBehaviour
{
    public GameObject Pic;

    // Parent
    public Sphere Sphere;

    public Controller Controller;

    // Input from user
    public bool RightPushingLocked = true;
    public bool LeftPushingLocked = true;
    public bool UpPushingLocked = true;
    public bool DownPushingLocked = true;

    // Use this for initialization
    void Awake()
    {
        Transform[] allChildren = transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            switch (child.gameObject.name)
            {
                case "left_Side":

                    child.gameObject.GetComponent<BoxCollider>().Initialize(this, Direction.Left);
                    break;

                case "right_Side":

                    child.gameObject.GetComponent<BoxCollider>().Initialize(this, Direction.Right);
                    break;

                case "up_Side":

                    child.gameObject.GetComponent<BoxCollider>().Initialize(this, Direction.Up);
                    break;

                case "down_Side":

                    child.gameObject.GetComponent<BoxCollider>().Initialize(this, Direction.Down);
                    break;
            }
        }
        Controller = GetComponent<Controller>();
        Controller.Initialize(this);
    }
}