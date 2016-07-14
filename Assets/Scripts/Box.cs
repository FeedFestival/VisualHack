using System;
using UnityEngine;
using Assets.Scripts.Utils;

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
            if (value == null)
            {
                if (UpperObject == Obstacle.Sphere)
                {
                    UpperObject = Obstacle.Nothing;
                    Sphere.DownBox = null;
                }
                if (RightObject == Obstacle.Sphere)
                {
                    RightObject = Obstacle.Nothing;
                    Sphere.LeftBox = null;
                }
                if (DownObject == Obstacle.Sphere)
                {
                    DownObject = Obstacle.Nothing;
                    Sphere.UpBox = null;
                }
                if (LeftObject == Obstacle.Sphere)
                {
                    LeftObject = Obstacle.Nothing;
                    Sphere.RightBox = null;
                }
            }
            _sphere = value;
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

                    UpperObject = Obstacle.Sphere;
                    Sphere.DownBox = this;
                    break;

                case Direction.Right:

                    RightObject = Obstacle.Sphere;
                    Sphere.LeftBox = this;
                    break;

                case Direction.Down:

                    DownObject = Obstacle.Sphere;
                    Sphere.UpBox = this;
                    break;

                case Direction.Left:

                    LeftObject = Obstacle.Sphere;
                    Sphere.RightBox = this;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public bool IsBoxDead;

    // children

    private BoxCollider _boxColliderUp;
    private BoxCollider _boxColliderRight;
    private BoxCollider _boxColliderDown;
    private BoxCollider _boxColliderLeft;

    private SpriteRenderer _boxSprite;

    private Transform _triggerCollider;

    public Obstacle UpperObject = Obstacle.Nothing;
    public Obstacle RightObject = Obstacle.Nothing;
    public Obstacle DownObject = Obstacle.Nothing;
    public Obstacle LeftObject = Obstacle.Nothing;

    // moving variables

    private Transform _thisTransform;

    private bool _pushRight;
    private bool _pushLeft;
    private bool _pushUp;
    private bool _pushDown;

    private float _lerpTime;
    private Vector3 _startMarker;
    private Vector3 _endMarkerRight;
    private Vector3 _endMarkerLeft;
    private Vector3 _endMarkerUp;
    private Vector3 _endMarkerDown;

    // Use this for initialization
    public void Initialize()
    {
        _thisTransform = transform;
        Transform[] allChildren = _thisTransform.GetComponentsInChildren<Transform>(true);
        foreach (Transform objT in allChildren)
        {
            switch (objT.tag)
            {
                case "BoxLeft":

                    _boxColliderLeft = objT.gameObject.GetComponent<BoxCollider>();
                    _boxColliderLeft.Initialize(this, Direction.Left);
                    break;

                case "BoxRight":

                    _boxColliderRight = objT.gameObject.GetComponent<BoxCollider>();
                    _boxColliderRight.Initialize(this, Direction.Right);
                    break;

                case "BoxUp":

                    _boxColliderUp = objT.gameObject.GetComponent<BoxCollider>();
                    _boxColliderUp.Initialize(this, Direction.Up);
                    break;

                case "BoxDown":

                    _boxColliderDown = objT.gameObject.GetComponent<BoxCollider>();
                    _boxColliderDown.Initialize(this, Direction.Down);
                    break;

                case "Sprite":

                    _boxSprite = objT.gameObject.GetComponent<SpriteRenderer>();
                    break;
            }

            if (string.Equals(objT.gameObject.name, "TriggerCollider"))
                _triggerCollider = objT;
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
        _thisTransform.position = Vector3.Lerp(_startMarker, endMarker, _lerpTime);
        _lerpTime = _lerpTime + Utils.LerpRatio * Utils.LerpSpeed;

        if (!(_lerpTime >= 1)) return;

        if (IsBoxDead)
            YouDeadBro(true);

        _lerpTime = 0f;
        _thisTransform.position = endMarker;
        CalculateNewCoord();
        referenceBool = false;
    }

    void Update()
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

        if (IsBoxDead)
            FallInPit();
    }

    private bool CheckBoxRestrictions(Direction desiredPushDirection)
    {
        switch (desiredPushDirection)
        {
            case Direction.Up:

                if (DownObject == Obstacle.Sphere && UpperObject == Obstacle.Nothing)
                    return true;
                break;

            case Direction.Right:

                if (LeftObject == Obstacle.Sphere && RightObject == Obstacle.Nothing)
                    return true;
                break;

            case Direction.Down:

                if (UpperObject == Obstacle.Sphere && DownObject == Obstacle.Nothing)
                    return true;
                break;

            case Direction.Left:

                if (RightObject == Obstacle.Sphere && LeftObject == Obstacle.Nothing)
                    return true;
                break;

            default:
                throw new ArgumentOutOfRangeException("desiredPushDirection", desiredPushDirection, null);
        }
        return false;
    }

    public void YouDeadBro(bool forReal = false)
    {
        IsBoxDead = true;

        if (forReal)
        {
            Destroy(_boxColliderUp.gameObject);
            Destroy(_boxColliderRight.gameObject);
            Destroy(_boxColliderDown.gameObject);
            Destroy(_boxColliderLeft.gameObject);

            Destroy(_triggerCollider.gameObject);

            if (UpperObject == Obstacle.Sphere)
            {
                Sphere.DownBox = null;
                Sphere.Controller.DownObject = Obstacle.Nothing;
            }
            else if (RightObject == Obstacle.Sphere)
            {
                Sphere.LeftBox = null;
                Sphere.Controller.LeftObject = Obstacle.Nothing;
            }
            else if (DownObject == Obstacle.Sphere)
            {
                Sphere.UpBox = null;
                Sphere.Controller.UpperObject = Obstacle.Nothing;
            }
            else if (LeftObject == Obstacle.Sphere)
            {
                Sphere.RightBox = null;
                Sphere.Controller.RightObject = Obstacle.Nothing;
            }
            
            _boxSprite.transform.localPosition = new Vector3(_boxSprite.transform.localPosition.x, _boxSprite.transform.localPosition.y, 8f);

            Destroy(this);
        }
    }

    private float _scaleLerpTime;

    private void FallInPit()
    {
        _thisTransform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0.85f, 0.85f, 1), _scaleLerpTime * 4);
        _boxSprite.color = Color.Lerp(Color.white, new Color32(199, 199, 199, 255), _scaleLerpTime * 4);
        
        _scaleLerpTime = _scaleLerpTime + Utils.LerpRatio * Utils.LerpSpeed;

        if (!(_scaleLerpTime >= 1)) return;

        _scaleLerpTime = 0;
    }
}