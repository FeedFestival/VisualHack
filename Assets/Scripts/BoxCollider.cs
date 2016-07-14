using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class BoxCollider : MonoBehaviour
{
    public Direction ColliderPosition;

    private Box _box;

    public void Initialize(Box box, Direction colPosition)
    {
        _box = box;
        ColliderPosition = colPosition;
    }

    void OnTriggerEnter(Collider objC)
    {
        if (objC.CompareTag("Sphere"))
        {
            _box.Sphere = objC.transform.parent.GetComponent<Sphere>();
            
            _box.PlayerIsIn = ColliderPosition;
        }
        
        switch (ColliderPosition)
        {
            case Direction.Up:

                switch (objC.tag)
                {
                    case "BoxDown":

                        _box.UpperObject = Obstacle.Box;
                        break;

                    case "SolidUp":

                        _box.UpperObject = Obstacle.Solid;
                        break;
                }
                break;

            case Direction.Right:

                switch (objC.tag)
                {
                    case "BoxLeft":

                        _box.RightObject = Obstacle.Box;
                        break;

                    case "SolidRight":

                        _box.RightObject = Obstacle.Solid;
                        break;
                }
                break;

            case Direction.Down:

                switch (objC.tag)
                {
                    case "BoxUp":

                        _box.DownObject = Obstacle.Box;
                        break;

                    case "SolidDown":

                        _box.DownObject = Obstacle.Solid;
                        break;
                }
                break;

            case Direction.Left:

                switch (objC.tag)
                {
                    case "BoxRight":

                        _box.LeftObject = Obstacle.Box;
                        break;

                    case "SolidLeft":

                        _box.LeftObject = Obstacle.Solid;
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
            _box.Sphere = null;
        }

        switch (ColliderPosition)
        {
            case Direction.Up:

                _box.UpperObject = Obstacle.Nothing;
                break;

            case Direction.Right:

                _box.RightObject = Obstacle.Nothing;
                break;

            case Direction.Down:

                _box.DownObject = Obstacle.Nothing;
                break;

            case Direction.Left:

                _box.LeftObject = Obstacle.Nothing;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}