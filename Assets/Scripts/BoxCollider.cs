using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class BoxCollider : MonoBehaviour
{
    public Direction ColliderPosition;

    private Box _box;

    public void Initialize(Box box, Direction colPosition)
    {
        _box = box;
        ColliderPosition = colPosition;
    }

    void OnTriggerEnter(Collider obj)
    {
        var objName = obj.gameObject.name;

        switch (ColliderPosition)
        {
            case Direction.Up:

                if (objName == "Sphere")
                {
                    _box.Player = obj.transform.parent.gameObject.GetComponent<Controller>();

                    if (_box.LockDownPushing == false)
                        _box.Push(Direction.Down);
                    else
                        _box.Player.LockPushing(Direction.Down);
                }
                // Stop pushing up !
                if (objName == "down_Side" || objName == "BorderUp")
                {
                    _box.LockPushing(Direction.Up);
                    if (_box.Player && _box.Player.PushingFromDown)
                        _box.Player.LockPushing(Direction.Up);
                }
                break;

            case Direction.Right:

                if (objName == "Sphere")
                {
                    _box.Player = obj.transform.parent.gameObject.GetComponent<Controller>();

                    if (_box.LockLeftPushing == false)
                        _box.Push(Direction.Left);
                    else
                        _box.Player.LockPushing(Direction.Left);
                }
                // Stop pushing right !
                if (objName == "left_Side" || objName == "BorderRight")
                {
                    _box.LockPushing(Direction.Right);
                    if (_box.Player && _box.Player.PushingFromLeft)
                        _box.Player.LockPushing(Direction.Right);
                }
                break;

            case Direction.Down:

                if (objName == "Sphere")
                {
                    _box.Player = obj.transform.parent.gameObject.GetComponent<Controller>();

                    if (_box.LockUpPushing == false)
                        _box.Push(Direction.Up);
                    else
                        _box.Player.LockPushing(Direction.Up);
                }
                // Stop pushing down !
                if (objName == "up_Side" || objName == "BorderDown")
                {
                    _box.LockPushing(Direction.Down);
                    if (_box.Player && _box.Player.PushingFromUp)
                        _box.Player.LockPushing(Direction.Down);
                }
                break;

            case Direction.Left:

                if (objName == "Sphere")
                {
                    _box.Player = obj.transform.parent.gameObject.GetComponent<Controller>();

                    if (_box.LockRightPushing == false)
                        _box.Push(Direction.Right);
                    else
                        _box.Player.LockPushing(Direction.Right);
                }
                // Stop pushing left !
                if (objName == "right_Side" || objName == "BorderLeft")
                {
                    _box.LockPushing(Direction.Left);
                    if (_box.Player && _box.Player.PushingFromRight)
                        _box.Player.LockPushing(Direction.Left);
                }
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void OnTriggerExit(Collider obj)
    {
        var objName = obj.gameObject.name;
        switch (ColliderPosition)
        {
            case Direction.Up:

                if (objName == "Sphere")
                {
                    // Player can push down again !
                    if (_box.Player)
                        _box.Player.DontLockPushing(Direction.Down);
                    _box.Player = null;
                    _box.DontPush(Direction.Down);
                }
                if (objName == "down_Side" || objName == "BorderUp")
                    _box.DontLockPushing(Direction.Up);
                break;

            case Direction.Right:

                if (objName == "Sphere")
                {
                    // Player can push left again !
                    _box.Player.DontLockPushing(Direction.Left);
                    _box.Player = null;
                    _box.DontPush(Direction.Left);
                }
                if (objName == "left_Side" || objName == "Border_Right")
                    _box.DontLockPushing(Direction.Right);
                break;

            case Direction.Down:

                if (objName == "Sphere")
                {
                    // Player can push up again !
                    _box.Player.DontLockPushing(Direction.Up);
                    _box.Player = null;
                    _box.DontPush(Direction.Up);
                }
                if (objName == "up_Side" || objName == "BorderDown")
                    _box.DontLockPushing(Direction.Down);
                break;

            case Direction.Left:

                if (objName == "Sphere")
                {
                    // Player can push right again !
                    _box.Player.DontLockPushing(Direction.Right);
                    _box.Player = null;
                    _box.DontPush(Direction.Right);
                }
                if (objName == "right_Side" || objName == "BorderLeft")
                    _box.DontLockPushing(Direction.Left);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
