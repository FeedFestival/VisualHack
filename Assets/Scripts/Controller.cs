using System;
using Assets.Scripts.Types;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Sphere _sphere;

    private Transform _thisTransform;

    // Input logic variables

    private bool _pushRight;
    private bool _pushLeft;
    private bool _pushUp;
    private bool _pushDown;

    // predefined
    private float _lerpTime;
    private Vector3 _startMarker;
    private Vector3 _endMarkerRight;
    private Vector3 _endMarkerLeft;
    private Vector3 _endMarkerUp;
    private Vector3 _endMarkerDown;

    public Obstacle UpperObject = Obstacle.Nothing;
    public Obstacle RightObject = Obstacle.Nothing;
    public Obstacle DownObject = Obstacle.Nothing;
    public Obstacle LeftObject = Obstacle.Nothing;

    public void Initialize(Sphere obj)
    {
        _sphere = obj;

        _thisTransform = transform;

        CalculateNewCoord();
    }

    private void MoveUpdate(Vector3 endMarker, ref bool referenceBool)
    {
        _thisTransform.position = Vector3.Lerp(_startMarker, endMarker, _lerpTime);
        _lerpTime = _lerpTime + Logic.LerpRatio * Logic.LerpSpeed;

        if (!(_lerpTime >= 1)) return;

        _lerpTime = 0f;
        _thisTransform.position = endMarker;
        CalculateNewCoord();
        referenceBool = false;
    }

    private bool CheckUserPress(Direction direction)
    {
        if (_sphere.FallInPit)
            return false;

        switch (direction)
        {
            case Direction.Up:

                if (_sphere.UserPressedUp)
                {
                    if (UpperObject == Obstacle.Nothing)
                        return true;
                    if (UpperObject == Obstacle.Box && _sphere.UpBox.UpperObject == Obstacle.Nothing)
                        return true;
                }
                return false;

            case Direction.Right:

                if (_sphere.UserPressedRight)
                {
                    if (RightObject == Obstacle.Nothing)
                        return true;
                    if (RightObject == Obstacle.Box && _sphere.RightBox.RightObject == Obstacle.Nothing)
                        return true;
                }
                return false;

            case Direction.Down:

                if (_sphere.UserPressedDown)
                {
                    if (DownObject == Obstacle.Nothing)
                        return true;
                    if (DownObject == Obstacle.Box && _sphere.DownBox.DownObject == Obstacle.Nothing)
                        return true;
                }
                return false;

            case Direction.Left:

                if (_sphere.UserPressedLeft)
                {
                    if (LeftObject == Obstacle.Nothing)
                        return true;
                    if (LeftObject == Obstacle.Box && _sphere.LeftBox.LeftObject == Obstacle.Nothing)
                        return true;
                }
                return false;

            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }

    void Update()
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
        if (foreignObjectCollider.CompareTag("BoxUp"))
        {
            DownObject = Obstacle.Box;
        }
        if (foreignObjectCollider.CompareTag("BoxRight"))
        {
            LeftObject = Obstacle.Box;
        }
        if (foreignObjectCollider.CompareTag("BoxDown"))
        {
            UpperObject = Obstacle.Box;
        }
        if (foreignObjectCollider.CompareTag("BoxLeft"))
        {
            RightObject = Obstacle.Box;
        }

        if (foreignObjectCollider.CompareTag("SolidUp"))
        {
            UpperObject = Obstacle.Solid;
        }
        if (foreignObjectCollider.CompareTag("SolidRight"))
        {
            RightObject = Obstacle.Solid;
        }
        if (foreignObjectCollider.CompareTag("SolidDown"))
        {
            DownObject = Obstacle.Solid;
        }
        if (foreignObjectCollider.CompareTag("SolidLeft"))
        {
            LeftObject = Obstacle.Solid;
        }
    }

    void OnTriggerExit(Collider foreignObjectCollider)
    {
        if (foreignObjectCollider.CompareTag("BoxUp"))
        {
            DownObject = Obstacle.Nothing;
        }
        if (foreignObjectCollider.CompareTag("BoxRight"))
        {
            LeftObject = Obstacle.Nothing;
        }
        if (foreignObjectCollider.CompareTag("BoxDown"))
        {
            UpperObject = Obstacle.Nothing;
        }
        if (foreignObjectCollider.CompareTag("BoxLeft"))
        {
            RightObject = Obstacle.Nothing;
        }

        if (foreignObjectCollider.CompareTag("SolidUp"))
        {
            UpperObject = Obstacle.Nothing;
        }
        if (foreignObjectCollider.CompareTag("SolidRight"))
        {
            RightObject = Obstacle.Nothing;
        }
        if (foreignObjectCollider.CompareTag("SolidDown"))
        {
            DownObject = Obstacle.Nothing;
        }
        if (foreignObjectCollider.CompareTag("SolidLeft"))
        {
            LeftObject = Obstacle.Nothing;
        }
    }
}