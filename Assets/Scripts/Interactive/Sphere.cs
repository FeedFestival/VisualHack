using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;
using System.Collections.Generic;

public class Sphere : MonoBehaviour
{
    public Main Main;
    
    [SerializeField]
    public List<Move> directionList;

    public Move GoDirection;

    // user input

    public ButtonState ControllerRight;
    public ButtonState ControllerLeft;
    public ButtonState ControllerUp;
    public ButtonState ControllerDown;

    // boxes
    [HideInInspector]
    public Box UpBox;
    [HideInInspector]
    public Box RightBox;
    [HideInInspector]
    public Box DownBox;
    [HideInInspector]
    public Box LeftBox;

    private SpriteRenderer _sprite;

    private Transform _thisTransform;

    // Input logic variables

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

    public bool IsMoving;

    public bool Win;

    private bool _fallInPit;
    public bool FallInPit
    {
        get { return _fallInPit; }
        set
        {
            _fallInPit = value;
            if (!_fallInPit) return;
        }
    }

    public void Initialize(Main main)
    {
        Main = main;
        
        _thisTransform = transform;

        Transform[] allChildren = transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform objT in allChildren)
        {
            switch (objT.tag)
            {
                case "Sprite":

                    _sprite = objT.gameObject.GetComponent<SpriteRenderer>();
                    break;
            }
        }

        ControllerRight = ButtonState.Released;
        ControllerLeft = ButtonState.Released;
        ControllerUp = ButtonState.Released;
        ControllerDown = ButtonState.Released;

        MoveTowards(Move.None);

