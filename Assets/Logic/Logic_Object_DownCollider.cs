using UnityEngine;
using System.Collections;

public class Logic_Object_DownCollider : MonoBehaviour {

    /***************************\
    *                           *
    * Down Side of the Object	*
    * 				            *
    *                           *
    \***************************/

    public Logic_PushedObject ObjectParent;
    Transform thisObject;

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.name == "Sphere")
        {
            ObjectParent.setPushingParent(obj.transform.parent.gameObject.GetComponent<Logic_ControlObject>());

            // Push up !
            if (ObjectParent.lock_upPushing == false)
            {
                ObjectParent.Push(8);   // up
            }
            else
            {
                ObjectParent.Player.LockPushing(8); // up
            }
        }
        if (obj.gameObject.name == "up_Side")
        {
            // Stop pushing down !
            ObjectParent.LockPushing(2);    // down
            if (ObjectParent.Player)
            {
                if (ObjectParent.Player.pushingFrom_Up)
                {
                    ObjectParent.Player.LockPushing(2); // down
                }
            }
        }

        if (obj.gameObject.name == "Border_Down")
        {
            // Stop pushing down !
            ObjectParent.LockPushing(2);    // down
            if (ObjectParent.Player)
            {
                if (ObjectParent.Player.pushingFrom_Up)
                {
                    ObjectParent.Player.LockPushing(2); // down
                }
            }
        }
    }
    void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject.name == "Sphere")
        {
            // Player can push up again !
            ObjectParent.Player.DontLockPushing(8); // up
            ObjectParent.removePushingParent();
            ObjectParent.DontPush(8);   // up
        }
        if (obj.gameObject.name == "up_Side")
        {

            ObjectParent.DontLockPushing(2);    // down
        }
        if (obj.gameObject.name == "Border_Down")
        {
            // Player can (probably) push down again !
            ObjectParent.DontLockPushing(2);    // down.
        }
    }

    void Awake()
    {
        thisObject = this.transform;
    }
}
