using System;
using Assets.Scripts.Types;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Sphere _sphere;

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

    public void Initialize(Sphere obj)
    {
        _sphere = obj;

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

                return _sphere.UserPressedUp && _sphere.UpPushingLocked == false;
            //return _box.Sphere && _box.UpPushingLocked == false;

            case Direction.Right:
                
                return _sphere.UserPressedRight && _sphere.RightPushingLocked == false;
            //return _box.Sphere && _box.RightPushingLocked == false;

            case Direction.Down:
                
                return _sphere.UserPressedDown && _sphere.DownPushingLocked == false;
            //return _box.Sphere && _box.DownPushingLocked == false;

            case Direction.Left:
                
                return _sphere.UserPressedLeft && _sphere.LeftPushingLocked == false;
            //return _box.Sphere && _box.LeftPushingLocked == false;

            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }

    bool CheckLockedPush(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                
                return _sphere.UpPushingLocked == false;

            case Direction.Right:
                
                return _sphere.RightPushingLocked == false;

            case Direction.Down:
                
                return _sphere.DownPushingLocked == false;

            case Direction.Left:
                
                return _sphere.LeftPushingLocked == false;

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
                _sphere.UpPushingLocked = value;
                break;
            case "SolidRight":
                _sphere.RightPushingLocked = value;
                break;
            case "SolidDown":
                _sphere.DownPushingLocked = value;
                break;
            case "SolidLeft":
                _sphere.LeftPushingLocked = value;
                break;
        }
    }

    public void Push(Direction directionIndex)
    {
        var direction = (Direction)directionIndex;
        switch (direction)
        {
            case Direction.Up:

                _sphere.UpPushingLocked = LockUpPushing;
                break;

            case Direction.Right:

                _sphere.RightPushingLocked = LockRightPushing;
                break;

            case Direction.Down:

                _sphere.DownPushingLocked = LockDownPushing;
                break;

            case Direction.Left:

                _sphere.LeftPushingLocked = LockLeftPushing;
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

                _sphere.UpPushingLocked = true;
                break;

            case Direction.Right:

                _sphere.RightPushingLocked = true;
                break;

            case Direction.Down:

                _sphere.DownPushingLocked = true;
                break;

            case Direction.Left:

                _sphere.LeftPushingLocked = true;
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