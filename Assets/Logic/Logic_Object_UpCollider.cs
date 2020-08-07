using UnityEngine;
using System.Collections;

public class Logic_Object_UpCollider : MonoBehaviour {

    /***************************\
    *                           *
    * Upper Side of the Object	*
    * 				            *
    *                           *
    \***************************/

    public Logic_PushedObject ObjectParent;

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.name == "Sphere")
        {
            ObjectParent.setPushingParent(obj.transform.parent.gameObject.GetComponent<Logic_ControlObject>());

            // Push down !
            if (ObjectParent.lock_downPushing == false)
            {
                ObjectParent.Push(2);   // down
            }
            else
            {
                ObjectParent.Player.LockPushing(2); // down
            }
        }
        if (obj.gameObject.name == "down_Side")
        {
            // Stop pushing up !
            ObjectParent.LockPushing(8);    // up
            if (ObjectParent.Player)
            {
                if (ObjectParent.Player.pushingFrom_Down)
                {
                    ObjectParent.Player.LockPushing(8); // up
                }
            }
        }
        if (obj.gameObject.name == "Border_Up")
        {
            // Stop pushing up !
            ObjectParent.LockPushing(8);    // up
            if (ObjectParent.Player)
            {
                if (ObjectParent.Player.pushingFrom_Down)
                {
                    ObjectParent.Player.LockPushing(8); // up
                }
            }
        }
    }
    void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject.name == "Sphere")
        {

            // Player can push down again !
            if (ObjectParent.Player)
            {
                ObjectParent.Player.DontLockPushing(2); // down
            }
            ObjectParent.removePushingParent();
            ObjectParent.DontPush(2);   // down
        }
        if (obj.gameObject.name == "down_Side")
        {

            ObjectParent.DontLockPushing(8);    // up
        }
        if (obj.gameObject.name == "Border_Up")
        {
            // Player can (probably) push up again !
            ObjectParent.DontLockPushing(8);    // up.
        }
    }
}