        CalculateNewCoord();
    }

    public void MoveDirection(Move moveIndex)
    {
        if (CheckButtonRestrictions(moveIndex))
            if (IsMoving && GoDirection != moveIndex)
            {
                AddToDirections(moveIndex);
            }
            else
            {
                MoveTowards(moveIndex);
            }
    }

    public void StopDirection(Move moveIndex)
    {
        RemoveFromDirections(moveIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            MoveDirection(Move.Right);
        if (Input.GetKeyUp(KeyCode.D))
            StopDirection(Move.Right);

        if (Input.GetKeyDown(KeyCode.A))
            MoveDirection(Move.Left);
        if (Input.GetKeyUp(KeyCode.A))
            StopDirection(Move.Left);

        if (Input.GetKeyDown(KeyCode.W))
            MoveDirection(Move.Up);
        if (Input.GetKeyUp(KeyCode.W))
            StopDirection(Move.Up);

        if (Input.GetKeyDown(KeyCode.S))
            MoveDirection(Move.Down);
        if (Input.GetKeyUp(KeyCode.S))
            StopDirection(Move.Down);

        if (IsMoving)
            switch (GoDirection)
            {
                case Move.Up:

                    if (UpBox)
                        PushBox(Move.Up);

                    MoveUpdate(_endMarkerUp);
                    break;

                case Move.Right:

                    MoveUpdate(_endMarkerRight);
                    break;

                case Move.Down:

                    MoveUpdate(_endMarkerDown);
                    break;

                case Move.Left:

                    MoveUpdate(_endMarkerLeft);
                    break;
            }

        if (FallInPit)
            FallInPitAnim();
    }

    private void MoveUpdate(Vector3 endMarker)
    {
        _thisTransform.position = Vector3.Lerp(_startMarker, endMarker, _lerpTime);
        _lerpTime = _lerpTime + Utils.LerpRatio * Utils.LerpSpeed;

        if (!(_lerpTime >= 1)) return;

        if (CheckRestrictions())
            GoDirection = GetLastDirection();

        _thisTransform.position = endMarker;
        CalculateNewCoord();
    }

    private float _scaleLerpTime;

    private void FallInPitAnim(bool outside = false)
    {
        transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0.85f, 0.85f, 1), _scaleLerpTime * 4);
        _sprite.color = Color.Lerp(Color.white, new Color32(199, 199, 199, 255), _scaleLerpTime * 4);

        _scaleLerpTime = _scaleLerpTime + Utils.LerpRatio * Utils.LerpSpeed;

        if (!(_scaleLerpTime >= 1)) return;

        _scaleLerpTime = 0;

        Main.FacebookController.LoadInterstitial();

        //Main.InitGame(Win ? Main.GetNextMapId() : 0);

        FallInPit = false;
    }

    public void YouDeadBro()
    {
        FallInPit = true;
    }

    public void YouWinBro()
    {
        FallInPit = true;
        Win = true;
    }

    private void MoveTowards(Move moveDirection)
    {
        if (moveDirection == Move.None)
            directionList = new List<Move>();
        else
            AddToDirections(moveDirection);

        GoDirection = moveDirection;

        IsMoving = moveDirection == Move.None ? false : true;
    }

    private void PushBox(Move move)
    {
        switch (move)
        {
            case Move.Up:

                if (UpBox != null)
                    UpBox.PushBox(move);
                break;

            case Move.Right:

                if (RightBox != null)
                    RightBox.PushBox(move);
                break;

            case Move.Down:

                if (DownBox != null)
                    DownBox.PushBox(move);
                break;

            case Move.Left:

                if (LeftBox != null)
                    LeftBox.PushBox(move);
                break;

            case Move.None:
                break;
            default:
                throw new ArgumentOutOfRangeException("move", move, null);
        }
    }

    private void CalculateNewCoord()
    {
        _lerpTime = 0f;

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
        //Debug.Log(foreignObjectCollider);
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

    private bool CheckButtonRestrictions(Move moveIndex)
    {
        switch (moveIndex)
        {
            case Move.Up:
                if (ControllerUp != ButtonState.Disabled)
                    return true;
                break;
            case Move.Right:

                if (ControllerRight != ButtonState.Disabled)
                    return true;
                break;
            case Move.Down:

                if (ControllerDown != ButtonState.Disabled)
                    return true;
                break;
            case Move.Left:
                if (ControllerLeft != ButtonState.Disabled)
                    return true;
                break;
            default:
                throw new ArgumentOutOfRangeException("moveIndex", moveIndex, null);
        }
        return false;
    }

    private bool CheckRestrictions()
    {
        var ret = true;
        if (UpperObject != Obstacle.Nothing)
        {
            ControllerUp = ButtonState.Disabled;
            if (GetLastDirection() == Move.Up)
                ret = false;
        }
        else
        {
            ControllerUp = ButtonState.Released;
        }
        if (RightObject != Obstacle.Nothing)
        {
            ControllerRight = ButtonState.Disabled;
            if (GetLastDirection() == Move.Right)
                ret = false;
        }
        else
        {
            ControllerRight = ButtonState.Released;
        }
        if (DownObject != Obstacle.Nothing)
        {
            ControllerDown = ButtonState.Disabled;
            if (GetLastDirection() == Move.Down)
                ret = false;
        }
        else
        {
            ControllerDown = ButtonState.Released;
        }
        if (LeftObject != Obstacle.Nothing)
        {
            ControllerLeft = ButtonState.Disabled;
            if (GetLastDirection() == Move.Left)
                ret = false;
        }
        else
        {
            ControllerLeft = ButtonState.Released;
        }

        if (!ret)
            MoveTowards(Move.None);

        return ret;
    }

    private void AddToDirections(Move moveDir)
    {
        if (directionList == null)
            directionList = new List<Move>();

        if (directionList.Contains(moveDir))
            return;

        directionList.Add(moveDir);
    }

    private void RemoveFromDirections(Move moveDir)
    {
        if (!directionList.Contains(moveDir))
            return;

        directionList.Remove(moveDir);
    }

    private Move GetLastDirection()
    {
        if (directionList.Count < 1)
        {
            MoveTowards(Move.None);
            return Move.None;
        }

        return directionList[directionList.Count - 1];
    }
}