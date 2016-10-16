using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;
using System.Collections.Generic;

public class Sphere : MonoBehaviour
{
    public Main Main;

    [SerializeField]
    public List<Move> DirectionList;

    public Move GoDirection;

    // user input

    public ButtonState ControllerRight;
    public ButtonState ControllerLeft;
    public ButtonState ControllerUp;
    public ButtonState ControllerDown;

    // boxes
    [HideInInspector]
    public Box UpperBox;
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

    private readonly List<Obstacle> _boxSurroundingObjects = new List<Obstacle> { Obstacle.Solid, Obstacle.Box };

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

                    if (UpperBox)
                        PushBox(Move.Up);

                    MoveUpdate(_endMarkerUp);
                    break;

                case Move.Right:

                    if (RightBox)
                        PushBox(Move.Right);

                    MoveUpdate(_endMarkerRight);
                    break;

                case Move.Down:

                    if (DownBox)
                        PushBox(Move.Down);

                    MoveUpdate(_endMarkerDown);
                    break;

                case Move.Left:

                    if (LeftBox)
                        PushBox(Move.Left);

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
        // when the animation ends

        _thisTransform.position = endMarker;

        CheckSurroundings();

        if (CheckRestrictions())
            GoDirection = GetLastDirection();

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
            DirectionList = new List<Move>();
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

                if (UpperBox != null)
                    UpperBox.PushBox(move);
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

    private void AddToDirections(Move moveDir)
    {
        if (DirectionList == null)
            DirectionList = new List<Move>();

        if (DirectionList.Contains(moveDir))
            return;

        DirectionList.Add(moveDir);
    }

    private void RemoveFromDirections(Move moveDir)
    {
        if (!DirectionList.Contains(moveDir))
            return;

        DirectionList.Remove(moveDir);
    }

    private Move GetLastDirection()
    {
        if (DirectionList.Count < 1)
        {
            MoveTowards(Move.None);
            return Move.None;
        }

        return DirectionList[DirectionList.Count - 1];
    }












    /***************************\//----------------------------------------------\
    *                           												*
    * The Colliding effects 													*
    * 					        												*
    *                           												*
    \***************************/
    // ---------------------------------------------\

