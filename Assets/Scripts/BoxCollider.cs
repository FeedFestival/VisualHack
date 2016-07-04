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

    void OnTriggerEnter(Collider objC)
    {
        switch (ColliderPosition)
        {
            case Direction.Up:

                if (objC.CompareTag("Sphere"))
                {
                    _box.Sphere = objC.transform.parent.gameObject.GetComponent<Sphere>();

                    if (_box.Controller.LockDownPushing == false)
                        _box.Controller.Push(Direction.Down);
                    else
                        _box.Sphere.Controller.LockPushing(Direction.Down);
                }
                // Stop pushing up !
                if (objC.CompareTag("BoxDown") || objC.CompareTag("SolidUp"))
                {
                    _box.Controller.LockPushing(Direction.Up);
                    if (_box.Sphere && _box.Sphere.Controller.PushingFromDown)
                        _box.Sphere.Controller.LockPushing(Direction.Up);
                }
                break;

            case Direction.Right:

                if (objC.CompareTag("Sphere"))
                {
                    _box.Sphere = objC.transform.parent.gameObject.GetComponent<Sphere>();

                    if (_box.Controller.LockLeftPushing == false)
                        _box.Controller.Push(Direction.Left);
                    else
                        _box.Sphere.Controller.LockPushing(Direction.Left);
                }
                // Stop pushing right !
                if (objC.CompareTag("BoxLeft") || objC.CompareTag("SolidRight"))
                {
                    _box.Controller.LockPushing(Direction.Right);
                    if (_box.Sphere && _box.Sphere.Controller.PushingFromLeft)
                        _box.Sphere.Controller.LockPushing(Direction.Right);
                }
                break;

            case Direction.Down:

                if (objC.CompareTag("Sphere"))
                {
                    _box.Sphere = objC.transform.parent.gameObject.GetComponent<Sphere>();

                    if (_box.Controller.LockUpPushing == false)
                        _box.Controller.Push(Direction.Up);
                    else
                        _box.Sphere.Controller.LockPushing(Direction.Up);
                }
                // Stop pushing down !
                if (objC.CompareTag("BoxUp") || objC.CompareTag("SolidDown"))
                {
                    _box.Controller.LockPushing(Direction.Down);
                    if (_box.Sphere && _box.Sphere.Controller.PushingFromUp)
                        _box.Sphere.Controller.LockPushing(Direction.Down);
                }
                break;

            case Direction.Left:

                if (objC.CompareTag("Sphere"))
                {
                    _box.Sphere = objC.transform.parent.gameObject.GetComponent<Sphere>();

                    if (_box.Controller.LockRightPushing == false)
                        _box.Controller.Push(Direction.Right);
                    else
                        _box.Sphere.Controller.LockPushing(Direction.Right);
                }
                // Stop pushing left !
                if (objC.CompareTag("BoxRight") || objC.CompareTag("SolidLeft"))
                {
                    _box.Controller.LockPushing(Direction.Left);
                    if (_box.Sphere && _box.Sphere.Controller.PushingFromRight)
                        _box.Sphere.Controller.LockPushing(Direction.Left);
                }
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void OnTriggerExit(Collider objC)
    {
        switch (ColliderPosition)
        {
            case Direction.Up:

                if (objC.CompareTag("Sphere"))
                {
                    // Sphere can push down again !
                    if (_box.Sphere)
                        _box.Sphere.Controller.DontLockPushing(Direction.Down);
                    _box.Sphere = null;
                    _box.Controller.DontPush(Direction.Down);
                }
                if (objC.CompareTag("BoxDown") || objC.CompareTag("SolidUp"))
                    _box.Controller.DontLockPushing(Direction.Up);
                break;

            case Direction.Right:

                if (objC.CompareTag("Sphere"))
                {
                    // Sphere can push left again !
                    _box.Sphere.Controller.DontLockPushing(Direction.Left);
                    _box.Sphere = null;
                    _box.Controller.DontPush(Direction.Left);
                }
                if (objC.CompareTag("BoxLeft") || objC.CompareTag("SolidRight"))
                    _box.Controller.DontLockPushing(Direction.Right);
                break;

            case Direction.Down:

                if (objC.CompareTag("Sphere"))
                {
                    // Sphere can push up again !
                    _box.Sphere.Controller.DontLockPushing(Direction.Up);
                    _box.Sphere = null;
                    _box.Controller.DontPush(Direction.Up);
                }
                if (objC.CompareTag("BoxUp") || objC.CompareTag("SolidDown"))
                    _box.Controller.DontLockPushing(Direction.Down);
                break;

            case Direction.Left:

                if (objC.CompareTag("Sphere"))
                {
                    // Sphere can push right again !
                    _box.Sphere.Controller.DontLockPushing(Direction.Right);
                    _box.Sphere = null;
                    _box.Controller.DontPush(Direction.Right);
                }
                if (objC.CompareTag("BoxRight") || objC.CompareTag("SolidLeft"))
                    _box.Controller.DontLockPushing(Direction.Left);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}