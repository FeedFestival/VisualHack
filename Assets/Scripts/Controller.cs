using System;
using Assets.Scripts.Types;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Sphere _sphere;
    private Box _box;

    private Transform _thisTransform;

    // Input logic variables
    private float _time;
    private readonly float _ratio = 0.02f;
    private readonly float _speed = 3f;

    private bool _pushRight;
    private bool _pushLeft;
    private bool _pushUp;
    private bool _pushDown;

    public bool LockRightPushing;
    public bool LockLeftPushing;
    public bool LockUpPushing;
    public bool LockDownPushing;

    // predefined
    private Vector3 _startMarker;
    private Vector3 _endMarkerRight;
    private Vector3 _endMarkerLeft;
    private Vector3 _endMarkerUp;
    private Vector3 _endMarkerDown;

    public bool PushingFromRight;
    public bool PushingFromLeft;
    public bool PushingFromUp;
    public bool PushingFromDown;

    public void Initialize(object obj)
    {
        if (obj is Sphere)
            _sphere = obj as Sphere;
        if (obj is Box)
            _box = obj as Box;

        _thisTransform = transform;

        CalculateNewCoord();
    }

    private void MoveUpdate(Vector3 endMarker, ref bool referenceBool)
    {
        _thisTransform.position = Vector3.Lerp(_startMarker, endMarker, _time);
        _time = _time + _ratio * _speed;

        if (!(_time >= 1)) return;

        _time = 0f;
        _thisTransform.position = endMarker;
        CalculateNewCoord();
        referenceBool = false;
    }

    bool CheckUserPress(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:

                if (_sphere)
                    return _sphere.UserPressedUp && _sphere.UpPushingLocked == false;
                //return _box.Sphere && _box.UpPushingLocked == false;
                return _box.Sphere && _box.Sphere.UserPressedUp && _box.UpPushingLocked == false;

            case Direction.Right:

                if (_sphere)
                    return _sphere.UserPressedRight && _sphere.RightPushingLocked == false;
                //return _box.Sphere && _box.RightPushingLocked == false;
                return _box.Sphere && _box.Sphere.UserPressedRight && _box.RightPushingLocked == false;

            case Direction.Down:

                if (_sphere)
                    return _sphere.UserPressedDown && _sphere.DownPushingLocked == false;
                //return _box.Sphere && _box.DownPushingLocked == false;
                return _box.Sphere && _box.Sphere.UserPressedDown && _box.DownPushingLocked == false;

            case Direction.Left:

                if (_sphere)
                    return _sphere.UserPressedLeft && _sphere.LeftPushingLocked == false;
                //return _box.Sphere && _box.LeftPushingLocked == false;
                return _box.Sphere && _box.Sphere.UserPressedLeft && _box.LeftPushingLocked == false;

            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }

    bool CheckLockedPush(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:

                if (_sphere)
                    return _sphere.UpPushingLocked == false;
                return _box.UpPushingLocked == false;

            case Direction.Right:

                if (_sphere)
                    return _sphere.RightPushingLocked == false;
                return _box.RightPushingLocked == false;

            case Direction.Down:

                if (_sphere)
                    return _sphere.DownPushingLocked == false;
                return _box.DownPushingLocked == false;

            case Direction.Left:

                if (_sphere)
                    return _sphere.LeftPushingLocked == false;
                return _box.LeftPushingLocked == false;

            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }

    void FixedUpdate()
    {
        if (_pushLeft == false && _pushUp == false && _pushDown == false)
            if (CheckUserPress(Direction.Right))
                _pushRight = true;

        if (_pushRight == false && _pushUp == false && _pushDown == false)
            if (CheckUserPress(Direction.Left))
                _pushLeft = true;

        if (_pushRight == false && _pushLeft == false && _pushDown == false)
            if (CheckUserPress(Direction.Up))
                _pushUp = true;

        if (_pushRight == false && _pushLeft == false && _pushUp == false)
            if (CheckUserPress(Direction.Down))
                _pushDown = true;

        if (_pushRight)
            MoveUpdate(_endMarkerRight, ref _pushRight);
        if (_pushLeft)
            MoveUpdate(_endMarkerLeft, ref _pushLeft);
        if (_pushUp)
            MoveUpdate(_endMarkerUp, ref _pushUp);
        if (_pushDown)
            MoveUpdate(_endMarkerDown, ref _pushDown);
    }

    public void CalculateNewCoord()
    {
        _startMarker = _thisTransform.position;
        _endMarkerRight = new Vector3(_thisTransform.position.x + 1f, _thisTransform.position.y, _thisTransform.position.z);
        _endMarkerLeft = new Vector3(_thisTransform.position.x - 1f, _thisTransform.position.y, _thisTransform.position.z);
        _endMarkerUp = new Vector3(_thisTransform.position.x, _thisTransform.position.y + 1f, _thisTransform.position.z);
        _endMarkerDown = new Vector3(_thisTransform.position.x, _thisTransform.position.y - 1f, _thisTransform.position.z);
    }

    /***************************\//----------------------------------------------\
    *                           												*
    * The Colliding effects 													*
    * 					        												*
    *                           												*
    \***************************/
    // ---------------------------------------------\

    void OnTriggerEnter(Collider foreignObjectCollider)
    {
        LockPushing(foreignObjectCollider.gameObject.tag, true);

        if (foreignObjectCollider.CompareTag("BoxUp"))
        {
            PushingFromUp = true;
        }
        if (foreignObjectCollider.CompareTag("BoxRight"))
        {
            PushingFromRight = true;
        }
        if (foreignObjectCollider.CompareTag("BoxDown"))
        {
            PushingFromDown = true;
        }
        if (foreignObjectCollider.CompareTag("BoxLeft"))
        {
            PushingFromLeft = true;
        }
    }

    void OnTriggerExit(Collider foreignObjectCollider)
    {
        LockPushing(foreignObjectCollider.gameObject.tag, false);

        if (foreignObjectCollider.CompareTag("BoxUp"))
        {
            PushingFromUp = false;
        }
        if (foreignObjectCollider.CompareTag("BoxRight"))
        {
            PushingFromRight = false;
        }
        if (foreignObjectCollider.CompareTag("BoxDown"))
        {
            PushingFromDown = false;
        }
        if (foreignObjectCollider.CompareTag("BoxLeft"))
        {
            PushingFromLeft = false;
        }
    }
    
    private void LockPushing(string tag, bool value)
    {
        switch (tag)
        {
            case "SolidUp":
                if (_sphere) _sphere.UpPushingLocked = value;
                else _box.UpPushingLocked = value;
                break;
            case "SolidRight":
                if (_sphere) _sphere.RightPushingLocked = value;
                else _box.RightPushingLocked = value;
                break;
            case "SolidDown":
                if (_sphere) _sphere.DownPushingLocked = value;
                else _box.DownPushingLocked = value;
                break;
            case "SolidLeft":
                if (_sphere) _sphere.LeftPushingLocked = value;
                else _box.LeftPushingLocked = value;
                break;
        }
    }
    
    public void Push(Direction directionIndex)
    {
        var direction = (Direction)directionIndex;
        switch (direction)
        {
            case Direction.Up:

                if (_sphere) _sphere.UpPushingLocked = LockUpPushing;
                else _box.UpPushingLocked = LockUpPushing;
                break;

            case Direction.Right:

                if (_sphere) _sphere.RightPushingLocked = LockRightPushing;
                else _box.RightPushingLocked = LockRightPushing;
                break;

            case Direction.Down:

                if (_sphere) _sphere.DownPushingLocked = LockDownPushing;
                else _box.DownPushingLocked = LockDownPushing;
                break;

            case Direction.Left:

                if (_sphere) _sphere.LeftPushingLocked = LockLeftPushing;
                else _box.LeftPushingLocked = LockLeftPushing;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void DontPush(Direction directionIndex)
    {
        var direction = (Direction)directionIndex;
        switch (direction)
        {
            case Direction.Up:

                if (_sphere) _sphere.UpPushingLocked = true;
                else _box.UpPushingLocked = true;
                break;

            case Direction.Right:

                if (_sphere) _sphere.RightPushingLocked = true;
                else _box.RightPushingLocked = true;
                break;

            case Direction.Down:

                if (_sphere) _sphere.DownPushingLocked = true;
                else _box.DownPushingLocked = true;
                break;

            case Direction.Left:

                if (_sphere) _sphere.LeftPushingLocked = true;
                else _box.LeftPushingLocked = true;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void LockPushing(Direction directionIndex)
    {
        var direction = (Direction)directionIndex;

        DontPush(direction);

        switch (direction)
        {
            case Direction.Up:

                LockUpPushing = true;
                break;

            case Direction.Right:

                LockRightPushing = true;
                break;

            case Direction.Down:

                LockDownPushing = true;
                break;

            case Direction.Left:

                LockLeftPushing = true;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void DontLockPushing(Direction directionIndex)
    {
        var direction = (Direction)directionIndex;
        switch (direction)
        {
            case Direction.Up:

                LockUpPushing = false;
                break;

            case Direction.Right:

                LockRightPushing = false;
                break;

            case Direction.Down:

                LockDownPushing = false;
                break;

            case Direction.Left:

                LockLeftPushing = false;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        Push(direction);
    }
}