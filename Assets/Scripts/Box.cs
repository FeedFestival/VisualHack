using System;
using UnityEngine;
using Assets.Scripts.Types;

public class Box : MonoBehaviour
{
    public GameObject Pic;

    // Parent
    public Controller Player;
    
    // Use this for initialization
    void Awake()
    {
        CalculateNewCoord();

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
    }

    // Input from user
    public bool RightPushingLocked = true;
    private bool _userPressedRight;
    private bool _pushRight;
    public bool LeftPushingLocked = true;
    private bool _userPressedLeft;
    private bool _pushLeft;
    public bool UpPushingLocked = true;
    private bool _userPressedUp;
    private bool _pushUp;
    public bool DownPushingLocked = true;
    private bool _userPressedDown;
    private bool _pushDown;

    // Input logic variables
    private float _time;
    private readonly float _ratio = 0.02f;
    private readonly float _speed = 3f;


    private Vector3 _startMarker;
    private Vector3 _endMarkerRight;
    private Vector3 _endMarkerLeft;
    private Vector3 _endMarkerUp;
    private Vector3 _endMarkerDown;


    public bool LockRightPushing;
    public bool LockLeftPushing;
    public bool LockUpPushing;
    public bool LockDownPushing;

    void FixedUpdate()
    {
        // Input

        if (RightPushingLocked == false)
        {
            if ((_pushLeft == false) &&
                (_pushUp == false) &&
                (_pushDown == false))
            {
                if ((Input.GetKey(KeyCode.D)) || (_userPressedRight))
                {

                    _pushRight = true;
                }
            }
        }
        if (LeftPushingLocked == false)
        {
            if ((_pushRight == false) &&
                (_pushUp == false) &&
                (_pushDown == false))
            {
                if ((Input.GetKey(KeyCode.A)) || (_userPressedLeft))
                {

                    _pushLeft = true;
                }
            }
        }
        if (UpPushingLocked == false)
        {
            if ((_pushRight == false) &&
                (_pushLeft == false) &&
                (_pushDown == false))
            {
                if ((Input.GetKey(KeyCode.W)) || (_userPressedUp))
                {

                    _pushUp = true;
                }

            }
        }
        if (DownPushingLocked == false)
        {
            if ((_pushRight == false) &&
                (_pushLeft == false) &&
                (_pushUp == false))
            {
                if ((Input.GetKey(KeyCode.S)) || (_userPressedDown))
                {

                    _pushDown = true;
                }
            }
        }
        // Logic

        if (_pushRight)
        {
            transform.position = Vector3.Lerp(_startMarker, _endMarkerRight, _time);
            _time = _time + _ratio * _speed;

            if (_time >= 1)
            {
                _time = 0f;
                transform.position = _endMarkerRight;
                CalculateNewCoord();
                _pushRight = false;

            }
        }
        if (_pushLeft)
        {
            transform.position = Vector3.Lerp(_startMarker, _endMarkerLeft, _time);
            _time = _time + _ratio * _speed;

            if (_time >= 1)
            {
                _time = 0f;
                transform.position = _endMarkerLeft;
                CalculateNewCoord();
                _pushLeft = false;
            }
        }
        if (_pushUp)
        {
            transform.position = Vector3.Lerp(_startMarker, _endMarkerUp, _time);
            _time = _time + _ratio * _speed;

            if (_time >= 1)
            {
                _time = 0f;
                transform.position = _endMarkerUp;
                CalculateNewCoord();
                _pushUp = false;
            }
        }
        if (_pushDown)
        {
            transform.position = Vector3.Lerp(_startMarker, _endMarkerDown, _time);
            _time = _time + _ratio * _speed;

            if (_time >= 1)
            {
                _time = 0f;
                transform.position = _endMarkerDown;
                CalculateNewCoord();
                _pushDown = false;
            }
        }
    }

    private void CalculateNewCoord()
    {
        _startMarker = transform.position;
        _endMarkerRight = new Vector3(transform.position.x + 1f,
                                 transform.position.y,
                                 transform.position.z);
        _endMarkerLeft = new Vector3(transform.position.x - 1f,
                                 transform.position.y,
                                 transform.position.z);
        _endMarkerUp = new Vector3(transform.position.x,
                                 transform.position.y + 1f,
                                 transform.position.z);
        _endMarkerDown = new Vector3(transform.position.x,
                                 transform.position.y - 1f,
                                 transform.position.z);

        if (isDead)
        {
            Pic.transform.parent = null;
            Pic.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            Destroy(gameObject);
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

    //  If Phone.
    public void userPressed(int i, bool n)
    {
        if (i == 6)
        {
            _userPressedRight = n;
        }
        else if (i == 4)
        {
            _userPressedLeft = n;
        }
        else if (i == 8)
        {
            _userPressedUp = n;
        }
        else
        {
            _userPressedDown = n;
        }
    }

    bool isDead;
    public void Death()
    {
        Debug.Log("Cube - Play_Animation (Death by Water).\n");

        // Destroy the side colliders (except the 'Pic') so no other restrictions will be applied to the player.
        foreach (Transform child in transform)
        {
            if (child.GetInstanceID() != Pic.transform.GetInstanceID())
                Destroy(child.gameObject);
        }

        isDead = true;
    }
}
