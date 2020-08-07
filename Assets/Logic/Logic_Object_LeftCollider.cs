using UnityEngine;
using System.Collections;

public class Logic_Object_LeftCollider : MonoBehaviour
{

    /***************************\
    *                           *
    * Left Side of the Object	*
    * 				            *
    *                           *
    \***************************/

    public Logic_PushedObject ObjectParent;

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.name == "Sphere")
        {
            ObjectParent.setPushingParent(obj.transform.parent.gameObject.GetComponent<Logic_ControlObject>());

            // Push right !
            if (ObjectParent.lock_rightPushing == false)
            {
                ObjectParent.Push(6);   // right
            }
            else
            {
                ObjectParent.Player.LockPushing(6); // right
            }
        }
        if (obj.gameObject.name == "right_Side")
        {
            // Stop pushing left !
            ObjectParent.LockPushing(4);    // left.
            if (ObjectParent.Player)
            {
                if (ObjectParent.Player.pushingFrom_Right)
                {
                    ObjectParent.Player.LockPushing(4); // left.
                }
            }
        }

        if (obj.gameObject.name == "Border_Left")
        {
            // Stop pushing left !
            ObjectParent.LockPushing(4);    // left.
            if (ObjectParent.Player)
            {
                if (ObjectParent.Player.pushingFrom_Right)
                {
                    ObjectParent.Player.LockPushing(4); // left.
                }
            }
        }
    }
    void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject.name == "Sphere")
        {
            // Player can push right again !
            ObjectParent.Player.DontLockPushing(6); // right
            ObjectParent.removePushingParent();
            ObjectParent.DontPush(6);   // right
        }
        if (obj.gameObject.name == "right_Side")
        {

            ObjectParent.DontLockPushing(4);    // left.
        }
        if (obj.gameObject.name == "Border_Left")
        {
            // Player can (probably) push left again !
            ObjectParent.DontLockPushing(4);    // left.
        }
    }
}