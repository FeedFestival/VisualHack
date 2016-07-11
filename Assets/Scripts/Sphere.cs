using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class Sphere : MonoBehaviour
{
    public Main Main;

    [HideInInspector]
    public Controller Controller;

    public Move GoDirection;

    // user input
    [HideInInspector]
    public bool UserPressedRight;
    [HideInInspector]
    public bool UserPressedLeft;
    [HideInInspector]
    public bool UserPressedUp;
    [HideInInspector]
    public bool UserPressedDown;

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

    public void Initialize(Main main)
    {
        Main = main;
        Controller = GetComponent<Controller>();
        Controller.Initialize(this);

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
    }

    public void MoveDirection(Move moveIndex)
    {
        GoDirection = moveIndex;
        switch (GoDirection)
        {
            case Move.Up:

                UserPressedUp = true;
                break;

            case Move.Right:

                UserPressedRight = true;
                break;

            case Move.Down:

                UserPressedDown = true;
                break;

            case Move.Left:

                UserPressedLeft = true;
                break;
        }
    }

    public void StopDirection(Move moveIndex)
    {
        if (GoDirection == moveIndex)
            GoDirection = Move.None;
        switch (moveIndex)
        {
            case Move.Up:

                UserPressedUp = false;
                break;

            case Move.Right:

                UserPressedRight = false;
                break;

            case Move.Down:

                UserPressedDown = false;
                break;

            case Move.Left:

                UserPressedLeft = false;
                break;
        }
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

        if (FallInPit)
            FallInPitAnim();
    }

    public bool Win;
    public bool FallInPit;

    private float _scaleLerpTime;

    private void FallInPitAnim(bool outside = false)
    {
        transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(0.85f, 0.85f, 1), _scaleLerpTime * 4);
        _sprite.color = Color.Lerp(Color.white, new Color32(199, 199, 199, 255), _scaleLerpTime * 4);

        _scaleLerpTime = _scaleLerpTime + Logic.LerpRatio * Logic.LerpSpeed;

        if (!(_scaleLerpTime >= 1)) return;

        _scaleLerpTime = 0;

        if (Win)
            Main.InitGame(Main.GetNextMapId());
        else
            Main.InitGame();
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
}