    public void CheckSurroundings()
    {
        var oldX = Mathf.RoundToInt(_startMarker.x);
        var oldY = Mathf.RoundToInt(_startMarker.y);
        Main.Tiles[oldX, oldY] = new MapTile
        {
            X = oldX,
            Y = oldY,
            TyleType = TileType.Empty
        };


        var x = Mathf.RoundToInt(_thisTransform.position.x);
        var y = Mathf.RoundToInt(_thisTransform.position.y);
        Main.Tiles[x, y] = new MapTile
        {
            X = x,
            Y = y,
            TyleType = TileType.PuzzleObject,
            PuzzleObject = PuzzleObject.Player
        };

        // UP
        if (y + 1 > Main.MaxY)
            UpperObject = Obstacle.Solid;
        else
        {
            var upperTile = Main.Tiles[x, y + 1];

            if (upperTile == null || upperTile.TyleType == TileType.Solid)
                UpperObject = Obstacle.Solid;
            else if (upperTile.TyleType == TileType.PuzzleObject && upperTile.PuzzleObject == PuzzleObject.Box)
            {
                UpperBox = upperTile.box;
            }
            else
                UpperObject = Obstacle.Nothing;

            if (UpperBox != null)
            {
                UpperBox.CheckSurroundings();
                if (_boxSurroundingObjects.Contains(UpperBox.UpperObject))
                    UpperObject = Obstacle.Solid;
                else
                    UpperObject = Obstacle.Box;
            }
        }

        // RIGHT
        if (x + 1 > Main.MaxX)
            RightObject = Obstacle.Solid;
        else
        {
            var rightTile = Main.Tiles[x + 1, y];

            if (rightTile == null || rightTile.TyleType == TileType.Solid)
                RightObject = Obstacle.Solid;
            else if (rightTile.TyleType == TileType.PuzzleObject && rightTile.PuzzleObject == PuzzleObject.Box)
            {
                RightBox = rightTile.box;
            }
            else
                RightObject = Obstacle.Nothing;

            if (RightBox != null)
            {
                RightBox.CheckSurroundings();
                if (_boxSurroundingObjects.Contains(RightBox.RightObject))
                    RightObject = Obstacle.Solid;
                else
                    RightObject = Obstacle.Box;
            }
        }

        // DOWN
        if (y - 1 < 0)
            DownObject = Obstacle.Solid;
        else
        {
            var downTile = Main.Tiles[x, y - 1];

            if (downTile == null || downTile.TyleType == TileType.Solid)
                DownObject = Obstacle.Solid;
            else if (downTile.TyleType == TileType.PuzzleObject && downTile.PuzzleObject == PuzzleObject.Box)
            {
                DownBox = downTile.box;
            }
            else
                DownObject = Obstacle.Nothing;

            if (DownBox != null)
            {
                DownBox.CheckSurroundings();
                if (_boxSurroundingObjects.Contains(DownBox.DownObject))
                    DownObject = Obstacle.Solid;
                else
                    DownObject = Obstacle.Box;
            }
        }

        // LEFT
        if (x - 1 < 0)
            LeftObject = Obstacle.Solid;
        else
        {
            var leftTile = Main.Tiles[x - 1, y];

            if (leftTile == null || leftTile.TyleType == TileType.Solid)
                LeftObject = Obstacle.Solid;
            else if (leftTile.TyleType == TileType.PuzzleObject && leftTile.PuzzleObject == PuzzleObject.Box)
            {
                LeftBox = leftTile.box;
            }
            else
                LeftObject = Obstacle.Nothing;

            if (LeftBox != null)
            {
                LeftBox.CheckSurroundings();
                if (_boxSurroundingObjects.Contains(LeftBox.LeftObject))
                    LeftObject = Obstacle.Solid;
                else
                    LeftObject = Obstacle.Box;
            }
        }
    }

    void OnTriggerExit(Collider foreignObjectCollider)
    {
        if (foreignObjectCollider.CompareTag("BoxUp"))
        {
            if (DownBox != null)
            {
                DownBox.CheckSurroundings();
                DownBox = null;
            }
            DownObject = Obstacle.Nothing;
        }
        if (foreignObjectCollider.CompareTag("BoxRight"))
        {
            if (LeftBox != null)
            {
                LeftBox.CheckSurroundings();
                LeftBox = null;
            }
            LeftObject = Obstacle.Nothing;
        }
        if (foreignObjectCollider.CompareTag("BoxDown"))
        {
            if (UpperBox != null)
            {
                UpperBox.CheckSurroundings();
                UpperBox = null;
            }
            UpperObject = Obstacle.Nothing;
        }
        if (foreignObjectCollider.CompareTag("BoxLeft"))
        {
            if (RightBox != null)
            {
                RightBox.CheckSurroundings();
                RightBox = null;
            }
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

    public bool CheckRestrictions()
    {
        var ret = true;
        if (UpperObject == Obstacle.Solid)
        {
            ControllerUp = ButtonState.Disabled;
            if (GetLastDirection() == Move.Up)
                ret = false;
        }
        else
        {
            ControllerUp = ButtonState.Released;
        }
        if (RightObject == Obstacle.Solid)
        {
            ControllerRight = ButtonState.Disabled;
            if (GetLastDirection() == Move.Right)
                ret = false;
        }
        else
        {
            ControllerRight = ButtonState.Released;
        }
        if (DownObject == Obstacle.Solid)
        {
            ControllerDown = ButtonState.Disabled;
            if (GetLastDirection() == Move.Down)
                ret = false;
        }
        else
        {
            ControllerDown = ButtonState.Released;
        }
        if (LeftObject == Obstacle.Solid)
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
}