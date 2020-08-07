using UnityEngine;
using System.Collections;

public class Logic_Object_RightCollider : MonoBehaviour {

    /***************************\
    *                           *
    * Right Side of the Object	*
    * 				            *
    *                           *
    \***************************/

    public Logic_PushedObject ObjectParent;
    Transform thisObject;

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.name == "Sphere")
        {
            ObjectParent.setPushingParent (obj.transform.parent.gameObject.GetComponent<Logic_ControlObject>());
            
            // Push left !
            if (ObjectParent.lock_leftPushing == false)
            {
                ObjectParent.Push(4);   // left.
            }
            else {
                ObjectParent.Player.LockPushing(4); // left.
            }
        }
        if (obj.gameObject.name == "left_Side")
        {
            // Stop pushing right !
            ObjectParent.LockPushing(6);    // right
            if (ObjectParent.Player)
            {
                if (ObjectParent.Player.pushingFrom_Left)
                {
                    ObjectParent.Player.LockPushing(6); // right
                }
            }
        }
        if (obj.gameObject.name == "Border_Right")
        {
            // Stop pushing right !
            ObjectParent.LockPushing(6);    // right
            if (ObjectParent.Player)
            {
                if (ObjectParent.Player.pushingFrom_Left)
                {
                    ObjectParent.Player.LockPushing(6); // right
                }
            }
        }
    }
    void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject.name == "Sphere")
        {
            // Player can push left again !
            ObjectParent.Player.DontLockPushing(4); // left.
            ObjectParent.removePushingParent();
            ObjectParent.DontPush(4);   // left.
        }
        if (obj.gameObject.name == "left_Side")
        {

            ObjectParent.DontLockPushing(6);    // right
        }
        if (obj.gameObject.name == "Border_Right")
        {
            // Player can (probably) push right again !
            ObjectParent.DontLockPushing(6);    // right.
        }
    }

    void Awake()
    {
        thisObject = this.transform;
    }
}
