using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class Sphere : MonoBehaviour
{
    private Main _main;

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

    public void Initialize(Main main)
    {
        _main = main;
        Controller = GetComponent<Controller>();
        Controller.Initialize(this);
    }

    public void MoveDirection(int moveIndex)
    {
        GoDirection = (Move)moveIndex;
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

    public void StopDirection(int moveIndex)
    {
        if (GoDirection == (Move)moveIndex)
            GoDirection = Move.None;
        switch ((Move)moveIndex)
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
            MoveDirection(1);
        if (Input.GetKeyUp(KeyCode.D))
            StopDirection(1);

        if (Input.GetKeyDown(KeyCode.A))
            MoveDirection(3);
        if (Input.GetKeyUp(KeyCode.A))
            StopDirection(3);

        if (Input.GetKeyDown(KeyCode.W))
            MoveDirection(0);
        if (Input.GetKeyUp(KeyCode.W))
            StopDirection(0);

        if (Input.GetKeyDown(KeyCode.S))
            MoveDirection(2);
        if (Input.GetKeyUp(KeyCode.S))
            StopDirection(2);
    }
}