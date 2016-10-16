using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class BoxCollider : MonoBehaviour
{
    public Direction ColliderPosition;

    public Box Box;

    public void Initialize(Box box, Direction colPosition)
    {
        Box = box;
        ColliderPosition = colPosition;
    }

    void OnTriggerEnter(Collider objC)
    {
        if (objC.CompareTag("Sphere"))
        {
            Box.Sphere = objC.transform.parent.GetComponent<Sphere>();
            
            Box.PlayerIsIn = ColliderPosition;
        }
        
        switch (ColliderPosition)
        {
            case Direction.Up:

                switch (objC.tag)
                {
                    case "BoxDown":

                        Box.UpperObject = Obstacle.Box;
                        break;

                    case "SolidUp":

                        Box.UpperObject = Obstacle.Solid;
                        break;
                }
                break;

            case Direction.Right:

                switch (objC.tag)
                {
                    case "BoxLeft":

                        Box.RightObject = Obstacle.Box;
                        break;

                    case "SolidRight":

                        Box.RightObject = Obstacle.Solid;
                        break;
                }
                break;

            case Direction.Down:

                switch (objC.tag)
                {
                    case "BoxUp":

                        Box.DownObject = Obstacle.Box;
                        break;

                    case "SolidDown":

                        Box.DownObject = Obstacle.Solid;
                        break;
                }
                break;

            case Direction.Left:

                switch (objC.tag)
                {
                    case "BoxRight":

                        Box.LeftObject = Obstacle.Box;
                        break;

                    case "SolidLeft":

                        Box.LeftObject = Obstacle.Solid;
                        break;
                }
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void OnTriggerExit(Collider objC)
    {
        if (objC.CompareTag("Sphere"))
        {
            Box.Sphere = null;
        }

        switch (ColliderPosition)
        {
            case Direction.Up:

                Box.UpperObject = Obstacle.Nothing;
                break;

            case Direction.Right:

                Box.RightObject = Obstacle.Nothing;
                break;

            case Direction.Down:

                Box.DownObject = Obstacle.Nothing;
                break;

            case Direction.Left:

                Box.LeftObject = Obstacle.Nothing;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}