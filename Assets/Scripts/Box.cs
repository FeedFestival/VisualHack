using System;
using UnityEngine;
using Assets.Scripts.Types;

public class Box : MonoBehaviour
{
    [SerializeField]
    private Sphere _sphere;
    [SerializeField]
    public Sphere Sphere
    {
        get { return _sphere; }
        set
        {
            _sphere = value;
            if (_sphere == null)
            {
                if (UpperObject == Obstacle.Player) UpperObject = Obstacle.Nothing;
                if (RightObject == Obstacle.Player) RightObject = Obstacle.Nothing;
                if (DownObject == Obstacle.Player) DownObject = Obstacle.Nothing;
                if (LeftObject == Obstacle.Player) LeftObject = Obstacle.Nothing;
            }
        }
    }
    
    [SerializeField]
    private Direction _playerIsIn;
    [SerializeField]
    public Direction PlayerIsIn
    {
        get { return _playerIsIn; }
        set
        {
            _playerIsIn = value;
            switch (_playerIsIn)
            {
                case Direction.Up:

                    UpperObject = Obstacle.Player;
                    break;

                case Direction.Right:

                    RightObject = Obstacle.Player;
                    break;

                case Direction.Down:

                    DownObject = Obstacle.Player;
                    break;

                case Direction.Left:

                    LeftObject = Obstacle.Player;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public Obstacle UpperObject = Obstacle.Nothing;
    public Obstacle RightObject = Obstacle.Nothing;
    public Obstacle DownObject = Obstacle.Nothing;
    public Obstacle LeftObject = Obstacle.Nothing;

    // moving variables

    private Transform _thisTransform;

    private float _time;
    private readonly float _ratio = 0.02f;
    private readonly float _speed = 3f;

    private bool _pushRight;
    private bool _pushLeft;
    private bool _pushUp;
    private bool _pushDown;

    private Vector3 _startMarker;
    private Vector3 _endMarkerRight;
    private Vector3 _endMarkerLeft;
    private Vector3 _endMarkerUp;
    private Vector3 _endMarkerDown;

    // Use this for initialization
    void Awake()
    {
        _thisTransform = transform;
        Transform[] allChildren = _thisTransform.GetComponentsInChildren<Transform>(true);
        foreach (Transform objT in allChildren)
        {
            switch (objT.tag)
            {
                case "BoxLeft":

                    objT.gameObject.GetComponent<BoxCollider>().Initialize(this, Direction.Left);
                    break;

                case "BoxRight":

                    objT.gameObject.GetComponent<BoxCollider>().Initialize(this, Direction.Right);
                    break;

                case "BoxUp":

                    objT.gameObject.GetComponent<BoxCollider>().Initialize(this, Direction.Up);
                    break;

                case "BoxDown":

                    objT.gameObject.GetComponent<BoxCollider>().Initialize(this, Direction.Down);
                    break;
            }
        }
        CalculateNewCoord();
    }

    public void CalculateNewCoord()
    {
        _startMarker = _thisTransform.position;
        _endMarkerRight = new Vector3(_thisTransform.position.x + 1f, _thisTransform.position.y, _thisTransform.position.z);
        _endMarkerLeft = new Vector3(_thisTransform.position.x - 1f, _thisTransform.position.y, _thisTransform.position.z);
        _endMarkerUp = new Vector3(_thisTransform.position.x, _thisTransform.position.y + 1f, _thisTransform.position.z);
        _endMarkerDown = new Vector3(_thisTransform.position.x, _thisTransform.position.y - 1f, _thisTransform.position.z);
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

    void FixedUpdate()
    {
        if (Sphere == null) return;

        if (_pushRight == false && _pushLeft == false && _pushDown == false)
            if (Sphere.UserPressedUp && CheckBoxRestrictions(Direction.Up))
                _pushUp = true;

        if (_pushLeft == false && _pushUp == false && _pushDown == false)
            if (Sphere.UserPressedRight && CheckBoxRestrictions(Direction.Right))
                _pushRight = true;

        if (_pushRight == false && _pushLeft == false && _pushUp == false)
            if (Sphere.UserPressedDown && CheckBoxRestrictions(Direction.Down))
                _pushDown = true;

        if (_pushRight == false && _pushUp == false && _pushDown == false)
            if (Sphere.UserPressedLeft && CheckBoxRestrictions(Direction.Left))
                _pushLeft = true;

        if (_pushUp)
            MoveUpdate(_endMarkerUp, ref _pushUp);

        if (_pushRight)
            MoveUpdate(_endMarkerRight, ref _pushRight);

        if (_pushDown)
            MoveUpdate(_endMarkerDown, ref _pushDown);

        if (_pushLeft)
            MoveUpdate(_endMarkerLeft, ref _pushLeft);
    }

    private bool CheckBoxRestrictions(Direction desiredPushDirection)
    {
        switch (desiredPushDirection)
        {
            case Direction.Up:

                if (DownObject == Obstacle.Player && UpperObject == Obstacle.Nothing)
                    return true;
                break;

            case Direction.Right:

                if (LeftObject == Obstacle.Player && RightObject == Obstacle.Nothing)
                    return true;
                break;

            case Direction.Down:

                if (UpperObject == Obstacle.Player && DownObject == Obstacle.Nothing)
                    return true;
                break;

            case Direction.Left:

                if (RightObject == Obstacle.Player && LeftObject == Obstacle.Nothing)
                    return true;
                break;

            default:
                throw new ArgumentOutOfRangeException("desiredPushDirection", desiredPushDirection, null);
        }
        return false;
    }
}