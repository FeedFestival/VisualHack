using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;

public class Sphere : MonoBehaviour
{
    private Main _main;

    public Controller Controller;

    public Move GoDirection;

    // user input
    public bool UserPressedRight;
    public bool UserPressedLeft;
    public bool UserPressedUp;
    public bool UserPressedDown;

    // locks
    public bool RightPushingLocked;
    public bool LeftPushingLocked;
    public bool UpPushingLocked;
    public bool DownPushingLocked;

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
            UserPressedRight = true;
        if (Input.GetKeyUp(KeyCode.D))
            UserPressedRight = false;

        if (Input.GetKeyDown(KeyCode.A))
            UserPressedLeft = true;
        if (Input.GetKeyUp(KeyCode.A))
            UserPressedLeft = false;

        if (Input.GetKeyDown(KeyCode.W))
            UserPressedUp = true;
        if (Input.GetKeyUp(KeyCode.W))
            UserPressedUp = false;

        if (Input.GetKeyDown(KeyCode.S))
            UserPressedDown = true;
        if (Input.GetKeyUp(KeyCode.S))
            UserPressedDown = false;
    }
}