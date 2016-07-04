using System;
using Assets.Scripts.Types;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Main _main;

    private Transform _sphereTransform;

    public Move GoDirection;

    // Input logic variables
    private float _time;
    private readonly float _ratio = 0.02f;
    private readonly float _speed = 3f;

    // user input
    private bool _userPressedRight;
    private bool _userPressedLeft;
    private bool _userPressedUp;
    private bool _userPressedDown;

    public bool RightPushingLocked;
    private bool _pushRight;
    public bool LeftPushingLocked;
    private bool _pushLeft;
    public bool UpPushingLocked;
    private bool _pushUp;
    public bool DownPushingLocked;
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

    public void Initialize(Main main)
    {
        _main = main;

        _sphereTransform = transform;

        CalculateNewCoord();
    }

    public void SelectControllerType(int controllerType)
    {
        _main.ControllerType = (ControllerType)controllerType;
        _main.DataService.UpdateUserControllerType(controllerType);

        _main.ButtonClicked((int)ButtonClick.SettingsBackButton);
    }

    private void MoveUpdate(Vector3 endMarker, ref bool referenceBool)
    {
        _sphereTransform.position = Vector3.Lerp(_startMarker, endMarker, _time);
        _time = _time + _ratio * _speed;

        if (!(_time >= 1)) return;

        _time = 0f;
        _sphereTransform.position = endMarker;
        CalculateNewCoord();
        referenceBool = false;
    }


    public void MoveDirection(int moveIndex)
    {
        GoDirection = (Move)moveIndex;
        switch (GoDirection)
        {
            case Move.Up:

                _userPressedUp = true;
                break;

            case Move.Right:

                _userPressedRight = true;
                break;

            case Move.Down:

                _userPressedDown = true;
                break;

            case Move.Left:

                _userPressedLeft = true;
                break;
        }
    }

    public void StopDirection(int moveIndex)
    {
        switch ((Move)moveIndex)
        {
            case Move.Up:

                _userPressedUp = false;
                break;

            case Move.Right:

                _userPressedRight = false;
                break;

            case Move.Down:

                _userPressedDown = false;
                break;

            case Move.Left:

                _userPressedLeft = false;
                break;
        }
    }

    void FixedUpdate()
    {
        if (_pushLeft == false && _pushUp == false && _pushDown == false)
            if ((Input.GetKey(KeyCode.D) || _userPressedRight) && RightPushingLocked == false)
                _pushRight = true;

        if (_pushRight == false && _pushUp == false && _pushDown == false)
            if ((Input.GetKey(KeyCode.A) || _userPressedLeft) && LeftPushingLocked == false)
                _pushLeft = true;

        if (_pushRight == false && _pushLeft == false && _pushDown == false)
            if ((Input.GetKey(KeyCode.W) || _userPressedUp) && UpPushingLocked == false)
                _pushUp = true;

        if (_pushRight == false && _pushLeft == false && _pushUp == false)
            if ((Input.GetKey(KeyCode.S) || _userPressedDown) && DownPushingLocked == false)
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
        _startMarker = _sphereTransform.position;
        _endMarkerRight = new Vector3(_sphereTransform.position.x + 1f, _sphereTransform.position.y, _sphereTransform.position.z);
        _endMarkerLeft = new Vector3(_sphereTransform.position.x - 1f, _sphereTransform.position.y, _sphereTransform.position.z);
        _endMarkerUp = new Vector3(_sphereTransform.position.x, _sphereTransform.position.y + 1f, _sphereTransform.position.z);
        _endMarkerDown = new Vector3(_sphereTransform.position.x, _sphereTransform.position.y - 1f, _sphereTransform.position.z);
    }

    /***************************\//----------------------------------------------\
    *                           												*
    * The Colliding effects 													*
    * 					        												*
    *                           												*
    \***************************/
    // ---------------------------------------------\

    void OnTriggerEnter(Collider obj)
    {
        LockPushing(obj.gameObject.name, true);
        
        if (obj.gameObject.name == "right_Side")
        {
            PushingFromRight = true;
        }
        if (obj.gameObject.name == "left_Side")
        {
            PushingFromLeft = true;
        }
        if (obj.gameObject.name == "up_Side")
        {
            PushingFromUp = true;
        }
        if (obj.gameObject.name == "down_Side")
        {
            PushingFromDown = true;
        }
    }
    void OnTriggerExit(Collider obj)
    {
        LockPushing(obj.gameObject.name, false);
        
        if (obj.gameObject.name == "right_Side")
        {
            PushingFromRight = false;
        }
        if (obj.gameObject.name == "left_Side")
        {
            PushingFromLeft = false;
        }
        if (obj.gameObject.name == "up_Side")
        {
            PushingFromUp = false;
        }
        if (obj.gameObject.name == "down_Side")
        {
            PushingFromDown = false;
        }
    }

    private void LockPushing(string goName, bool value)
    {
        switch (goName)
        {
            case "BorderUp":
                UpPushingLocked = value;
                break;
            case "BorderRight":
                RightPushingLocked = value;
                break;
            case "BorderDown":
                DownPushingLocked = value;
                break;
            case "BorderLeft":
                LeftPushingLocked = value;
                break;
        }
    }

    public void Push(Direction directionIndex)
    {
        var direction = (Direction)directionIndex;
        switch (direction)
        {
            case Direction.Up:

                UpPushingLocked = LockUpPushing != false;
                break;

            case Direction.Right:
                
                RightPushingLocked = LockRightPushing != false;
                break;

            case Direction.Down:

                DownPushingLocked = LockDownPushing != false;
                break;

            case Direction.Left:

                LeftPushingLocked = LockLeftPushing != false;
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

                UpPushingLocked = true;
                break;

            case Direction.Right:

                RightPushingLocked = true;
                break;

            case Direction.Down:

                DownPushingLocked = true;
                break;

            case Direction.Left:

                LeftPushingLocked = true;
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

    public void Death()
    {
        Debug.Log("Sphere - Play_Animation (Death by Water).\n");

        RightPushingLocked = true;
        LeftPushingLocked = true;
        UpPushingLocked = true;
        DownPushingLocked = true;
    }
}